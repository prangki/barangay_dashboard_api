using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.Aggregates.Common;

namespace webapi.Controllers.SubscriberAppControllers.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    //[ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class RegisterLicenseController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILicenseRegisterRepository _repo;
        public RegisterLicenseController(IConfiguration config, ILicenseRegisterRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("license")]
        public async Task<IActionResult> License()
        {
            var result = await _repo.LicenseRegister();
            if (result.result == Results.Success)
                return Ok(result.licinfo);
            return NotFound();
        }
        [HttpPost]
        [Route("license/registration")]
        public async Task<IActionResult> LicenseRegister([FromBody] LicenseKey lic)
        {
            var result = await _repo.LicenseKeyRegister(lic);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }
    }
}
