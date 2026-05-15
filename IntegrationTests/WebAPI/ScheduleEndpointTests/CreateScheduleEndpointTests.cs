using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTests.WebAPI.ScheduleEndpointTests;

public class CreateScheduleEndpointTests
{
    [Fact]
    public async Task CreateSchedule_ShouldReturnNoContent()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                "/api/schedules/create",
                new { });

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}