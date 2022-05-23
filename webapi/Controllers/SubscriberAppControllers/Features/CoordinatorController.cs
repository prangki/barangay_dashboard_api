using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using webapi.App.Aggregates.Common;
using webapi.App.Model.User;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Features;
using webapi.App.RequestModel.Common;

using webapi.App.RequestModel.SubscriberApp.Features.Coordinator;
using Comm.Commons.Extensions;

namespace webapi.Controllers.SubscriberAppControllers.Features
{
    [Route("app/v1/stl/a")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class CoordinatorController : ControllerBase
    {
        private readonly ISubscriber _identity;
        private readonly ICoordinatorRepository _coordRepo;
        public STLAccount account { get{ return _identity.AccountIdentity(); } } 
        public CoordinatorController(ISubscriber identity, ICoordinatorRepository coordRepo){
            _identity = identity; 
            _coordRepo = coordRepo;
        }

        [HttpPost]
        [Route("invite")]
        public async Task<IActionResult> Task0a([FromBody] InvitationRequest request){
            //if(account.IsPlayer) return Unauthorized();
            if(!validity(request)) return NotFound();
            var repoResult = await _coordRepo.InvitationAsync(request);
            if(repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Message = repoResult.message });
            else if(repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }
        [HttpPost]
        [Route("credit/requests")]
        public async Task<IActionResult> Task0b([FromBody] FilterRequest filter){
            //if(account.IsPlayer) return Unauthorized();
            if(!FilterRequest.validity0c(filter)) return NotFound();
            var repoResult = await _coordRepo.RequestingCreditsAsync(filter);
            if(repoResult.result == Results.Success)
                return Ok(repoResult.items);
            return NotFound();
        }
        [HttpPost]
        [Route("credit/complete")]
        public async Task<IActionResult> Task0c([FromBody] FilterRequest filter){
            //if(account.IsPlayer) return Unauthorized();
            if(!FilterRequest.validity0c(filter)) return NotFound();
            var repoResult = await _coordRepo.ApprovedCreditAsync(filter);
            if(repoResult.result == Results.Success)
                return Ok(new {Summary = repoResult.summary, List = repoResult.items});
            return NotFound();
        }
        [HttpPost]
        [Route("credit/approval")]
        public async Task<IActionResult> Task0d([FromBody] RequestingCreditRequest request){
            //if(account.IsPlayer) return Unauthorized();
            if(!RequestingCreditRequest.validity0a(request)) return NotFound();
            var repoResult = await _coordRepo.CreditApprovalAsync(request);
            if(repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Message = repoResult.message, Data = repoResult.data }); // , IsCancelled = (!request.IsApproved)
            else if(repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }
        [HttpPost]
        [Route("credit/paid")]
        public async Task<IActionResult> Task0e([FromBody] RequestingCreditRequest request){
            //if(account.IsPlayer) return Unauthorized();
            if(!RequestingCreditRequest.validity0b(request)) return NotFound();
            var repoResult = await _coordRepo.CreditPaidAsync(request);
            if(repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Message = repoResult.message });
            else if(repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }
        [HttpPost]
        [Route("ledger")]
        public async Task<IActionResult> Task0f([FromBody] FilterRequest filter){
            //if(account.IsPlayer) return Unauthorized();
            if(!FilterRequest.validity0a(filter)) return NotFound();
            if(!FilterRequest.validity0g(filter)) return NotFound();
            var repoResult = await _coordRepo.CommissionLedgerAsync(filter);
            if(repoResult.result == Results.Success)
                return Ok(repoResult.ledger);
            return NotFound();
        }
        [HttpPost]
        [Route("customers")]
        public async Task<IActionResult> Task0g([FromBody] FilterRequest filter){
            //if(account.IsPlayer) return Unauthorized();
            if(!FilterRequest.validity0d(filter)) return NotFound();
            var repoResult = await _coordRepo.SubscribersAsync(filter);
            if(repoResult.result == Results.Success)
                return Ok(repoResult.items);
            return NotFound();
        }
        //
        private bool validity(InvitationRequest request){
            if(request==null)
                return false;
            var form = request.RequestForm;

            string mobilenumber = form.MobileNumber.Str().Trim();
            if(mobilenumber.IsEmpty())
                return false;
            if(mobilenumber.Length==10 && mobilenumber.IndexOf("9")==0)
                mobilenumber = $"+63{ mobilenumber }";
            else if(mobilenumber.Length==11 && mobilenumber.IndexOf("09")==0)
                mobilenumber = $"+63{ mobilenumber.Substring(1) }";

            if(mobilenumber.Length!=13) 
                return false;
            form.MobileNumber = mobilenumber;

            //if(account.IsGeneralCoordinator)
                //form.SharedCommission = ((int)form.SharedCommission.Str().ToDecimalDouble()).Str();
            //else form.SharedCommission = "0";
            return true;
        }
    }
}