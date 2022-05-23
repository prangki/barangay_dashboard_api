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
    public class CreditServiceController : ControllerBase
    {
        private readonly ICreditServiceRepository _serviceRepo;
        public CreditServiceController(ICreditServiceRepository serviceRepo){
            _serviceRepo = serviceRepo;
        }

        [HttpPost]
        [Route("credit/buy")]
        public async Task<IActionResult> Task0a([FromBody] BuyCreditRequest request){
            if(!validity(request)) return NotFound();
            var repoResult = await _serviceRepo.BuyCreditAsync(request);
            if(repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Message = repoResult.message });
            else if(repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }
        [HttpPost]
        [Route("credit/pasa")]
        public async Task<IActionResult> Task0b([FromBody] PasaCreditRequest request){
            if(!validity(request)) return NotFound();
            var repoResult = await _serviceRepo.PasaCreditAsync(request);
            if(repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Message = repoResult.message });
            else if(repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }
        /*[HttpPost]
        [Route("credit/to/wc")]
        public async Task<IActionResult> Task0c([FromBody] WinCreditRequest request){
            if(!validity(request)) return NotFound();
            var repoResult = await _serviceRepo.GameCreditToWinCreditAsync(request);
            if(repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Message = repoResult.message });
            else if(repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }*/
        [HttpPost]
        [Route("ledger")]
        public async Task<IActionResult> Task0d([FromBody] FilterRequest filter){
            if(!FilterRequest.validity0a(filter)) return NotFound();
            if(!FilterRequest.validity0g(filter)) return NotFound();
            var repoResult = await _serviceRepo.CreditLedgerAsync(filter);
            if(repoResult.result == Results.Success)
                return Ok(repoResult.ledger);
            return NotFound();
        }
        //
        private bool validity(BuyCreditRequest request){
            if(request==null)
                return false;
            if(request.Amount<1)
                return false;

            return true;
        }
        private bool validity(PasaCreditRequest request){
            if(request==null)
                return false;
            if(request.Amount<10)
                return false;
            
            string subscriber = request.Subscriber.Str().Trim();
            if(subscriber.Length==10&subscriber.StartsWith("9"))
                request.Subscriber = $"+63{ subscriber }";
            else if(subscriber.Length==11&subscriber.StartsWith("09"))
                request.Subscriber = $"+63{ subscriber.Substring(1) }";
                
            return true;
        }
        private bool validity(WinCreditRequest request){
            if(request==null)
                return false;
            if(request.Amount<10)
                return false;

            return true;
        }
    }
}