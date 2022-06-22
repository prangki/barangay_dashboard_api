using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class BlotterController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IBlottersRepository _repo;
        private readonly IAccountRepository _loginrepo;

        public BlotterController(IConfiguration config, IBlottersRepository repo, IAccountRepository loginrepo)
        {
            _repo = repo;
            _config = config;
            _loginrepo = loginrepo;
        }
        [HttpPost]
        [Route("blotter/save")]
        public async Task<IActionResult> Save([FromBody] Blotter info)
        {
            var result = await _repo.SaveFile(info);
            if (result.result == Results.Success)
                return Ok(result.blotter);
            return NotFound();
        }

        [HttpPost]
        [Route("blotter/load")]
        public async Task<IActionResult> Load(string plid, string pgrpid)
        {
            var result = await _repo.LoadBlotter(plid, pgrpid);
            if (result.result == Results.Success)
                return Ok(result.blotter);
            return NotFound();
        }

        [HttpPost]
        [Route("blotter/summon/load")]
        public async Task<IActionResult> Load(string plid, string pgrpid, string brgycsno, string createdby, string summondt, int issummon, string modifiedby, string dtmodified)
        {
            var result = await _repo.LoadSummon(plid, pgrpid, brgycsno, createdby, summondt,issummon,modifiedby,dtmodified);
            if (result.result == Results.Success)
                return Ok(result.summon);
            return NotFound();
        }

        [HttpPost]
        [Route("blotter/maxcaseno")]
        public async Task<IActionResult> GetMaxCaseNo()
        {
            var result = await _repo.UpdatedCaseNo();
            if (result.result == Results.Success)
                return Ok(result.caseNo);
            return NotFound();
        }
    }
}
