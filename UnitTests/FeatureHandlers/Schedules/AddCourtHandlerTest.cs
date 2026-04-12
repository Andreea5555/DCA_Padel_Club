using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;
using UnitTests.Fakes;
using UnitTests.Fakes.ScheduleFakes;

namespace UnitTests.FeatureHandlers.Schedules;

using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

public class AddCourtHandlerTest
{
    
    [Fact]
    public async Task TestAddCourtHandler_Success()
    {
        Schedule schedule = Schedule.Create();
        var courtId = CourtId.CreateCourtId("S2");

        ICurrentDate currentDate = new FakeCurrentDate(new DateOnly(2026, 4, 1));
        FakeScheduleRepository repository = new FakeScheduleRepository();
        await repository.AddAsync(schedule);

        IUnitOfWork unitOfWork = new FakeUnitOfWork();
        
        Result<AddCourtCommand> commandResult =
            AddCourtCommand.Create(schedule.Id.Id.ToString(), courtId.ToString());
        
        AddCourtCommand command = commandResult.value;

        ICommandHandler<AddCourtCommand> handler =
            new AddCourtHandler(
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

        Result<AddCourtCommand> commandResult =
            AddCourtCommand.Create(Guid.NewGuid().ToString(),"S2" );

        AddCourtCommand command = commandResult.value;

        ICommandHandler<AddCourtCommand> handler =
            new AddCourtHandler(
                repository,
                unitOfWork,
                currentDate);

        Result<None> result = await handler.HandleAsync(command);

        Assert.True(result.IsFailure);
    }
}