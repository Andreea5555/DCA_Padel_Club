using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;
using UnitTests.Fakes;
using UnitTests.Fakes.ScheduleFakes;
namespace UnitTests.FeatureHandlers.Schedules;

using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

public class ActivateScheduleHandlerTests
{
     [Fact]
    public async Task Handle_ValidInput_ReturnsSuccess()
    {
        Schedule schedule = Schedule.Create();

        ICurrentDate currentDate = new FakeCurrentDate(new DateOnly(2026, 4, 1));
        ICurrentTime currentTime = new FakeCurrentTime(new TimeOnly(10, 0));
        IActiveScheduleOnDate activeScheduleOnDate = new FakeActiveScheduleOnDate(false);

        schedule.UpdateScheduledDate(new DateOnly(2026, 4, 10), currentDate);
        schedule.UpdateScheduledTimes(new TimeOnly(15, 0), new TimeOnly(22, 0));
        schedule.AddCourt(new CourtId("D1"), currentDate);

        FakeScheduleRepository repository = new FakeScheduleRepository();
        await repository.AddAsync(schedule);

        IUnitOfWork unitOfWork = new FakeUnitOfWork();

        Result<ActivateScheduleCommand> commandResult =
            ActivateScheduleCommand.Create(schedule.Id.ToString());

        ActivateScheduleCommand command = commandResult.value;

        ICommandHandler<ActivateScheduleCommand> handler =
            new ActivateScheduleHandler(
                repository,
                unitOfWork,
                currentDate,
                currentTime,
                activeScheduleOnDate);

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
        IActiveScheduleOnDate activeScheduleOnDate = new FakeActiveScheduleOnDate(false);

        Result<ActivateScheduleCommand> commandResult =
            ActivateScheduleCommand.Create(Guid.NewGuid().ToString());

        ActivateScheduleCommand command = commandResult.value;

        ICommandHandler<ActivateScheduleCommand> handler =
            new ActivateScheduleHandler(
                repository,
                unitOfWork,
                currentDate,
                currentTime,
                activeScheduleOnDate);

        Result<None> result = await handler.HandleAsync(command);

        Assert.True(result.IsFailure);
    }
}