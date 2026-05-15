using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DCA_Padel_Club.Presentation.WebAPI.Endpoints.Schedule.Queries;

[ApiController]
[Route("api/schedules")]
public class ManagerScheduleOverviewEndpoint(IQueryDispatcher dispatcher) : ControllerBase
{
    [HttpGet("manager-overview")]
    public async Task<ManagerScheduleOverview.Answer> HandleAsync(
        [FromQuery] int year,
        [FromQuery] int month)
    {
        ManagerScheduleOverview.Query query =
            new(year, month);

        return await dispatcher.DispatchAsync(query);
    }
}