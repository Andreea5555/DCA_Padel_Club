using System.Net;
using System.Net.Http.Json;
using DCA_Padel_Club.Core.Tools.OperationResult;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTests.WebAPI.PlayerEndpointTests;

public class CreatePlayerEndpointTests
{
    [Fact]
    public async Task CreatePlayer_ValidInput_ReturnsNoContent()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/players/create",
            new
            {
                PlayerId = 123456,
                FirstName = "Alice",
                LastName = "Smith",
                Email = "123456@via.dk",
                Password = "secret123",
                ProfilePictureUri = "https://example.com/profile.png"
            });

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var savedPlayer = await PlayerEndpointTestHelpers.GetPlayerAsync(factory, 123456);
        Assert.NotNull(savedPlayer);
        Assert.Equal("Alice", savedPlayer!.FirstName.Value);
        Assert.Equal("123456@via.dk", savedPlayer.Email.Value);
    }

    [Fact]
    public async Task CreatePlayer_InvalidEmail_ReturnsBadRequest()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/players/create",
            new
            {
                PlayerId = 123456,
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice@gmail.com",
                Password = "secret123",
                ProfilePictureUri = "https://example.com/profile.png"
            });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        OperationError[]? errors = await response.Content.ReadFromJsonAsync<OperationError[]>();
        Assert.NotNull(errors);
        Assert.Contains(errors!, error => error.ErrorCode == "Email.WrongDomain");
    }
}

