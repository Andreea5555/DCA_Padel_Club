using DCA_Padel_Club.Core.Application.AppEntry;
using DCA_Padel_Club.Core.Application.Commands.PlayerCommands;
using DCA_Padel_Club.Core.Tools.OperationResult;
using DCA_Padel_Club.Presentation.WebAPI.Endpoints.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DCA_Padel_Club.Presentation.WebAPI.Endpoints.Player;

public class ManagePlayerStatusEndpoint(ICommandDispatcher dispatcher)
    : ApiEndpoint
        .WithRequest<ManagePlayerStatusRequest>
        .AndResults<NoContent, BadRequest<IReadOnlyList<OperationError>>>
{
    [HttpPost("players/{playerId}/manage-status")]
    public override async Task<Results<NoContent, BadRequest<IReadOnlyList<OperationError>>>>
        HandleAsync([FromRoute] ManagePlayerStatusRequest request)
    {
        Result<ManagePlayerStatusCommand> commandResult =
            ManagePlayerStatusCommand.Create(
                request.PlayerId,
                request.RequestBody.Action,
                request.RequestBody.Amount);

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

public record ManagePlayerStatusRequest(
    [FromRoute(Name = "playerId")] int PlayerId,
    [FromBody] ManagePlayerStatusRequest.Body RequestBody)
{
    public record Body(string Action, int? Amount = null);
}

