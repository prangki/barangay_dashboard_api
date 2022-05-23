using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.STLDashboardModel;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class TextBlastingController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITextBlastingRepository _repo;
        private readonly IAccountRepository _loginrepo;
        public TextBlastingController(IConfiguration config, ITextBlastingRepository repo, IAccountRepository loginrepo)
        {
            _config = config;
            _repo = repo;
            _loginrepo = loginrepo;
        }
        [HttpPost]
        [Route("mobileno")]
        public async Task<IActionResult> Statistic(AcctStatistic acct)
        {
            var result = await _repo.Load_Mobile(acct);
            if (result.result == Results.Success)
                return Ok(result.mob);
            return NotFound();
        }

        [HttpPost]
        [Route("accnt/mobileno")]
        public async Task<IActionResult> AcctMobile(TextBlastingMemberAccount acct)
        {
            var result = await _repo.Load_MobileMemberAccount(acct);
            if (result.result == Results.Success)
                return Ok(result.mob);
            return NotFound();
        }
        [HttpPost]
        [Route("textblast")]
        public async Task<IActionResult> Statistic(TextBlasting mob)
        {
            var result = await _repo.TextBlast(mob);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("textblast/generatednumber")]
        public async Task<IActionResult> Gen_Number([FromBody] TextBlasting mob)
        {
            var result = await _repo.TextBlast_Gen_Number(mob);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("accnt/totalmember")]
        public async Task<IActionResult> TotalMember(TextBlastingMemberAccount acct)
        {
            var result = await _repo.TotalMemberAccount(acct);
            if (result.result == Results.Success)
                return Ok(result.mob);
            return NotFound();
        }
        [HttpPost]
        [Route("prefix")]
        public async Task<IActionResult> PrefixMobileNumber(string search)
        {
            var result = await _repo.Load_PhilPrefix(search);
            if (result.result == Results.Success)
                return Ok(result.prefix);
            return NotFound();
        }
        [HttpPost]
        [Route("prefix/generate")]
        public async Task<IActionResult> GenerateMobileNumber([FromBody] GenerateBlasting prefix)
        {
            var result = await _repo.GenerateTextBlast(prefix);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("mobilenumber/generated")]
        public async Task<IActionResult> GeneratedMobileNumber(int row)
        {
            var result = await _repo.GeneratedMobileNumber(row);
            if (result.result == Results.Success)
                return Ok(result.mob);
            return NotFound();
        }

        [HttpPost]
        [Route("textblast/individual")]
        public async Task<IActionResult> Statistic([FromBody] IndividualText mob)
        {
            var result = await _repo.IndividualText(mob);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

    }
}
