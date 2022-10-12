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
using webapi.App.STLDashboardModel;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class LegalDocumentController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILegalDocumentsRepository _repo; 
        public LegalDocumentController(IConfiguration config, ILegalDocumentsRepository repo)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost]
        [Route("formtab/new")]
        public async Task<IActionResult> Task0a([FromBody] LegalDocument request)
        {
            var result = await _repo.FormTabAsyn(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, FormTab = result.formtab });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("formtab")]
        public async Task<IActionResult> Task0b(LegalDocument req)
        {
            var result = await _repo.Load_FormTab(req);
            if (result.result == Results.Success)
                return Ok(result.formtab);
            return NotFound();
        }
        [HttpPost]
        [Route("formtab/remove")]
        public async Task<IActionResult> Task0c([FromBody] LegalDocument request)
        {
            var result = await _repo.Delete_FormTab(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("legaldocument/new")]
        public async Task<IActionResult> Task0d([FromBody] LegalDocument_Transaction request)
        {
            var result = await _repo.LegalDocAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, LegalDoc = result.legaldoc });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("legaldocument")]
        public async Task<IActionResult> Task0e(LegalDocument_Transaction req)
        {
            var result = await _repo.Load_LegalDoc(req);
            if (result.result == Results.Success)
                return Ok(result.legaldoc);
            return NotFound();
        }
        [HttpPost]
        [Route("legaldocument/details")]
        public async Task<IActionResult> Task0f(LegalDocument_Transaction req)
        {
            var result = await _repo.Load_LegalDocDetails(req);
            if (result.result == Results.Success)
                return Ok(result.legaldocdetails);
            return NotFound();
        }
        [HttpPost]
        [Route("legaldocument/release")]
        public async Task<IActionResult> Task0b([FromBody] LegalDocument_Transaction request)
        {
            var result = await _repo.ReleaseAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Release = result.release });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("legaldocument/cancel")]
        public async Task<IActionResult> Task0c([FromBody] LegalDocument_Transaction request)
        {
            var result = await _repo.CancellAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Cancel = result.cancel });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
    }
}
