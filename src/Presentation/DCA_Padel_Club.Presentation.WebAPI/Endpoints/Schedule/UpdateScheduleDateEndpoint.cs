using DCA_Padel_Club.Core.Application.AppEntry;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Tools.OperationResult;
using DCA_Padel_Club.Presentation.WebAPI.Endpoints.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DCA_Padel_Club.Presentation.WebAPI.Endpoints.Schedule;

public class UpdateScheduleDateEndpoint(ICommandDispatcher dispatcher)
    : ApiEndpoint
        .WithRequest<UpdateScheduleDateRequest>
        .AndResults<NoContent, BadRequest<IReadOnlyList<OperationError>>>
{
    [HttpPost("schedules/{id}/update-date")]
    public override async Task<Results<NoContent, BadRequest<IReadOnlyList<OperationError>>>>
        HandleAsync([FromRoute] UpdateScheduleDateRequest request)
    {
        Result<UpdateScheduleDateCommand> commandResult =
            UpdateScheduleDateCommand.Create(request.Id, request.RequestBody.Date);

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

public record UpdateScheduleDateRequest(
    [FromRoute(Name = "id")] string Id,
    [FromBody] UpdateScheduleDateRequest.Body RequestBody)
{
    public record Body(string Date);
}