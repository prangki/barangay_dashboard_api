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
    public class AnnouncementController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAnnouncementRepository _repo;
        public AnnouncementController(IConfiguration config, IAnnouncementRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("announcement")]
        public async Task<IActionResult> Announcement(string title, string description)
        {
            var result = await _repo.AnnouncementAsync(title,description);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("event/new")]
        public async Task<IActionResult> Task0a([FromBody] GROUPEVENT request)
        {
            var result = await _repo.EventsAsync(request);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message, content = result.request });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("event/edit")]
        public async Task<IActionResult> Task0b([FromBody] GROUPEVENT request)
        {
            var result = await _repo.EventsAsync(request,true);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message, content=result.request });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message, content = result.request });
            return NotFound();
        }
        [HttpPost]
        [Route("event")]
        public async Task<IActionResult> Group()
        {
            var result = await _repo.LoadEvents();
            if (result.result == Results.Success)
                return Ok(result.stlevents);
            return NotFound();
        }
    }
}
