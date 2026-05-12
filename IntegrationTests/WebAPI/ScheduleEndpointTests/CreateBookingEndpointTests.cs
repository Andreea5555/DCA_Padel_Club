using System.Net;
using System.Net.Http.Json;
using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;
using IntegrationTests.Fakes;
using IntegrationTests.Fakes.Schedule;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.WebAPI.ScheduleEndpointTests;

public class CreateBookingEndpointTests
{
    [Fact]
    public async Task CreateBooking_InvalidScheduleId_ShouldReturnInternalServerError()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                "/api/schedules/not-a-guid/create-booking",
                new
                {
                    BookerId = 1,
                    CourtId = "Court 1",
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)).ToString("yyyy-MM-dd"),
                    StartTime = "10:00:00",
                    EndTime = "11:00:00"
                });

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task CreateBooking_ScheduleDoesNotExist_ShouldReturnBadRequest()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                $"/api/schedules/{Guid.NewGuid()}/create-booking",
                new
                {
                    BookerId = 1,
                    CourtId = "Court 1",
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)).ToString("yyyy-MM-dd"),
                    StartTime = "10:00:00",
                    EndTime = "11:00:00"
                });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateBooking_ScheduleWithoutCourt_ShouldReturnBadRequest()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        Schedule schedule = CreateActiveSchedule();
        await SaveScheduleAsync(factory, schedule);

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                $"/api/schedules/{schedule.Id.Id}/create-booking",
                new
                {
                    BookerId = 1,
                    CourtId = "Court 1",
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)).ToString("yyyy-MM-dd"),
                    StartTime = "10:00:00",
                    EndTime = "11:00:00"
                });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateBooking_ValidInput_ShouldReturnNoContent()
    {
        await using WebApplicationFactory<Program> factory = new DCAWebApplicationFactory();
        HttpClient client = factory.CreateClient();

        Schedule schedule = CreateActiveSchedule();

        schedule.AddCourt(
            new CourtId("Court 1"),
            new FakeCurrentDate());

        await SavePlayerAsync(factory, 1);

        await SaveScheduleAsync(factory, schedule);

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                $"/api/schedules/{schedule.Id.Id}/create-booking",
                new
                {
                    BookerId = 1,
                    CourtId = "Court 1",
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)).ToString("yyyy-MM-dd"),
                    StartTime = "16:00:00",
                    EndTime = "17:00:00"
                });

        // Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        string content = await response.Content.ReadAsStringAsync();

        Assert.True(
            response.StatusCode == HttpStatusCode.NoContent,
            $"Expected NoContent but got {response.StatusCode}. Response body: {content}");
    }

    private static Schedule CreateActiveSchedule()
    {
        Schedule schedule = Schedule.Create();

        schedule.UpdateScheduledDate(
            DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            new FakeCurrentDate());

        schedule.UpdateScheduledTimes(
            TimeOnly.Parse("15:00:00"),
            TimeOnly.Parse("22:00:00"));

        schedule.AddCourt(
            new CourtId("Court 1"),
            new FakeCurrentDate());

        schedule.ActivateSchedule(
            new FakeActiveScheduleOnDate(false),
            new FakeCurrentDate(),
            new FakeCurrentTime(DateTime.Now));

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

    private static async Task SavePlayerAsync(
        WebApplicationFactory<Program> factory,
        int playerId)
    {
        using IServiceScope scope = factory.Services.CreateScope();

        EfcDbContext context =
            scope.ServiceProvider.GetRequiredService<EfcDbContext>();

        Result<Player> playerResult = Player.Register(
            new ViaId(playerId),
            "John",
            "Doe",
            $"JHV@via.dk",
            "Password123",
            "profile.jpg");

        if (playerResult.IsFailure)
        {
            throw new Exception(
                string.Join(", ", playerResult.errorMessages.Select(e => e.ErrorMessage)));
        }

        context.Players.Add(playerResult.value);
        await context.SaveChangesAsync();
    }
}
