using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.RequestModel.Common;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class EstablishmentController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IEstablishmentRepository _repo;
        public EstablishmentController(IConfiguration config, IEstablishmentRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("establishment/list")]
        public async Task<IActionResult> Task0a(Establishment_Request req)
        {
            var result = await _repo.Load_Establishment(req);
            if (result.result == Results.Success)
                return Ok(result.est);
            return NotFound();
        }
        [HttpPost]
        [Route("establishment/new")]
        public async Task<IActionResult> Task0b([FromBody] Establishment req)
        {
            var result = await _repo.EstablishmentAsync(req);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", message = result.message, Establishment_ID = result.estid });
            else if (result.result == Results.Failed)
                return Ok(new { Status = "error", message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("establishment/edit")]
        public async Task<IActionResult> Task0c([FromBody] Establishment req)
        {
            var result = await _repo.EstablishmentAsync(req, true);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", message = result.message, Establishment_ID = result.estid });
            else if (result.result == Results.Failed)
                return Ok(new { Status = "error", message = result.message });
            return NotFound();
        }
    }
}
