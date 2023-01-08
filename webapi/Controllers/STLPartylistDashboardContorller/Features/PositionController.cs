using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.STLDashboardModel;





namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class PositionController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IPositionRepository _repo;
        public PositionController(IConfiguration config, IPositionRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        
        [HttpPost]
        [Route("position/add")]
        public async Task<IActionResult> AddPosition([FromBody] Position position)
        {
            //var result = await _repo.AddPosition(position.JsonString);
            var result = await _repo.AddPosition(position);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("position/update")]
        public async Task<IActionResult> UpdatePosition([FromBody] Position position)
        {
            //var result = await _repo.AddPosition(position.JsonString);
            var result = await _repo.UpdatePosition(position);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("position/delete")]
        public async Task<IActionResult> DeletePosition([FromBody] Position position)
        {
            //var result = await _repo.AddPosition(position.JsonString);
            var result = await _repo.DeletePosition(position.PositionId);

            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("position/load")]
        public async Task<IActionResult> LoadPosition()
        {
            var result = await _repo.LoadPosition();
            if (result.result == Results.Success)
                return Ok(result.household);
            return NotFound();
        }

    }
}
