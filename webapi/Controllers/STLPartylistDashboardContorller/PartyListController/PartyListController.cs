using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard.PartyListRepository;

namespace webapi.Controllers.STLPartylistDashboardContorller.PartyListController
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    public class PartyListController:ControllerBase
    {
        private readonly IPartyListRepository _repo;
        private readonly IConfiguration _config;
        public PartyListController(IConfiguration config, IPartyListRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("dashboardstl")]
        public async Task<IActionResult> Task0a()
        {
            var result = await _repo.LoadPartyList();
            if (result.result == Results.Success)
            {
                return Ok(new { Result = result.result, Message = result.message, Content = result.stl });
            }
            return NotFound();
        }
    }
}
