using System.Xml.Linq;

namespace Microservices.ArchitectureTests;

public sealed class ArchitectureRulesTests
{
    private static readonly string[] Services =
    [
        "Identity", "Customers", "Catalog", "Pricing", "Inventory", "Ordering", "Payments", "Notifications"
    ];

    private static readonly string Root = FindMicroservicesRoot();

    [Fact]
    public void NoMicroserviceProjectReferencesAnotherMicroservice()
    {
        foreach (var project in ProjectsUnder("Services"))
        {
            var owningService = Services.Single(service => project.Contains($"/Services/{service}/"));
            var forbidden = ProjectReferences(project)
                .Where(reference => Services.Any(service => service != owningService && reference.Contains($"/Services/{service}/")));

            Assert.Empty(forbidden);
        }
    }

    [Fact]
    public void DomainProjectsHaveNoProjectReferences()
    {
        foreach (var project in ProjectsUnder("Services").Where(project => project.EndsWith(".Domain.csproj")))
        {
            Assert.Empty(ProjectReferences(project));
        }
    }

    [Fact]
    public void ApplicationProjectsDoNotReferenceInfrastructureOrApi()
    {
        foreach (var project in ProjectsUnder("Services").Where(project => project.EndsWith(".Application.csproj")))
        {
            var forbidden = ProjectReferences(project)
                .Where(reference => reference.EndsWith(".Infrastructure.csproj") || reference.EndsWith(".Api.csproj"));

            Assert.Empty(forbidden);
        }
    }

    [Fact]
    public void ApiGatewayReferencesNoServiceProject()
    {
        var gateway = Path.Combine(Root, "Gateway", "Microservices.ApiGateway", "Microservices.ApiGateway.csproj");
        Assert.Empty(ProjectReferences(gateway).Where(reference => reference.Contains("/Services/")));
    }

    [Fact]
    public void BuildingBlocksReferencesNoServiceProject()
    {
        var buildingBlocks = Path.Combine(Root, "Shared", "Microservices.BuildingBlocks", "Microservices.BuildingBlocks.csproj");
        Assert.Empty(ProjectReferences(buildingBlocks).Where(reference => reference.Contains("/Services/")));
    }

    [Fact]
    public void ContractsProjectsDoNotContainDomainEntities()
    {
        foreach (var project in ProjectsUnder("Services").Where(project => project.EndsWith(".Contracts.csproj")))
        {
            var projectDirectory = Path.GetDirectoryName(project)!;
            var forbiddenFiles = Directory.EnumerateFiles(projectDirectory, "*.cs", SearchOption.AllDirectories)
                .Where(file => file.Contains("/Entities/") || File.ReadAllText(file).Contains(" class "));

            Assert.Empty(forbiddenFiles);
        }
    }

    [Fact]
    public void EveryDeployableServiceHasItsOwnExecutableProject()
    {
        var expected = Services.Where(service => service != "Notifications")
            .Select(service => $"Microservices.{service}.Api.csproj")
            .Append("Microservices.Notifications.Worker.csproj")
            .Append("Microservices.ApiGateway.csproj");

        var actual = Directory.EnumerateFiles(Root, "*.csproj", SearchOption.AllDirectories)
            .Where(IsExecutableProject)
            .Select(Path.GetFileName);

        Assert.Equal(expected.OrderBy(project => project), actual.OrderBy(project => project));
    }

    [Fact]
    public void NotificationsUsesWorkerProjectAndIsNotHostedInsideOrdering()
    {
        Assert.True(File.Exists(Path.Combine(Root, "Services", "Notifications", "Microservices.Notifications.Worker", "Microservices.Notifications.Worker.csproj")));
        Assert.Empty(Directory.EnumerateFiles(Path.Combine(Root, "Services", "Ordering"), "*Notifications*", SearchOption.AllDirectories));
    }

    [Fact]
    public void NoProjectOutsideAServiceReferencesThatServiceInfrastructureProject()
    {
        foreach (var project in ProjectsUnder("Services"))
        {
            var owningService = Services.Single(service => project.Contains($"/Services/{service}/"));
            var forbidden = ProjectReferences(project)
                .Where(reference => reference.EndsWith(".Infrastructure.csproj") && !reference.Contains($"/Services/{owningService}/"));

            Assert.Empty(forbidden);
        }
    }

    private static IEnumerable<string> ProjectsUnder(string folder) =>
        Directory.EnumerateFiles(Path.Combine(Root, folder), "*.csproj", SearchOption.AllDirectories)
            .Select(Normalize);

    private static IEnumerable<string> ProjectReferences(string projectPath)
    {
        var projectDirectory = Path.GetDirectoryName(projectPath)!;
        var document = XDocument.Load(projectPath);
        return document.Descendants("ProjectReference")
            .Select(element => element.Attribute("Include")?.Value)
            .Where(include => !string.IsNullOrWhiteSpace(include))
            .Select(include => Normalize(Path.GetFullPath(Path.Combine(projectDirectory, include!))));
    }

    private static bool IsExecutableProject(string projectPath)
    {
        var content = File.ReadAllText(projectPath);
        return content.Contains("Microsoft.NET.Sdk.Web") || content.Contains("Microsoft.NET.Sdk.Worker");
    }

    private static string FindMicroservicesRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "MicroservicesArchitecture");
            if (Directory.Exists(candidate)) return Normalize(candidate);
            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate src/MicroservicesArchitecture.");
    }

    private static string Normalize(string path) => Path.GetFullPath(path).Replace('\\', '/');
}
