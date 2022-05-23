using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Comm.Commons.Extensions;
using webapi.App.Aggregates.Common;
using webapi.App.Model.User;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Features;
using webapi.App.RequestModel.SubscriberApp.Features.LotteryGame;
using webapi.App.RequestModel.Common;

namespace webapi.Controllers.SubscriberAppControllers.Features
{
    [Route("app/v1/stl/lottery")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class LotteryGameController : ControllerBase
    {
        private readonly ISubscriber _identity;
        private readonly ILotteryGameRepository _gameRepo;
        public STLAccount account { get{ return _identity.AccountIdentity(); } }
        public LotteryGameController(ISubscriber identity, ILotteryGameRepository gameRepo){
            _identity = identity;
            _gameRepo = gameRepo;
        }

        //
        [HttpPost]
        [Route("profile")]
        public async Task<IActionResult> Task0a(){
            var repoResult = await _gameRepo.ProfileSettingsAsync();
            if(repoResult.result == Results.Success)
                return Ok(repoResult.item);
            return NotFound();
        }
        
        /*[HttpPost]
        [Route("settings")]
        public async Task<IActionResult> Task0b(){
            var repoResult = await _gameRepo.GameSettingsAsync();
            if(repoResult.result == Results.Success)
                return Ok(repoResult.item);
            return NotFound();
        }*/

        [HttpPost]
        [Route("play")]
        public async Task<IActionResult> Task0c([FromBody] PlaceBetRequest request){
            if(!PlaceBetRequest.validity(request)) return NotFound();
            var repoResult = await _gameRepo.PlaceBetAsync(request);
            if(repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Message = repoResult.message, Tickets = repoResult.Tickets });
            if(repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message, Tickets = repoResult.Tickets });
            return NotFound();
        }

        [HttpPost]
        [Route("play/history")]
        public async Task<IActionResult> Task0d([FromBody] FilterRequest request){
            if(!FilterRequest.validity0h(request)) return NotFound();
            var repoResult = await _gameRepo.BetHistoryAsync(request);  
            if(repoResult.result == Results.Success)
                return Ok(repoResult.tickets);
            return NotFound();
        }
        
        [HttpPost]
        [Route("reprint/{TransactionNo}")]
        public async Task<IActionResult> Task0e(String TransactionNo){
            //if(!account.IsTerminal || TransactionNo.IsEmpty()) return NotFound();
            var repoResult = await _gameRepo.ReprintLogsAsync(TransactionNo);  
            if(repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Message = repoResult.message, Ticket = repoResult.Ticket });
            if(repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message, });
            return NotFound();
        }

        [HttpPost]
        [Route("results")]
        public async Task<IActionResult> Task0f([FromBody] FilterRequest request){
            if(!FilterRequest.validity0h(request)) return NotFound();
            var repoResult = await _gameRepo.ResultsAsync(request);  
            if(repoResult.result == Results.Success)
                return Ok(repoResult.items); 
            return NotFound();
        }
        
        [HttpPost]
        [Route("winnings")]
        public async Task<IActionResult> Task0g([FromBody] FilterRequest request){
            if(!FilterRequest.validity0g(request)) return NotFound();
            var repoResult = await _gameRepo.WinningResultsAsync(request);  
            if(repoResult.result == Results.Success)
                return Ok(repoResult.items); 
            return NotFound();
        }
    }
}