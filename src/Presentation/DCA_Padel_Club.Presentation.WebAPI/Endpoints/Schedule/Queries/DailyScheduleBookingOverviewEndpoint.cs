using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DCA_Padel_Club.Presentation.WebAPI.Endpoints.Schedule.Queries;

[ApiController]
[Route("api/schedules")]
public class DailyScheduleBookingOverviewEndpoint(IQueryDispatcher dispatcher) : ControllerBase
{
    [HttpGet("{scheduleId}/bookings-overview")]
    public async Task<DailyScheduleBookingOverview.Answer> HandleAsync(
        [FromRoute] string scheduleId)
    {
        DailyScheduleBookingOverview.Query query =
            new(scheduleId);

        return await dispatcher.DispatchAsync(query);
    }
}