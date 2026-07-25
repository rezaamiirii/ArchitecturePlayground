using System.Net;
using System.Net.Http.Json;
using ModularMonolith.IntegrationTests.Models;
using Xunit;

namespace ModularMonolith.IntegrationTests;

public sealed class UsersApiTests : IClassFixture<Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UsersApiTests(Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program> factory)
    {
        _client = factory
            .WithWebHostBuilder(_ => { })
            .CreateClient();
    }

    [Fact]
    public async Task Create_User_Returns_Created_User_Response()
    {
        var user = await CreateUserAsync("Vertical", "Slice", UniqueEmail());

        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("Vertical", user.FirstName);
        Assert.Equal("Slice", user.LastName);
        Assert.True(user.IsActive);
    }

    [Fact]
    public async Task Create_User_Rejects_Invalid_Email()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/users",
            new { FirstName = "Invalid", LastName = "Email", Email = "not-an-email" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_User_Rejects_Duplicate_Email()
    {
        var email = UniqueEmail();
        await CreateUserAsync("Duplicate", "One", email);

        var response = await _client.PostAsJsonAsync(
            "/api/users",
            new { FirstName = "Duplicate", LastName = "Two", Email = email });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_User_Returns_Existing_User()
    {
        var created = await CreateUserAsync("Find", "Me", UniqueEmail());

        var found = await _client.GetFromJsonAsync<UserDto>($"/api/users/{created.Id}");

        Assert.Equal(created.Id, found!.Id);
        Assert.Equal(created.Email, found.Email);
    }

    [Fact]
    public async Task Get_User_Returns_NotFound_For_Missing_User()
    {
        var response = await _client.GetAsync($"/api/users/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Activate_And_Deactivate_User_Preserve_Routes_And_Response_Contract()
    {
        var created = await CreateUserAsync("Route", "Check", UniqueEmail());

        var deactivateResponse = await _client.PostAsync($"/api/users/{created.Id}/deactivate", content: null);
        var deactivated = await deactivateResponse.Content.ReadFromJsonAsync<UserDto>();

        Assert.Equal(HttpStatusCode.OK, deactivateResponse.StatusCode);
        Assert.False(deactivated!.IsActive);

        var activateResponse = await _client.PostAsync($"/api/users/{created.Id}/activate", content: null);
        var activated = await activateResponse.Content.ReadFromJsonAsync<UserDto>();

        Assert.Equal(HttpStatusCode.OK, activateResponse.StatusCode);
        Assert.True(activated!.IsActive);
    }

    private async Task<UserDto> CreateUserAsync(string firstName, string lastName, string email)
    {
        var response = await _client.PostAsJsonAsync(
            "/api/users",
            new { FirstName = firstName, LastName = lastName, Email = email });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        return (await response.Content.ReadFromJsonAsync<UserDto>())!;
    }

    private static string UniqueEmail()
    {
        return $"user-{Guid.NewGuid():N}@example.com";
    }
}
