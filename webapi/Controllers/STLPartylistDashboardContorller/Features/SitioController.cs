using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.STLDashboardModel;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class SitioController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ISitioRepository _repo;
        private readonly IAccountRepository _loginrepo;
        public SitioController(IConfiguration config, ISitioRepository repo, IAccountRepository loginrepo)
        {
            _repo = repo;
            _config = config;
            _loginrepo = loginrepo;
        }
        [HttpPost]
        [Route("sitio")]
        public async Task<IActionResult> Sitio(string code, int row = 0, string search="")
        {
            var result = await _repo.LoadSitio(code, row, search);
            if (result.result == Results.Success)
                return Ok(result.sit);
            return NotFound();
        }
        [HttpPost]
        [Route("barangay/sitio")]
        public async Task<IActionResult> SitioPerBarangay(string strcode)
        {
            var result = await _repo.LoadSitioPerBarangay(strcode);
            if (result.result == Results.Success)
                return Ok(result.sit);
            return NotFound();
        }
        [HttpPost]
        [Route("sitio/new")]
        public async Task<IActionResult> Task0a([FromBody] Sitio sit)
        {
            var result = await _repo.SitioAsync(sit);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message, sitcode=result.sitcode });
            return NotFound();
        }
        [HttpPost]
        [Route("sitio/edit")]
        public async Task<IActionResult> Task0B([FromBody] Sitio sit)
        {
            var result = await _repo.SitioAsync(sit,true);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message, sitcode = result.sitcode });
            return NotFound();
        }
        [HttpPost]
        [Route("brgy")]
        public async Task<IActionResult> Barangay([FromBody] BarangayList request)
        {
            var result = await _repo.LoadBrgy(request);
            if (result.result == Results.Success)
                return Ok(result.brgy);
            return NotFound();
        }
        [HttpPost]
        [Route("municiapl/brgy")]
        public async Task<IActionResult> BarangayA(string strcode)
        {
            var result = await _repo.LoadBrgyPerTown(strcode);
            if (result.result == Results.Success)
                return Ok(result.brgy);
            return NotFound();
        }

        [HttpPost]
        [Route("brgy/new")]
        public async Task<IActionResult> Task0C([FromBody] Barangay brgy)
        {
            var result = await _repo.BrgyAsync(brgy);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message, brgycode=result.brgycode });
            return NotFound();
        }
        [HttpPost]
        [Route("brgy/edit")]
        public async Task<IActionResult> Task0D([FromBody] Barangay brgy)
        {
            var result = await _repo.BrgyAsync(brgy, true);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message, brgycode = result.brgycode });
            return NotFound();
        }
    }
}
