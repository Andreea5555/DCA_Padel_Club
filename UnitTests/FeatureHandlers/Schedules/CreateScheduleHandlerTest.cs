using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Application.Features;
using DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;
using UnitTests.Fakes;
using UnitTests.Fakes.ScheduleFakes;

namespace UnitTests.FeatureHandlers.Schedules;

public class CreateScheduleHandlerTest
{

    [Fact]
    public async Task Handle_CreatesSchedule_ReturnsSuccess()
    {
        // Arrange
        var repository = new FakeScheduleRepository();
        IUnitOfWork unitOfWork = new FakeUnitOfWork();

        ICommandHandler<CreateScheduleCommand> handler =
            new CreateScheduleHandler(repository, unitOfWork);

        var commandResult = CreateScheduleCommand.Create();
        CreateScheduleCommand command = commandResult.value;

        // Act
        Result<None> result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(repository.Schedules);
    }
}
