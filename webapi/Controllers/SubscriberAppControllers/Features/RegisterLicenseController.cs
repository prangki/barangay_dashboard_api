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
        public async Task<IActionResult> License([FromBody] LicenseKey lic)
        {
            var result = await _repo.LicenseRegister(lic);
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
        [HttpPost]
        [Route("license/generate")]
        public async Task<IActionResult> LicenseGenerated([FromBody] Generate_License_Key lic)
        {
            if (lic.ID == "")
            {
                var result = await _repo.GenerateLicenseKey(lic);
                if (result.result == Results.Success)
                    return Ok(new { result = result.result, message = result.message, Content = lic });
                else if (result.result == Results.Failed)
                    return Ok(new { result = result.result, message = result.message });
                return NotFound();
            }
            else
            {
                var result = await _repo.GenerateLicenseKey(lic, true);
                if (result.result == Results.Success)
                    return Ok(new { result = result.result, message = result.message, Content = lic });
                else if (result.result == Results.Failed)
                    return Ok(new { result = result.result, message = result.message });
                return NotFound();
            }
                
        }
        [HttpPost]
        [Route("license/generatedkey")]
        public async Task<IActionResult> GeneratedLicenseKey([FromBody] LicenseFilterRequest filter)
        {
            var result = await _repo.LoadGeneatedLicense(filter);
            if (result.result == Results.Success)
                return Ok(result.lic);
            return NotFound();
        }
        [HttpPost]
        [Route("license/availability")]
        public async Task<IActionResult> AvailalbeLicenseKey([FromBody] LicenseKeyAvailable filter)
        {
            var result = await _repo.LicenseKeyAvilability(filter);
            if (result.result == Results.Success)
                return Ok(result.lic);
            return NotFound();
        }
    }
}
