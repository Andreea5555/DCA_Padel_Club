using DCA_Padel_Club.Core.Application.AppEntry;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Tools.OperationResult;
using DCA_Padel_Club.Presentation.WebAPI.Endpoints.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DCA_Padel_Club.Presentation.WebAPI.Endpoints.Schedule;

public class UpdateScheduleTimeEndpoint(ICommandDispatcher dispatcher)
    : ApiEndpoint
        .WithRequest<UpdateScheduleTimeRequest>
        .AndResults<NoContent, BadRequest<IReadOnlyList<OperationError>>>
{
    [HttpPost("schedules/{id}/update-time")]
    public override async Task<
            Results<NoContent, BadRequest<IReadOnlyList<OperationError>>>>
        HandleAsync([FromRoute] UpdateScheduleTimeRequest request)
    {
        Result<UpdateScheduleTimeCommand> commandResult =
            UpdateScheduleTimeCommand.Create(request.Id, request.RequestBody.startTime, request.RequestBody.endTime);

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
public record UpdateScheduleTimeRequest(
    [FromRoute(Name = "id")] string Id,
    [FromBody] UpdateScheduleTimeRequest.Body RequestBody)
{
    public record Body(string startTime, string endTime);
}