using System.Net;
using System.Net.Http.Json;
using DCA_Padel_Club.Core.QueryContracts.Queries;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTests.WebAPI.PlayerEndpointTests;

public class PlayerPageEndpointTests
{
    [Fact]
    public async Task PlayerPage_ReturnsSeededPlayerPage()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        await PlayerEndpointTestHelpers.CreateAndSavePlayerAsync(
            factory,
            123456,
            firstName: "Alice",
            lastName: "Smith",
            email: "123456@via.dk",
            profilePictureUri: "https://example.com/alice.png");

        HttpResponseMessage response = await client.GetAsync("/api/players/123456");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        PlayerPage.Answer? answer = await response.Content.ReadFromJsonAsync<PlayerPage.Answer>();
        Assert.NotNull(answer);
        Assert.Equal(123456, answer!.PlayerId);
        Assert.Equal("Alice", answer.FirstName);
        Assert.Equal("Smith", answer.LastName);
        Assert.Equal("123456@via.dk", answer.Email);
        Assert.Equal("https://example.com/alice.png", answer.ProfilePicture);
        Assert.Equal(0, answer.UpcomingBookingsCount);
        Assert.Empty(answer.UpcomingBookings);
        Assert.Empty(answer.PastBookings);
    }
}

