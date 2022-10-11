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
    public class ReligionController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IReligionRepository _repo;
        public ReligionController(IConfiguration config, IReligionRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        
        [HttpPost]
        [Route("religion/add")]
        public async Task<IActionResult> AddReligion([FromBody] Religion religion)
        {
            //var result = await _repo.AddPosition(position.JsonString);
            var result = await _repo.AddReligion(religion);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("religion/update")]
        public async Task<IActionResult> UpdateReligion([FromBody] Religion religion)
        {
            //var result = await _repo.AddPosition(position.JsonString);
            var result = await _repo.UpdateReligion(religion);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("religion/delete")]
        public async Task<IActionResult> DeleteReligion([FromBody] Religion religion)
        {
            //var result = await _repo.AddPosition(position.JsonString);
            var result = await _repo.DeleteReligion(religion.Description);

            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("religion/load")]
        public async Task<IActionResult> LoadReligion(string plid)
        {
            var result = await _repo.LoadReligion("0002");
            if (result.result == Results.Success)
                return Ok(result.position);
            return NotFound();
        }
    }
}
