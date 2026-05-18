using System.Net;
using System.Net.Http.Json;
using DCA_Padel_Club.Core.Tools.OperationResult;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTests.WebAPI.PlayerEndpointTests;

public class ManagePlayerStatusEndpointTests
{
    [Fact]
    public async Task ManagePlayerStatus_BlacklistExistingPlayer_ReturnsNoContent_AndUpdatesPlayer()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        await PlayerEndpointTestHelpers.CreateAndSavePlayerAsync(factory, 123456);

        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/players/123456/manage-status",
            new
            {
                Action = "Blacklist"
            });

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var updatedPlayer = await PlayerEndpointTestHelpers.GetPlayerAsync(factory, 123456);
        Assert.NotNull(updatedPlayer);
        Assert.True(updatedPlayer!.Blacklisted);
    }

    [Fact]
    public async Task ManagePlayerStatus_InvalidAction_ReturnsBadRequest()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        await PlayerEndpointTestHelpers.CreateAndSavePlayerAsync(factory, 123456);

        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/players/123456/manage-status",
            new
            {
                Action = "Freeze"
            });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        OperationError[]? errors = await response.Content.ReadFromJsonAsync<OperationError[]>();
        Assert.NotNull(errors);
        Assert.Contains(errors!, error => error.ErrorCode == "PlayerStatus.Action.Invalid");
    }
}

