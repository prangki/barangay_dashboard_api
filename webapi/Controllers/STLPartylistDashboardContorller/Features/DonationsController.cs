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
    public class DonationsController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IDonationsRepository _repo;
        public DonationsController(IConfiguration config, IDonationsRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("donation")]
        public async Task<IActionResult> Group()
        {
            var result = await _repo.LoadDonations();
            if (result.result == Results.Success)
                return Ok(result.donation);
            return NotFound();
        }
        [HttpPost]
        [Route("donation/new")]
        public async Task<IActionResult> Task0a([FromBody] Donation request)
        {
            var result = await _repo.DonationAsync(request);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("donation/edit")]
        public async Task<IActionResult> Task0b([FromBody] Donation request)
        {
            var result = await _repo.UpdateDonationAsync(request);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("donation/resend/otp")]
        public async Task<IActionResult> Task0c([FromBody] Donation request)
        {
            var result = await _repo.ResendOTP(request);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("donation/claim")]
        public async Task<IActionResult> Task0d([FromBody] Donation request)
        {
            var result = await _repo.ClaimDonation(request);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }
    }
}
