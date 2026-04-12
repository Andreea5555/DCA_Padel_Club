using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;
using UnitTests.Fakes;
using UnitTests.Fakes.ScheduleFakes;

namespace UnitTests.FeatureHandlers.Schedules;

using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
public class CreateBookingHandlerTests
{
    [Fact]
    public async Task Handle_ValidInput_ReturnsSuccess()
    {
        
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

        Result<CreateBookingCommand> commandResult = CreateBookingCommand.Create(
            schedule.Id.Id.ToString(),
            123456,
            "D1",
            "2026-04-10",
            "16:00",
            "17:00");

        CreateBookingCommand command = commandResult.value;

        ICommandHandler<CreateBookingCommand> handler =
            new CreateBookingHandler(repository, unitOfWork, currentDate, currentTime);

        Result<None> result = await handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ScheduleNotFound_ReturnsFailure()
    {
        FakeScheduleRepository repository = new FakeScheduleRepository();
        IUnitOfWork unitOfWork = new FakeUnitOfWork();
        ICurrentDate currentDate = new FakeCurrentDate(new DateOnly(2026, 4, 1));
        ICurrentTime currentTime = new FakeCurrentTime(new TimeOnly(10, 0));

        Result<CreateBookingCommand> commandResult = CreateBookingCommand.Create(
            Guid.NewGuid().ToString(),
            123456,
            "D1",
            "2026-04-10",
            "16:00",
            "17:00");

        CreateBookingCommand command = commandResult.value;

        ICommandHandler<CreateBookingCommand> handler =
            new CreateBookingHandler(repository, unitOfWork, currentDate, currentTime);

        Result<None> result = await handler.HandleAsync(command);

        Assert.True(result.IsFailure);
    }
}