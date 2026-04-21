using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.PlayerCommands;
using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Features.PlayerFeatures;

public class CreatePlayerHandler : ICommandHandler<CreatePlayerCommand>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePlayerHandler(IPlayerRepository playerRepository, IUnitOfWork unitOfWork)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<None>> HandleAsync(CreatePlayerCommand command)
    {
        Player? existingPlayer = await _playerRepository.GetAsync(command.PlayerId);
        if (existingPlayer is not null)
        {
            return Result<None>.Failure(new List<OperationError>
            {
                OperationError.Create("Player.AlreadyExists", "A player with this id already exists.")
            });
        }

        Result<Player> playerResult = Player.Register(
            command.PlayerId,
            command.FirstName.Value,
            command.LastName.Value,
            command.Email.Value,
            command.Password.Value,
            command.ProfilePicture.Value);

        if (playerResult.IsFailure)
        {
            return Result<None>.Failure(playerResult.errorMessages.ToList());
        }

        await _playerRepository.AddAsync(playerResult.value);
        await _unitOfWork.SaveChangesAsync();

        return Result<None>.Success(None.Value);
    }
}