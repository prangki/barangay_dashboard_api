using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.STLDashboardModel;
using Comm.Commons.Extensions;
using webapi.App.Features.UserFeature;
using Newtonsoft.Json;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.Common;
using System.IO;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class RequestDocumentController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IRequestDocumentRepository _repo;
        public RequestDocumentController(IConfiguration config, IRequestDocumentRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("reqdoc")]
        public async Task<IActionResult> Task01([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadRequestDocument(request);
            if (result.result == Results.Success)
                return Ok(result.reqdoc);
            return NotFound();
        }
        [HttpPost]
        [Route("reqdoc/new")]
        public async Task<IActionResult> Task02([FromBody] RequestDocument request)
        {
            var valResult = await validity(request);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _repo.RequestDocumentAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", ReqDocID = repoResult.reqdocid, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }


        [HttpPost]
        [Route("reqdoc/edit")]
        public async Task<IActionResult> Task03([FromBody] RequestDocument request)
        {
            var valResult = await validity(request);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _repo.UpdateRequestDocumentAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", ReqDocID = repoResult.reqdocid, URLAttachment=request.URLAttachment, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }


        [HttpPost]
        [Route("reqclearance/new")]
        public async Task<IActionResult> Task04([FromBody] RequestDocument request)
        {
            var valResult = await validity(request);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _repo.RequestBrgyClearanceAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", ReqDocID = repoResult.reqdocid, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }


        [HttpPost]
        [Route("reqclearance/edit")]
        public async Task<IActionResult> Task05([FromBody] RequestDocument request)
        {
            var valResult = await validity(request);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _repo.UpdateRequestBrgyClearanceAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", ReqDocID = repoResult.reqdocid, URLAttachment = request.URLAttachment, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }

        [HttpPost]
        [Route("reqdoc/generatereport")]
        public async Task<IActionResult> Task06([FromBody] RequestDocument request)
        {
            var valResult = await validityReport(request);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _repo.PrintRequestDocumentAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", URLDocPath=request.URL_DocPath, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }

        private async Task<(Results result, string message)> validity(RequestDocument request)
        {
            if (request == null)
                return (Results.Null, null);
            if (request.Attachment.IsEmpty())
                return (Results.Success, null);
            byte[] bytes = Convert.FromBase64String(request.Attachment.Str());
            if (bytes.Length == 0)
                return (Results.Failed, "Make sure selected image is invalid.");
            var res = await PDFService.SendAsync(bytes);
            bytes.Clear();
            if (res == null)
                return (Results.Failed, "Please contact to admin.");
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
            if (json["status"].Str() != "error")
            {
                request.URLAttachment = json["url"].Str();
                return (Results.Success, null);
            }
            return (Results.Null, "Make sure selected image is invalid");
        }
        private async Task<(Results result, string message)> validityReport(RequestDocument request)
        {
            if (request == null)
                return (Results.Null, null);
            if (request.BizReport.IsEmpty())
                return (Results.Success, null);
            byte[] bytes = Convert.FromBase64String(request.BizReport.Str());
            if (bytes.Length == 0)
                return (Results.Failed, "Make sure selected image is invalid.");
            var res = await ReportService.SendAsync(bytes, request.ControlNo);
            bytes.Clear();
            if (res == null)
                return (Results.Failed, "Please contact to admin.");
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
            if (json["status"].Str() != "error")
            {
                request.URL_DocPath = json["url"].Str();

                return (Results.Success, null);
            }
            return (Results.Null, "Make sure selected image is invalid");
        }
    }
}
