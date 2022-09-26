using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.STLDashboardModel;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class AppointedMemberController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAppointedMemberRepository _repo;
        private readonly IAccountRepository _loginrepo;


        public AppointedMemberController(IConfiguration config, IAppointedMemberRepository repo, IAccountRepository loginrepo)
        {
            _repo = repo;
            _config = config;
            _loginrepo = loginrepo;
        }

        [HttpPost]
        [Route("appoint/member/save")]
        public async Task<IActionResult> SaveAppointedMember([FromBody] AppointDetails appoint)
        {
            var result = await _repo.SaveAppointedMember(appoint.JsonString);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("appoint/member/load")]
        public async Task<IActionResult> LoadAppointedMember()
        {
            var result = await _repo.LoadAppointMember();
            if (result.result == Results.Success)
                return Ok(result.appoint);
            else
                return NotFound();

        }

        [HttpPost]
        [Route("appoint/member/withdraw")]
        public async Task<IActionResult> WithdrawAppointedMember(string appointid)
        {
            var result = await _repo.WithdrawAppointedMember(appointid);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("appoint/member/remove")]
        public async Task<IActionResult> RemoveAppointedMember(string appointid)
        {
            var result = await _repo.RemoveAppointedMember(appointid);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("appoint/member/detail")]
        public async Task<IActionResult> ViewAppointedMemberDetails(string id)
        {
            var result = await _repo.ViewAppointedMemberDetails(id);
            if (result.result == Results.Success)
                return Ok(result.details);
            else
                return NotFound();

        }

        [HttpPost]
        [Route("appoint/document")]
        public async Task<IActionResult> ViewDocumentTags(string id)
        {
            var result = await _repo.ViewDocumentTags(id);
            if (result.result == Results.Success)
                return Ok(result.details);
            else
                return NotFound();

        }
    }
}
