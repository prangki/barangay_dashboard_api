﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comm.Commons.Extensions;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Features.UserFeature;
using webapi.App.RequestModel.AppRecruiter;
using Newtonsoft.Json;
using webapi.App.STLDashboardModel;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class BrgyClearanceController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IBrgyClearanceRepository _repo;
        public BrgyClearanceController(IConfiguration config, IBrgyClearanceRepository repo)
        {
            _config = config;
            _repo = repo;
        }


        [HttpPost]
        [Route("brgyclearance/new")]
        public async Task<IActionResult> Task0a([FromBody] BrgyClearance request)
        {
            var result = await _repo.BrgyClearanceAsync(request);
            if (result.result == Results.Success)
            {
                return Ok(new { Status = "ok", Message = result.message, ClearanceNo = result.brgyclrid, ControlNo = result.cntrlno });
            }
            else if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message, URLDocument = "" });
            return NotFound();
        }

        [HttpPost]
        [Route("brgyclearance/export")]
        public async Task<IActionResult> Task0a1([FromBody] BrgyClearance request)
        {
            var valresult = await validityReport(request);
            if (valresult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valresult.message });
            if (valresult.result != Results.Success)
                return NotFound();

            var result = await _repo.Generate_BrgyClearance(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message});
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("brgyclearance/release")]
        public async Task<IActionResult> Task0b([FromBody] BrgyClearance request)
        {
            var result = await _repo.ReleaseAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Release = result.release });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("brgyclearance/cancel")]
        public async Task<IActionResult> Task0c([FromBody] BrgyClearance request)
        {
            var result = await _repo.CancellAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Cancel = result.cancel });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("brgyclearance")]
        public async Task<IActionResult> Task0d([FromBody] BrgyClearance request)
        {
            var result = await _repo.Load_BrgyClearance(request);
            if (result.result == Results.Success)
                return Ok(result.bryclrid);
            return NotFound();
        }
        [HttpPost]
        [Route("brgyclearance/id")]
        public async Task<IActionResult> Task0d1([FromBody] BrgyClearance request)
        {
            var result = await _repo.Load_BrgyClearanceID(request);
            if (result.result == Results.Success)
                return Ok(result.bryclrid);
            return NotFound();
        }
        [HttpPost]
        [Route("brgyclearance/received")]
        public async Task<IActionResult> Task0e([FromBody] BrgyClearance request)
        {
            var result = await _repo.ReceivedBrgyClearanceRequestAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("brgyclearance/process")]
        public async Task<IActionResult> Task0f([FromBody] BrgyClearance request)
        {
            var result = await _repo.ProcessRecivedBrgyClearanceRequestAsync(request);
            if (result.result == Results.Success)
            {
                return Ok(new { Status = "ok", Message = result.message });
            }
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("requestdocument/list")]
        public async Task<IActionResult> Task0g([FromBody] FilterRequestDoc request)
        {
            var result = await _repo.Load_RequestDocument(request);
            var res = await _repo.TotalRequestDocumentAsyn(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", requestdocument = result.reqdoc, totalrequestdocument = res.total_reqdoc });
            if(result.result == Results.Failed)
                return Ok(new { Status = "error", requestdocument = result.reqdoc, totalrequestdocument = res.total_reqdoc });
            return NotFound();
        }

        private async Task<(Results result, string message)> validityReport(BrgyClearance request)
        {
            if (request == null)
                return (Results.Null, null);
            if (request.ExportedDocument.IsEmpty())
                return (Results.Success, null);
            byte[] bytes = Convert.FromBase64String(request.ExportedDocument.Str());
            if (bytes.Length == 0)
                return (Results.Failed, "Make sure you have internet connection.");
            var res = await ReportService.SendAsync(bytes, $"barangayclearance_{request.PLID}{request.PGRPID}{request.ClearanceID}");
            bytes.Clear();
            if (res == null)
                return (Results.Failed, "Please contact to admin.");
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
            if(json["status"].Str() != "error")
            {
                //request.URLDocument = json["url"].Str();
                request.URLDocument = (json["url"].Str()).Replace(_config["Portforwarding:LOCAL"].Str(), _config["Portforwarding:URL"].Str());
                return (Results.Success, null);
            }
            return (Results.Null, "Make sure you have internet connection.");
        }
    }
}
