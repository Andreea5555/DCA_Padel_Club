using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.PlayerCommands;
using DCA_Padel_Club.Core.Application.Features.PlayerFeatures;
using PlayerAggregate = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player;
using ViaId = DCA_Padel_Club.Core.Domain.Aggregates.Players.ViaId;
using DCA_Padel_Club.Core.Tools.OperationResult;
using UnitTests.Fakes;
using UnitTests.Fakes.PlayerFakes;

namespace UnitTests.FeatureHandlers.Player;

public class ManagePlayerStatusHandlerTests
{
    [Fact]
    public async Task Handle_ValidBlacklistCommand_UpdatesPlayerAndReturnsSuccess()
    {
        var repository = new FakePlayerRepository();
        var unitOfWork = new FakeUnitOfWork();

        var existingPlayerResult = PlayerAggregate.Register(
            new ViaId(123456),
            "Jane",
            "Doe",
            "abcd@via.dk",
            "password123",
            "https://example.com/existing.png");

        await repository.AddAsync(existingPlayerResult.value);

        var commandResult = ManagePlayerStatusCommand.Create(123456, "Blacklist");
        ManagePlayerStatusCommand command = commandResult.value;

        ICommandHandler<ManagePlayerStatusCommand> handler = new ManagePlayerStatusHandler(repository, unitOfWork);

        Result<None> result = await handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);

        PlayerAggregate? updatedPlayer = await repository.GetAsync(new ViaId(123456));
        Assert.NotNull(updatedPlayer);
        Assert.True(updatedPlayer.Blacklisted);
    }

    [Fact]
    public async Task Handle_PlayerNotFound_ReturnsFailure()
    {
        var repository = new FakePlayerRepository();
        var unitOfWork = new FakeUnitOfWork();

        var commandResult = ManagePlayerStatusCommand.Create(123456, "Blacklist");
        ManagePlayerStatusCommand command = commandResult.value;

        ICommandHandler<ManagePlayerStatusCommand> handler = new ManagePlayerStatusHandler(repository, unitOfWork);

        Result<None> result = await handler.HandleAsync(command);

        Assert.True(result.IsFailure);
        Assert.Equal(0, unitOfWork.SaveChangesCallCount);
        Assert.Contains(result.errorMessages, error => error.ErrorCode == "Player.NotFound");
    }
}