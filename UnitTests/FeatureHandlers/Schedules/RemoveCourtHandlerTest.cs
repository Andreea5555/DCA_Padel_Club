using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;
using UnitTests.Fakes;
using UnitTests.Fakes.ScheduleFakes;

namespace UnitTests.FeatureHandlers.Schedules;

public class RemoveCourtHandlerTest
{
    
    
    [Fact]
    public async Task Handle_ValidInput_ReturnsSuccess()
    {
        Schedule schedule = Schedule.Create();
        schedule.AddCourt(new CourtId("S2"), new FakeCurrentDate(new DateOnly(2026, 4, 1)));

        FakeScheduleRepository repository = new FakeScheduleRepository();
        await repository.AddAsync(schedule);

        IUnitOfWork unitOfWork = new FakeUnitOfWork();

        Result<RemoveCourtCommand> commandResult =
            RemoveCourtCommand.Create(schedule.Id.Id.ToString(), "S2");

        RemoveCourtCommand command = commandResult.value;

        ICommandHandler<RemoveCourtCommand> handler =
            new RemoveCourtHandler(repository, unitOfWork);

        Result<None> result = await handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ScheduleNotFound_ReturnsFailure()
    {
        FakeScheduleRepository repository = new FakeScheduleRepository();
        IUnitOfWork unitOfWork = new FakeUnitOfWork();

        Result<RemoveCourtCommand> commandResult =
            RemoveCourtCommand.Create(Guid.NewGuid().ToString(), "S2");

        RemoveCourtCommand command = commandResult.value;

        ICommandHandler<RemoveCourtCommand> handler =
            new RemoveCourtHandler(repository, unitOfWork);

        Result<None> result = await handler.HandleAsync(command);

        Assert.True(result.IsFailure);
    }
}