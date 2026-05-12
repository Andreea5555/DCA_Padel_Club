using System.Net;
using System.Net.Http.Json;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.WebAPI.ScheduleEndpointTests;

public class RemoveCourtEndpointTests
{
     [Fact]
    public async Task RemoveCourt_InvalidScheduleId_ShouldReturnInternalServerError()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                "/api/schedules/not-a-guid/remove-court",
                new { CourtId = "Court 1" });

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task RemoveCourt_ScheduleDoesNotExist_ShouldReturnBadRequest()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                $"/api/schedules/{Guid.NewGuid()}/remove-court",
                new { CourtId = "Court 1" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task RemoveCourt_ExistingSchedule_ShouldReturnNoContent()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        Schedule schedule = Schedule.Create();

        schedule.UpdateScheduledDate(
            DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            new TestCurrentDate());

        schedule.AddCourt(
            new CourtId("Court 1"),
            new TestCurrentDate());

        await SaveScheduleAsync(factory, schedule);

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                $"/api/schedules/{schedule.Id.Id}/remove-court",
                new { CourtId = "Court 1" });

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

    private class TestCurrentDate : DCA_Padel_Club.Core.Domain.Common.Contracts.ICurrentDate
    {
        public DateOnly Now => DateOnly.FromDateTime(DateTime.Today);
    }
}