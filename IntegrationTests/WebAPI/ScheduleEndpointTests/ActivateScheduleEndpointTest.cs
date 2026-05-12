using System.Net;
using System.Net.Http.Json;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.WebAPI.ScheduleEndpointTests;

public class ActivateScheduleEndpointTest
{
    [Fact]
    public async Task ActivateSchedule_InvalidId_ShouldReturnInternalServerError()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response =
            await client.PostAsync("/api/schedules/not-a-guid/activate", JsonContent.Create(new { }));

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task ActivateSchedule_ScheduleDoesNotExist_ShouldReturnBadRequest()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response =
            await client.PostAsync(
                $"/api/schedules/{Guid.NewGuid()}/activate",
                JsonContent.Create(new { }));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ActivateSchedule_ExistingScheduleWithoutCourt_ShouldReturnBadRequest()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        Schedule schedule = Schedule.Create();
        await SaveScheduleAsync(factory, schedule);

        HttpResponseMessage response =
            await client.PostAsync(
                $"/api/schedules/{schedule.Id.Id}/activate",
                JsonContent.Create(new { }));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ActivateSchedule_ExistingScheduleWithCourt_ShouldReturnNoContent()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        Schedule schedule = CreateValidScheduleWithCourt();
        await SaveScheduleAsync(factory, schedule);

        HttpResponseMessage response =
            await client.PostAsync(
                $"/api/schedules/{schedule.Id.Id}/activate",
                JsonContent.Create(new { }));

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private static Schedule CreateValidScheduleWithCourt()
    {
        Schedule schedule = Schedule.Create();

        schedule.UpdateScheduledDate(
            DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            new TestCurrentDate());

        schedule.AddCourt(
            new CourtId("Court 1"),
            new TestCurrentDate());

        return schedule;
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

    private class TestCurrentDate : ICurrentDate
    {
        public DateOnly Now => DateOnly.FromDateTime(DateTime.Today);
    }
}