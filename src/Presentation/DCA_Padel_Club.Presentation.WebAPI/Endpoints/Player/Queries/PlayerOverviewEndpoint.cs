using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DCA_Padel_Club.Presentation.WebAPI.Endpoints.Player.Queries;

[ApiController]
[Route("api/players")]
public class PlayerOverviewEndpoint(IQueryDispatcher dispatcher) : ControllerBase
{
    [HttpGet("overview")]
    public async Task<PlayerOverview.Answer> HandleAsync()
    {
        PlayerOverview.Query query = new();

        return await dispatcher.DispatchAsync(query);
    }
}

