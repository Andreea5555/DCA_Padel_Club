using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Application.Features;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedule;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;
using UnitTests.Fakes;
using UnitTests.Fakes.ScheduleFakes;
using UnitTests.Helpers;
using System.Reflection;
using Xunit;

namespace DCA_Padel_Club.Tests.FeatureHandlers;

public class CreateBookingHandlerTests
{
    [Fact]
    public async Task Handle_ValidInput_ReturnsSuccess()
    {
        // Arrange
        Schedule schedule = Schedule.Create();

        ICurrentDate setupDate = new FakeCurrentDate(new DateOnly(2026, 4, 1));
        ICurrentTime setupTime = new FakeCurrentTime(new TimeOnly(10, 0));
        IActiveScheduleOnDate activeScheduleChecker = new FakeActiveScheduleOnDate(false);

        schedule.UpdateScheduledDate(new DateOnly(2026, 4, 10), setupDate);
        schedule.UpdateScheduledTimes(new TimeOnly(15, 0), new TimeOnly(22, 0));
        schedule.AddCourt(new CourtId("D1"), setupDate);
        schedule.ActivateSchedule(activeScheduleChecker, setupDate, setupTime);

        FakeScheduleRepository repository = new FakeScheduleRepository();
        await repository.AddAsync(schedule);

        IUnitOfWork unitOfWork = new FakeUnitOfWork();
        ICurrentDate currentDate = new FakeCurrentDate(new DateOnly(2026, 4, 1));
        ICurrentTime currentTime = new FakeCurrentTime(new TimeOnly(10, 0));

        var commandResult = CreateBookingCommand.Create(
            schedule.Id.Id.ToString(),
            123456,
            "D1",
            "2026-04-10",
            "16:00",
            "17:00");
        CreateBookingCommand command = commandResult.value;

        //TODO fix this after you asked TROELS
        ICommandHandler<CreateBookingCommand> handler =
            new CreateBookingHandler(repository, unitOfWork, currentDate, currentTime);

        // Act
        Result<None> result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(GetBookings(schedule));
    }

    [Fact]
    public async Task Handle_ScheduleNotFound_ReturnsFailure()
    {
        // Arrange
        FakeScheduleRepository repository = new FakeScheduleRepository();
        IUnitOfWork unitOfWork = new FakeUnitOfWork();
        ICurrentDate currentDate = new FakeCurrentDate(new DateOnly(2026, 4, 1));
        ICurrentTime currentTime = new FakeCurrentTime(new TimeOnly(10, 0));

        var commandResult = CreateBookingCommand.Create(
            Guid.NewGuid().ToString(),
            123456,
            "D1",
            "2026-04-10",
            "16:00",
            "17:00");

        CreateBookingCommand command = commandResult.value;

        //TODO same here
        ICommandHandler<CreateBookingCommand> handler =
            new CreateBookingHandler(repository, unitOfWork, currentDate, currentTime);

        // Act
        Result<None> result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsFailure);
    }

    private static IList<Booking> GetBookings(Schedule schedule)
    {
        var field = typeof(Schedule).GetField("Bookings", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(field);
        return (IList<Booking>)field!.GetValue(schedule)!;
    }
}
