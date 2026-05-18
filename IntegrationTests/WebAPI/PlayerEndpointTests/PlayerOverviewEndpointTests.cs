using System.Net;
using System.Net.Http.Json;
using DCA_Padel_Club.Core.QueryContracts.Queries;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTests.WebAPI.PlayerEndpointTests;

public class PlayerOverviewEndpointTests
{
    [Fact]
    public async Task PlayerOverview_ReturnsSeededPlayers()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        await PlayerEndpointTestHelpers.CreateAndSavePlayerAsync(
            factory,
            123456,
            firstName: "Alice",
            lastName: "Smith",
            email: "123456@via.dk");

        await PlayerEndpointTestHelpers.CreateAndSavePlayerAsync(
            factory,
            234567,
            firstName: "Bob",
            lastName: "Jones",
            email: "234567@via.dk",
            profilePictureUri: "https://example.com/bob.png");

        HttpResponseMessage response = await client.GetAsync("/api/players/overview");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        PlayerOverview.Answer? answer = await response.Content.ReadFromJsonAsync<PlayerOverview.Answer>();
        Assert.NotNull(answer);
        Assert.Equal(2, answer!.Players.Count);
        Assert.Contains(answer.Players, p => p.PlayerId == 123456 && p.FirstName == "Alice" && p.Email == "123456@via.dk");
        Assert.Contains(answer.Players, p => p.PlayerId == 234567 && p.FirstName == "Bob" && p.Email == "234567@via.dk");
    }
}

