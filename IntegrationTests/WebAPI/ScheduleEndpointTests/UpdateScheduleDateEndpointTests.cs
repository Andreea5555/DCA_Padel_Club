using System.Net;
using System.Net.Http.Json;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.WebAPI.ScheduleEndpointTests;

public class UpdateScheduleDateEndpointTests
{
    [Fact]
    public async Task UpdateScheduleDate_InvalidScheduleId_ShouldReturnInternalServerError()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                "/api/schedules/not-a-guid/update-date",
                new { Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)).ToString("yyyy-MM-dd") });

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task UpdateScheduleDate_InvalidDate_ShouldReturnInternalServerError()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                $"/api/schedules/{Guid.NewGuid()}/update-date",
                new { Date = "not-a-date" });

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task UpdateScheduleDate_ScheduleDoesNotExist_ShouldReturnBadRequest()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                $"/api/schedules/{Guid.NewGuid()}/update-date",
                new { Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)).ToString("yyyy-MM-dd") });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateScheduleDate_DateInPast_ShouldReturnBadRequest()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        Schedule schedule = Schedule.Create();
        await SaveScheduleAsync(factory, schedule);

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                $"/api/schedules/{schedule.Id.Id}/update-date",
                new { Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)).ToString("yyyy-MM-dd") });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateScheduleDate_ValidInput_ShouldReturnNoContent()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        Schedule schedule = Schedule.Create();
        await SaveScheduleAsync(factory, schedule);

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                $"/api/schedules/{schedule.Id.Id}/update-date",
                new { Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)).ToString("yyyy-MM-dd") });

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