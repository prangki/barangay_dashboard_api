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
            var result = await _repo.SaveAppointedMember(appoint);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("appoint/member/load")]
        public async Task<IActionResult> LoadAppointedMember(int recordType)
        {
            var result = await _repo.LoadAppointMember(recordType);
            if (result.result == Results.Success)
                return Ok(result.appoint);
            else
                return NotFound();

        }

        [HttpPost]
        [Route("appoint/member/approve")]
        public async Task<IActionResult> Approve(string appointid)
        {
            var result = await _repo.Approve(appointid);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("appoint/member/approveselect")]
        public async Task<IActionResult> ApproveBySelection(string groupid)
        {
            var result = await _repo.ApproveBySelection(groupid);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("appoint/member/withdraw")]
        public async Task<IActionResult> Withdraw(string appointid)
        {
            var result = await _repo.Withdraw(appointid);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("appoint/member/withdrawselect")]
        public async Task<IActionResult> WithdrawBySelection(string groupid)
        {
            var result = await _repo.WithdrawBySelection(groupid);
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
        [Route("appoint/member/removeselect")]
        public async Task<IActionResult> RemoveBySelection(string groupid)
        {
            var result = await _repo.RemoveBySelection(groupid);
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

        [HttpPost]
        [Route("appoint/update/oath")]
        public async Task<IActionResult> UpdateOathDate(string date)
        {
            var result = await _repo.UpdateDate(date);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("appoint/update/statement")]
        public async Task<IActionResult> UpdateStatementDate(string date)
        {
            var result = await _repo.UpdateDate(date,true);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }


    }
}
