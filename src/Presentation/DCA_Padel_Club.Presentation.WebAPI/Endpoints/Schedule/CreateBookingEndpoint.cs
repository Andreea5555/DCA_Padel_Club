using DCA_Padel_Club.Core.Application.AppEntry;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Tools.OperationResult;
using DCA_Padel_Club.Presentation.WebAPI.Endpoints.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DCA_Padel_Club.Presentation.WebAPI.Endpoints.Schedule;

public class CreateBookingEndpoint(ICommandDispatcher dispatcher)
    : ApiEndpoint
        .WithRequest<CreateBookingRequest>
        .AndResults<NoContent, BadRequest<IReadOnlyList<OperationError>>>
{
    [HttpPost("schedules/{id}/create-booking")]
    public override async Task<Results<NoContent, BadRequest<IReadOnlyList<OperationError>>>>
        HandleAsync([FromRoute] CreateBookingRequest request)
    {
        Result<CreateBookingCommand> commandResult =
            CreateBookingCommand.Create(
                request.Id,
                request.RequestBody.BookerId,
                request.RequestBody.CourtId,
                request.RequestBody.Date,
                request.RequestBody.StartTime,
                request.RequestBody.EndTime);

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

public record CreateBookingRequest(
    [FromRoute(Name = "id")] string Id,
    [FromBody] CreateBookingRequest.Body RequestBody)
{
    public record Body(
        int BookerId,
        string CourtId,
        string Date,
        string StartTime,
        string EndTime);
}
