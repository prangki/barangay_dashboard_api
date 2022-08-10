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
    public class OrganizationController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IOrganizationRepository _repo;
        public OrganizationController(IConfiguration config, IOrganizationRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("organization")]
        public async Task<IActionResult> Task0a()
        {
            var result = await _repo.Load_Organization();
            if (result.result == Results.Success)
                return Ok(result.org);
            return NotFound();
        }
    }
}
