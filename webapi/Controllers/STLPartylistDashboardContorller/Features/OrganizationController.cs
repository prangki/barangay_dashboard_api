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
    public class OrganizationController : ControllerBase
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
        public async Task<IActionResult> Task0a(string organization)
        {
            var result = await _repo.Load_Organization(organization);
            if (result.result == Results.Success)
                return Ok(result.org);
            return NotFound();
        }
        [HttpPost]
        [Route("organization/new")]
        public async Task<IActionResult> Task0b([FromBody] Organization req)
        {
            var result = await _repo.OrganizationAsync(req);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message, OrganizationID=result.orgid });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("organization/edit")]
        public async Task<IActionResult> Task0c([FromBody] Organization req)
        {
            var result = await _repo.OrganizationAsync(req, true);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message, OrganizationID = result.orgid });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }
    }
}
