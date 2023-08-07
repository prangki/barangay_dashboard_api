using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class PoliticalPositionController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IPoliticalPositionRepository _repo;
        public PoliticalPositionController(IConfiguration config, IPoliticalPositionRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("politicalposition")]
        public async Task<IActionResult> PoliticalPosition()
        {
            var result = await _repo.LoadPoliticalPosition();
            if (result.result == Results.Success)
                return Ok(result.psn );
            return NotFound();
        }

        [HttpPost]
        [Route("barangayposition")]
        public async Task<IActionResult> BarangayPosition()
        {
            var result = await _repo.LoadBarangayPosition();
            if (result.result == Results.Success)
                return Ok(result.psn);
            return NotFound();
        }

        [HttpPost]
        [Route("politicalposition/new")]
        public async Task<IActionResult> Task0a(string psn)
        {
            var result = await _repo.PoliticalPositionAsyn(psn);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("politicalposition/edit")]
        public async Task<IActionResult> Task0b(string psn, string psncd)
        {
            var result = await _repo.PoliticalPositionAsyn(psn,psncd, true);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("barangayposition/search")]
        public async Task<IActionResult> BarangayPosition(string userId)
        {
            var result = await _repo.SearchBarangayPosition(userId);
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }
    }
}
