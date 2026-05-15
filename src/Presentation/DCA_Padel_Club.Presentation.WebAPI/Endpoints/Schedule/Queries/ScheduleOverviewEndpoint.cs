using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DCA_Padel_Club.Presentation.WebAPI.Endpoints.Schedule.Queries;


[ApiController]
[Route("api/schedules")]
public class ScheduleOverviewEndpoint(IQueryDispatcher dispatcher) : ControllerBase
{
    [HttpGet("overview")]
    public async Task<ScheduleOverview.Answer> HandleAsync(
        [FromQuery] int year,
        [FromQuery] int month)
    {
        ScheduleOverview.Query query =
            new(year, month);

        return await dispatcher.DispatchAsync(query);
    }
}