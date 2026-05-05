using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.PlayerCommands;
using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Features.PlayerFeatures;

public class ManagePlayerStatusHandler : ICommandHandler<ManagePlayerStatusCommand>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ManagePlayerStatusHandler(IPlayerRepository playerRepository, IUnitOfWork unitOfWork)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<None>> HandleAsync(ManagePlayerStatusCommand command)
    {
        Player? player = await _playerRepository.GetAsync(command.PlayerId);
        if (player is null)
        {
            return Result<None>.Failure(new List<OperationError>
            {
                OperationError.Create("Player.NotFound", "No player found with this id.")
            });
        }

        switch (command.Action)
        {
            case PlayerStatusAction.Blacklist:
                player.BlackListPlayer();
                break;
            case PlayerStatusAction.Unblacklist:
                player.UnblacklistPlayer();
                break;
            case PlayerStatusAction.Quarantine:
                if (!command.Amount.HasValue)
                {
                    return Result<None>.Failure(new List<OperationError>
                    {
                        OperationError.Create("PlayerStatus.Amount.Required", "Quarantine requires a valid number of days.")
                    });
                }

                player.QuarantinePlayer(command.Amount.Value);
                break;
            case PlayerStatusAction.RenewVip:
                if (!command.Amount.HasValue)
                {
                    return Result<None>.Failure(new List<OperationError>
                    {
                        OperationError.Create("PlayerStatus.Amount.Required", "VIP renewal requires a valid number of months.")
                    });
                }

            //     player.RenewVip(command.Amount.Value);
            //     break;
            // case PlayerStatusAction.RevokeVip:
            //     player.RevokeVip();
                break;
            default:
                return Result<None>.Failure(new List<OperationError>
                {
                    OperationError.Create("PlayerStatus.Action.Unsupported", "The provided action is not supported.")
                });
        }

        await _unitOfWork.SaveChangesAsync();
        return Result<None>.Success(None.Value);
    }
}

