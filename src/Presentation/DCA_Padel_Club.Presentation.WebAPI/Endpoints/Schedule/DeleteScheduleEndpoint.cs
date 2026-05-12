using DCA_Padel_Club.Core.Application.AppEntry;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Tools.OperationResult;
using DCA_Padel_Club.Presentation.WebAPI.Endpoints.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DCA_Padel_Club.Presentation.WebAPI.Endpoints.Schedule;

public class DeleteScheduleEndpoint(ICommandDispatcher dispatcher)
    : ApiEndpoint
        .WithRequest<DeleteScheduleRequest>
        .AndResults<NoContent, BadRequest<IReadOnlyList<OperationError>>>
{
    [HttpPost("schedules/{id}/delete")]
    public override async Task<Results<NoContent, BadRequest<IReadOnlyList<OperationError>>>>
        HandleAsync([FromRoute] DeleteScheduleRequest request)
    {
        Result<DeleteScheduleCommand> commandResult =
            DeleteScheduleCommand.Create(request.Id);

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
public record DeleteScheduleRequest(
    [FromRoute(Name = "id")] string Id);