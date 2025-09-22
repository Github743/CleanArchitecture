using Microsoft.AspNetCore.Mvc;
using WayPoint.Model;
using WayPoint_Infrastructure.Interfaces;

namespace WayPoint_BFA.Controllers
{
    [ApiController]
    [Route("api")]
    public class ClientController(IClientRepository service) : ControllerBase
    {
        private readonly IClientRepository _service = service;

        [HttpGet("clients")]
        public async Task<ActionResult<IReadOnlyList<ClientDetail>>> GetClients([FromQuery] string? clientSearch, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(clientSearch)) return BadRequest("Search text is mandaotry");
            var rows = await _service.GetClientsAsync(clientSearch, ct);
            return Ok(rows);
        }

        //[HttpGet("Client/{clientId:int}")]
        //public async Task<ActionResult<ClientDetail>> GetClient(int clientId, CancellationToken ct = default)
        //{
        //    var client = await _service.GetClient(clientId, ct);

        //    if (client == null)
        //        return NotFound();

        //    return Ok(client);
        //}
    }
}
