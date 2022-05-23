using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using webapi.App.Aggregates.Common;
using webapi.App.Model.User;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;

using webapi.App.RequestModel.SubscriberApp.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Features;
using webapi.App.RequestModel.Common;
using Comm.Commons.Extensions;

namespace webapi.Controllers.SubscriberAppControllers.Features
{
    [Route("app/v1/stl")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class SalesReportController : ControllerBase
    {
        private readonly ISalesReportRepository _reportRepo;
        public SalesReportController(ISalesReportRepository reportRepo){
            _reportRepo = reportRepo;
        }
        [HttpPost]
        [Route("sales/report")]
        public async Task<IActionResult> Task0d([FromBody] FilterRequest filter){
            if(!FilterRequest.validity0a(filter)) return NotFound();
            if(!FilterRequest.validity0g(filter)) return NotFound();
            var repoResult = await _reportRepo.MaxSalesReportAsync(filter);
            if(repoResult.result == Results.Success)
                return Ok(repoResult.report);
            return NotFound();
        }
    }
}