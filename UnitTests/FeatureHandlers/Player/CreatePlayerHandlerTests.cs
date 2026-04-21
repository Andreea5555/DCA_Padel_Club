using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.PlayerCommands;
using DCA_Padel_Club.Core.Application.Features.PlayerFeatures;
using PlayerAggregate = DCA_Padel_Club.Core.Domain.Aggregates.Players.Player;
using ViaId = DCA_Padel_Club.Core.Domain.Aggregates.Players.ViaId;
using DCA_Padel_Club.Core.Tools.OperationResult;
using UnitTests.Fakes;
using UnitTests.Fakes.PlayerFakes;

namespace UnitTests.FeatureHandlers.Player;

public class CreatePlayerHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_AddsPlayerAndReturnsSuccess()
    {
        var repository = new FakePlayerRepository();
        var unitOfWork = new FakeUnitOfWork();

        var commandResult = CreatePlayerCommand.Create(
            123456,
            "John",
            "Doe",
            "abc@via.dk",
            "secret123",
            "https://example.com/profile.png");

        CreatePlayerCommand command = commandResult.value;
        ICommandHandler<CreatePlayerCommand> handler = new CreatePlayerHandler(repository, unitOfWork);

        Result<None> result = await handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);

        PlayerAggregate? storedPlayer = await repository.GetAsync(command.PlayerId);
        Assert.NotNull(storedPlayer);
        Assert.Equal("John", storedPlayer!.FirstName.Value);
        Assert.Equal("Doe", storedPlayer.LastName.Value);
    }

    [Fact]
    public async Task Handle_DuplicatePlayerId_ReturnsFailure()
    {
        var repository = new FakePlayerRepository();
        var unitOfWork = new FakeUnitOfWork();

        var existingPlayerResult = PlayerAggregate.Register(
            new ViaId(123456),
            "Jane",
            "Doe",
            "def@via.dk",
            "password123",
            "https://example.com/existing.png");

        await repository.AddAsync(existingPlayerResult.value);

        var commandResult = CreatePlayerCommand.Create(
            123456,
            "John",
            "Doe",
            "abc@via.dk",
            "secret123",
            "https://example.com/profile.png");

        CreatePlayerCommand command = commandResult.value;
        ICommandHandler<CreatePlayerCommand> handler = new CreatePlayerHandler(repository, unitOfWork);

        Result<None> result = await handler.HandleAsync(command);

        Assert.True(result.IsFailure);
        Assert.Equal(0, unitOfWork.SaveChangesCallCount);
        Assert.Contains(result.errorMessages, error => error.ErrorCode == "Player.AlreadyExists");
    }
}