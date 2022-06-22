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

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class DocumentTypeController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IDocumentTypeRepository _repo;
        public DocumentTypeController(IConfiguration config, IDocumentTypeRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("doctype")]
        public async Task<IActionResult> Task01([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadMDocumentType(request);
            if (result.result == Results.Success)
                return Ok(result.doctype);
            return NotFound();
        }
        [HttpPost]
        [Route("doctype/new")]
        public async Task<IActionResult> Task02([FromBody] DocumentType request)
        {
            var repoResult = await _repo.DocumentTypeAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", DocTypeID = repoResult.doctypeid, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }
        [HttpPost]
        [Route("doctype/edit")]
        public async Task<IActionResult> Task03([FromBody] DocumentType request)
        {

            var repoResult = await _repo.UpdateDocumentTypeAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", DocTypeID = request.DocTypeID, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }

        [HttpPost]
        [Route("docrqrd")]
        public async Task<IActionResult> Task04([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadMDocRequirement(request);
            if (result.result == Results.Success)
                return Ok(result.docrqrd);
            return NotFound();
        }
    }
}
