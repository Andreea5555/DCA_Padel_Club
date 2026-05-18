using DCA_Padel_Club.Core.Application.AppEntry;
using DCA_Padel_Club.Core.Application.Commands.PlayerCommands;
using DCA_Padel_Club.Core.Tools.OperationResult;
using DCA_Padel_Club.Presentation.WebAPI.Endpoints.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DCA_Padel_Club.Presentation.WebAPI.Endpoints.Player;

public class CreatePlayerEndpoint(ICommandDispatcher dispatcher)
    : ApiEndpoint
        .WithRequest<CreatePlayerRequest>
        .AndResults<NoContent, BadRequest<IReadOnlyList<OperationError>>>
{
    [HttpPost("players/create")]
    public override async Task<Results<NoContent, BadRequest<IReadOnlyList<OperationError>>>>
        HandleAsync([FromBody] CreatePlayerRequest request)
    {
        Result<CreatePlayerCommand> commandResult =
            CreatePlayerCommand.Create(
                request.PlayerId,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                request.ProfilePictureUri);

        if (commandResult.IsFailure)
        {
            return TypedResults.BadRequest(commandResult.errorMessages);
        }

        Result<None> result =
            await dispatcher.DispatchAsync(commandResult.value);

        if (result.IsFailure)
        {
            return TypedResults.BadRequest(result.errorMessages);
        }

        return TypedResults.NoContent();
    }
}

public record CreatePlayerRequest(
    int PlayerId,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ProfilePictureUri);

