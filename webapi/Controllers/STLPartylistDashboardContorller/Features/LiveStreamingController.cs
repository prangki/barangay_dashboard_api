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
    public class LiveStreamingController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILiveStreamingRepository _repo;
        public LiveStreamingController(IConfiguration config, ILiveStreamingRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("streaming")]
        public async Task<IActionResult> streamingurl()
        {
            var result = await _repo.LiveStreamingURL();
            if (result.result == Results.Success)
                return Ok(new { result = result.result, content = result.url });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, content = result.url });
            return NotFound();
        }
        [HttpPost]
        [Route("streaming/update")]
        public async Task<IActionResult> Task0a(string url, string description)
        {
            var result = await _repo.LiveStreamingAsync(url,description);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("streaming/sa/update")]
        public async Task<IActionResult> Task0b([FromBody] LiveStreaming request)
        {
            var result = await _repo.LiveStreamingAsyncSA(request);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }
    }
}
