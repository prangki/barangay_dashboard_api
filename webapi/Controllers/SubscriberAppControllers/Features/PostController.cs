using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using webapi.App.Aggregates.Common;
using webapi.App.Model.User;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Features;

namespace webapi.Controllers.SubscriberAppControllers.Features
{
    [Route("app/v1/stl/post")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepo;
        public PostController(IPostRepository postRepo){
            _postRepo = postRepo;
        }
        
        [HttpPost]
        [Route("announcement")]
        public async Task<IActionResult> Task01([FromBody] FilterRequest filter){
            if(!FilterRequest.validity0g(filter)) return NotFound();
            var repoResult = await _postRepo.AnnouncementAsync(filter);
            if(repoResult.result == Results.Success)
                return Ok(repoResult.items);
            return NotFound();
        } 
        [HttpPost]
        [Route("helpcenter")]
        public async Task<IActionResult> Task02([FromBody] FilterRequest filter){
            if(!FilterRequest.validity0b(filter)) return NotFound();
            var repoResult = await _postRepo.HelpCenterAsync(filter);
            if(repoResult.result == Results.Success)
                return Ok(repoResult.items);
            return NotFound();
        } 
    }
}