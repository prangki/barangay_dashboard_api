using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.STLDashboardModel;
using Comm.Commons.Extensions;
using Newtonsoft.Json;
using System;
using webapi.App.Features.UserFeature;
using System.Collections.Generic;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class BlotterController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IBlottersRepository _repo;
        private readonly IAccountRepository _loginrepo;

        public BlotterController(IConfiguration config, IBlottersRepository repo, IAccountRepository loginrepo)
        {
            _repo = repo;
            _config = config;
            _loginrepo = loginrepo;
        }

        [HttpPost]
        [Route("blotter/save")]
        public async Task<IActionResult> SaveBlotter([FromBody] Blotter info)
        {
            var valResult = await validityReport(info);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var result = await _repo.SaveBlotter(info);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("blotter/update")]
        public async Task<IActionResult> UpdateBlotter([FromBody] Blotter info)
        {
            var valResult = await validityReport(info);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var result = await _repo.UpdateBlotter(info);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("blotter/load")]
        public async Task<IActionResult> LoadBlotter(string plid, string pgrpid)
        {
            var result = await _repo.LoadBlotter(plid, pgrpid);
            if (result.result == Results.Success)
                return Ok(result.blotter);
            return NotFound();
        }

        [HttpPost]
        [Route("blotter/summon/load")]
        public async Task<IActionResult> LoadSummon(string plid, string pgrpid, bool isSummon = true)
        {
            var result = await _repo.LoadSummon(plid, pgrpid);
            if (result.result == Results.Success)
                return Ok(result.summon);
            return NotFound();
        }

        [HttpPost]
        [Route("blotter/summon/save")]
        public async Task<IActionResult> SaveSummon([FromBody] Blotter info, bool isSummon = true)
        {
            var valResult = await validityReport(info);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var result = await _repo.SaveSummon(info);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("blotter/summon/update")]
        public async Task<IActionResult> UpdateSummon([FromBody] Blotter info, bool isSummon = true)
        {
            var valResult = await validityReport(info);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var result = await _repo.UpdateSummon(info);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("blotter/summon/resolve")]
        public async Task<IActionResult> ResolveSummon([FromBody] Blotter info, bool isSummon = true)
        {
            var result = await _repo.ResolveSummon(info);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("blotter/summon/unsummon")]
        public async Task<IActionResult> RemoveSummon([FromBody] Blotter info, bool isSummon = true)
        {
            var result = await _repo.RemoveSummon(info);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("blotter/getbrgycpt")]
        public async Task<IActionResult> GetBrgyCpt(string plid, string pgrpid)
        {
            var result = await _repo.GetBrgyCpt(plid, pgrpid);
            if (result.result == Results.Success)
                return Ok(result.brgycpt);
            return NotFound();
        }

        [HttpPost]
        [Route("blotter/maxcaseno")]
        public async Task<IActionResult> GetMaxCaseNo(string plid, string pgrpid)
        {
            var result = await _repo.UpdatedCaseNo(plid, pgrpid);
            if (result.result == Results.Success)
                return Ok(result.caseNo);
            return NotFound();
        }

        [HttpPost]
        [Route("blotter/reprint")]
        public async Task<IActionResult> Reprint(string plid, string pgrpid, string brgycsno, string colname)
        {
            var result = await _repo.Reprint(plid, pgrpid, brgycsno, colname);
            if (result.result == Results.Success)
                return Ok(result.docpath);
            return NotFound();
        }

        [HttpPost]
        [Route("blotter/summon/signatures")]
        public async Task<IActionResult> GetSignature(string plid, string pgrpid)
        {
            var result = await _repo.GetSignature(plid, pgrpid);
            if (result.result == Results.Success)
                return Ok(result.signatures);
            return NotFound();
        }

        [HttpPost]
        [Route("blotter/summon/header")]
        public async Task<IActionResult> GetHeader(string plid, string pgrpid)
        {
            var result = await _repo.GetHeader(plid, pgrpid);
            if (result.result == Results.Success)
                return Ok(result.header);
            return NotFound();
        }

        [HttpPost]
        [Route("blotter/summon/getallresidents")]
        public async Task<IActionResult> GetAllResidents()
        {
            var result = await _repo.GetAllResidents();
            if (result.result == Results.Success)
                return Ok(result.residents);
            return NotFound();
        }

        private async Task<(Results result, string message)> validityReport(Blotter request)
        {
            if (request == null)
                return (Results.Null, null);
            if (request.ReportPath.IsEmpty())
                return (Results.Success, null);
            byte[] bytes = Convert.FromBase64String(request.ReportPath.Str());
            if (bytes.Length == 0)
                return (Results.Failed, "Make sure selected document path is invalid.");
            var res = await ReportService.SendAsync(bytes, request.Docname);
            bytes.Clear();
            if (res == null)
                return (Results.Failed, "Please contact to admin.");
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
            if (json["status"].Str() != "error")
            {
                request.ReportPath = json["url"].Str();

                return (Results.Success, null);
            }
            return (Results.Null, "Make sure selected image is invalid");
        }
    }
}
