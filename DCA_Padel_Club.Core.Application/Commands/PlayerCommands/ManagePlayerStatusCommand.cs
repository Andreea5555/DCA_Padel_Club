using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Commands.PlayerCommands;

public enum PlayerStatusAction
{
    Blacklist,
    Unblacklist,
    Quarantine,
    RenewVip,
    RevokeVip
}

public class ManagePlayerStatusCommand
{
    public ViaId PlayerId { get; }
    public PlayerStatusAction Action { get; }
    public int? Amount { get; }

    private ManagePlayerStatusCommand(ViaId playerId, PlayerStatusAction action, int? amount)
    {
        PlayerId = playerId;
        Action = action;
        Amount = amount;
    }

    public static Result<ManagePlayerStatusCommand> Create(int playerId, string action, int? amount = null)
    {
        var errors = new List<OperationError>();

        if (!Enum.TryParse(action, ignoreCase: true, out PlayerStatusAction parsedAction))
        {
            errors.Add(OperationError.Create(
                "PlayerStatus.Action.Invalid",
                "The provided player status action is invalid."));
        }

        if (errors.Count == 0 && RequiresAmount(parsedAction) && (!amount.HasValue || amount.Value <= 0))
        {
            errors.Add(OperationError.Create(
                "PlayerStatus.Amount.Invalid",
                "The provided amount must be greater than zero for this action."));
        }

        if (errors.Count > 0)
        {
            return Result<ManagePlayerStatusCommand>.Failure(errors);
        }

        return Result<ManagePlayerStatusCommand>.Success(
            new ManagePlayerStatusCommand(new ViaId(playerId), parsedAction, amount));
    }

    private static bool RequiresAmount(PlayerStatusAction action)
    {
        return action is PlayerStatusAction.Quarantine or PlayerStatusAction.RenewVip;
    }
}
