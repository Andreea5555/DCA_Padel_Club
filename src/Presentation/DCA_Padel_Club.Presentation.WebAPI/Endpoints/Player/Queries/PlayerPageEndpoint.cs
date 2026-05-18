using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DCA_Padel_Club.Presentation.WebAPI.Endpoints.Player.Queries;

[ApiController]
[Route("api/players")]
public class PlayerPageEndpoint(IQueryDispatcher dispatcher) : ControllerBase
{
    [HttpGet("{playerId}")]
    public async Task<PlayerPage.Answer> HandleAsync([FromRoute] int playerId)
    {
        PlayerPage.Query query = new(playerId);

        return await dispatcher.DispatchAsync(query);
    }
}

