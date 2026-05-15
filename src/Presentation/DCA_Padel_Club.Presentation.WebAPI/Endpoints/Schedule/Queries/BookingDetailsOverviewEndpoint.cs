using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DCA_Padel_Club.Presentation.WebAPI.Endpoints.Schedule.Queries;

[ApiController]
[Route("api/bookings")]
public class BookingDetailsOverviewEndpoint(IQueryDispatcher dispatcher) : ControllerBase
{
    [HttpGet("{bookingId}")]
    public async Task<BookingDetailsOverview.Answer> HandleAsync(
        [FromRoute] string bookingId)
    {
        BookingDetailsOverview.Query query =
            new(bookingId);

        return await dispatcher.DispatchAsync(query);
    }
}