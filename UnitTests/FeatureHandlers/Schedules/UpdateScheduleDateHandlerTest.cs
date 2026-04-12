using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;
using UnitTests.Fakes;
using UnitTests.Fakes.ScheduleFakes;

namespace UnitTests.FeatureHandlers.Schedules;

public class UpdateScheduleDateHandlerTest
{
    [Fact]
    public async Task Handle_ValidInput_ReturnsSuccess()
    {
        Schedule schedule = Schedule.Create();

        ICurrentDate currentDate = new FakeCurrentDate(new DateOnly(2026, 4, 1));

        FakeScheduleRepository repository = new FakeScheduleRepository();
        await repository.AddAsync(schedule);

        IUnitOfWork unitOfWork = new FakeUnitOfWork();

        Result<UpdateScheduleDateCommand> commandResult =
            UpdateScheduleDateCommand.Create(
                schedule.Id.Id.ToString(),
                "2026-04-10");

        UpdateScheduleDateCommand command = commandResult.value;

        ICommandHandler<UpdateScheduleDateCommand> handler =
            new UpdateScheduleDateHandler(
                repository,
                unitOfWork,
                currentDate);

        Result<None> result = await handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ScheduleNotFound_ReturnsFailure()
    {
        FakeScheduleRepository repository = new FakeScheduleRepository();
        IUnitOfWork unitOfWork = new FakeUnitOfWork();
        ICurrentDate currentDate = new FakeCurrentDate(new DateOnly(2026, 4, 1));

        Result<UpdateScheduleDateCommand> commandResult =
            UpdateScheduleDateCommand.Create(
                Guid.NewGuid().ToString(),
                "2026-04-10");

        UpdateScheduleDateCommand command = commandResult.value;

        ICommandHandler<UpdateScheduleDateCommand> handler =
            new UpdateScheduleDateHandler(
                repository,
                unitOfWork,
                currentDate);

        Result<None> result = await handler.HandleAsync(command);

        Assert.True(result.IsFailure);
    }
}