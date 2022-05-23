using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard.HeadOfficeRepository;
using webapi.App.STLDashboardModel;

namespace webapi.Controllers.STLPartylistDashboardContorller.PartyListController.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    public class HeadOfficeController : ControllerBase
    {
        private readonly IHeadOfficeRepository _repo;
        private readonly IConfiguration _config;
        public HeadOfficeController(IConfiguration config, IHeadOfficeRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("headoffice")]
        public async Task<IActionResult> Task0a(AgentHeadOffice request)
        {
            var result = await _repo.SaveHeadOffice(request);
            return Ok(new { result = result.result, message = result.message });
        }
    }
}
