namespace UnitTests.FeatureCommands.Player;

public class ManagePlayerStatusCommandTests
{
    [Fact]
    public void Create_ValidBlacklistInput_ReturnsSuccess()
    {
        var result = DCA_Padel_Club.Core.Application.Commands.PlayerCommands.ManagePlayerStatusCommand.Create(
            123456,
            "Blacklist");

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.value);
        Assert.Equal(123456, result.value.PlayerId.Value);
        Assert.Equal(DCA_Padel_Club.Core.Application.Commands.PlayerCommands.PlayerStatusAction.Blacklist, result.value.Action);
        Assert.Null(result.value.Amount);
    }

    [Fact]
    public void Create_InvalidAction_ReturnsFailure()
    {
        var result = DCA_Padel_Club.Core.Application.Commands.PlayerCommands.ManagePlayerStatusCommand.Create(
            123456,
            "NotARealAction");

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, error => error.ErrorCode == "PlayerStatus.Action.Invalid");
    }
}