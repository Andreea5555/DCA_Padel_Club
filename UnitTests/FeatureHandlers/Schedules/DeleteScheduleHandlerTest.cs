using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;
using UnitTests.Fakes;
using UnitTests.Fakes.ScheduleFakes;

namespace UnitTests.FeatureHandlers.Schedules;

public class DeleteScheduleHandlerTest
{
    [Fact]
    public async Task TestDeleteSchedule_Success()
    {
        Schedule schedule = Schedule.Create();

        ICurrentDate currentDate = new FakeCurrentDate(new DateOnly(2026, 4, 1));

        FakeScheduleRepository repository = new FakeScheduleRepository();
        await repository.AddAsync(schedule);

        IUnitOfWork unitOfWork = new FakeUnitOfWork();

        Result<DeleteScheduleCommand> commandResult =
            DeleteScheduleCommand.Create(schedule.Id.Id.ToString());
        
        DeleteScheduleCommand command = commandResult.value;

        ICommandHandler<DeleteScheduleCommand> handler =
            new DeleteScheduleHandler(
                unitOfWork,
                repository,
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
        
        Result<DeleteScheduleCommand> commandResult =
            DeleteScheduleCommand.Create(Guid.NewGuid().ToString());

        DeleteScheduleCommand command = commandResult.value;

        ICommandHandler<DeleteScheduleCommand> handler =
            new DeleteScheduleHandler(
                unitOfWork,
                repository,
                currentDate);

        Result<None> result = await handler.HandleAsync(command);

        Assert.True(result.IsFailure);
    }
}