using Xunit;

namespace ModularMonolith.ArchitectureTests;

public sealed class ArchitectureRulesTests
{
    [Fact]
    public void Orders_Domain_Does_Not_Reference_User_Or_Product_Domain()
    {
        AssertSource("Modules/Orders", ["Users.Domain", "Products.Domain"]);
    }

    [Fact]
    public void Users_And_Products_Do_Not_Depend_On_Orders()
    {
        AssertSource("Modules/Users", ["Modules.Orders"]);
        AssertSource("Modules/Products", ["Modules.Orders"]);
    }

    [Fact]
    public void Domain_Entities_Are_Internal()
    {
        AssertSource(
            "Modules",
            [
                "public sealed class User",
                "public sealed class Product",
                "public sealed class Order ",
                "public sealed class OrderItem",
            ]);
    }

    [Fact]
    public void Controllers_Are_In_Module_Api_Namespaces()
    {
        var controllerFiles = Directory
            .GetFiles(PathToReferenceRoot(), "*Controller.cs", SearchOption.AllDirectories)
            .Where(file => file.Contains("Modules", StringComparison.Ordinal));

        foreach (var controllerFile in controllerFiles)
        {
            Assert.Contains(".Api.Controllers", File.ReadAllText(controllerFile));
        }
    }

    [Fact]
    public void Module_DbContexts_Are_Not_Referenced_By_Other_Modules()
    {
        AssertSource("Modules/Orders", ["UsersDbContext", "ProductsDbContext"]);
        AssertSource("Modules/Users", ["OrdersDbContext"]);
        AssertSource("Modules/Products", ["OrdersDbContext"]);
    }

    [Fact]
    public void Host_Contains_Composition_Only()
    {
        var programPath = Path.Combine(PathToReferenceRoot(), "ModularMonolith.Reference.Api/Program.cs");
        var program = File.ReadAllText(programPath);

        Assert.DoesNotContain("DbContext", program);
        Assert.DoesNotContain("Repository", program);
    }

    private static void AssertSource(string relativePath, string[] forbiddenTerms)
    {
        var files = Directory.GetFiles(
            Path.Combine(PathToReferenceRoot(), relativePath),
            "*.cs",
            SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var text = File.ReadAllText(file);

            foreach (var forbiddenTerm in forbiddenTerms)
            {
                Assert.DoesNotContain(forbiddenTerm, text);
            }
        }
    }

    private static string PathToReferenceRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null && !Directory.Exists(Path.Combine(directory.FullName, "Modules")))
        {
            directory = directory.Parent;
        }

        return directory!.FullName;
    }
}
