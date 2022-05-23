using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using webapi.App.Aggregates.Common;
using webapi.App.Model.User;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Features;
using webapi.App.RequestModel.SubscriberApp.Features.Winning;
using webapi.App.RequestModel.Common;

namespace webapi.Controllers.SubscriberAppControllers.Features
{
    [Route("app/v1/stl/winning")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class WinningController : ControllerBase
    {
        private readonly IWinningRepository _winningRepo;
        public WinningController(IWinningRepository winningRepo){
            _winningRepo = winningRepo;
        }

        
        [HttpPost]
        [Route("claiming")]
        public async Task<IActionResult> Task01([FromBody] ClaimingRequest request){
            if(!ClaimingRequest.validity0a(request)) return NotFound();
            var repoResult = await _winningRepo.ClaimingAsync(request);
            if(repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Message = repoResult.message });
            else if(repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }

        [HttpPost]
        [Route("process")]
        public async Task<IActionResult> Task02([FromBody] FilterRequest filter){
            if(!FilterRequest.validity0g(filter)) return NotFound();
            var repoResult = await _winningRepo.ProcessedAsync(filter);
            if(repoResult.result == Results.Success)
                return Ok(repoResult.items);
            return NotFound();
        }
        
        [HttpPost]
        [Route("complete")]
        public async Task<IActionResult> Task03([FromBody] FilterRequest filter){
            if(!FilterRequest.validity0g(filter)) return NotFound();
            var repoResult = await _winningRepo.CompletedAsync(filter);
            if(repoResult.result == Results.Success)
                return Ok(repoResult.items);
            return NotFound();
        } 

        
        [HttpPost]
        [Route("/app/v1/subscriber/remittances")]
        public async Task<IActionResult> Task(){
            var repoResult = await _winningRepo.RemittancesAsync();
            if(repoResult.result == Results.Success)
                return Ok(repoResult.items);
            return NotFound();
        } 
    }
}