using System.Net;
using System.Net.Http.Json;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.WebAPI.ScheduleEndpointTests;

public class UpdateScheduleTimeEndpointTest
{
    [Fact]
    public async Task UpdateScheduleTime_InvalidScheduleId_ShouldReturnInternalServerError()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                "/api/schedules/not-a-guid/update-time",
                new { StartTime = "10:00:00", EndTime = "12:00:00" });

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task UpdateScheduleTime_InvalidTime_ShouldReturnInternalServerError()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                $"/api/schedules/{Guid.NewGuid()}/update-time",
                new { StartTime = "not-a-time", EndTime = "12:00:00" });

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task UpdateScheduleTime_ScheduleDoesNotExist_ShouldReturnBadRequest()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                $"/api/schedules/{Guid.NewGuid()}/update-time",
                new { StartTime = "10:00:00", EndTime = "12:00:00" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateScheduleTime_StartTimeAfterEndTime_ShouldReturnBadRequest()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        Schedule schedule = Schedule.Create();
        await SaveScheduleAsync(factory, schedule);

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                $"/api/schedules/{schedule.Id.Id}/update-time",
                new { StartTime = "13:00:00", EndTime = "12:00:00" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateScheduleTime_ValidInput_ShouldReturnNoContent()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        Schedule schedule = Schedule.Create();
        await SaveScheduleAsync(factory, schedule);

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                $"/api/schedules/{schedule.Id.Id}/update-time",
                new { StartTime = "10:00:00", EndTime = "12:00:00" });

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private static async Task SaveScheduleAsync(
        WebApplicationFactory<Program> factory,
        Schedule schedule)
    {
        using IServiceScope scope = factory.Services.CreateScope();

        EfcDbContext context =
            scope.ServiceProvider.GetRequiredService<EfcDbContext>();

        context.Schedules.Add(schedule);
        await context.SaveChangesAsync();
    }
}