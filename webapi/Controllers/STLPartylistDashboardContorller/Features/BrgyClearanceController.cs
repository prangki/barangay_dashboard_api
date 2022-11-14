using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comm.Commons.Extensions;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Features.UserFeature;
using webapi.App.RequestModel.AppRecruiter;
using Newtonsoft.Json;
using webapi.App.STLDashboardModel;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class BrgyClearanceController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IBrgyClearanceRepository _repo;
        public BrgyClearanceController(IConfiguration config, IBrgyClearanceRepository repo)
        {
            _config = config;
            _repo = repo;
        }


        [HttpPost]
        [Route("brgyclearance/new")]
        public async Task<IActionResult> Task0a([FromBody] BrgyClearance request)
        {
            var result = await _repo.BrgyClearanceAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, ClearanceNo = result.brgyclrid, ControlNo=result.cntrlno });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("brgyclearance/release")]
        public async Task<IActionResult> Task0b([FromBody] BrgyClearance request)
        {
            var result = await _repo.ReleaseAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Release = result.release });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("brgyclearance/cancel")]
        public async Task<IActionResult> Task0c([FromBody] BrgyClearance request)
        {
            var result = await _repo.CancellAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Cancel = result.cancel });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("brgyclearance")]
        public async Task<IActionResult> Task0d([FromBody] BrgyClearance request)
        {
            var result = await _repo.Load_BrgyClearance(request);
            if (result.result == Results.Success)
                return Ok(result.bryclrid);
            return NotFound();
        }
        [HttpPost]
        [Route("brgyclearance/received")]
        public async Task<IActionResult> Task0e([FromBody] BrgyClearance request)
        {
            var result = await _repo.ReceivedBrgyClearanceRequestAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("brgyclearance/process")]
        public async Task<IActionResult> Task0f([FromBody] BrgyClearance request)
        {
            var result = await _repo.ProcessRecivedBrgyClearanceRequestAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
    }
}
