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
    public class RequestCedulaController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IRequestCedulaRepository _repo;
        private readonly IAccountRepository _loginrepo;


        public RequestCedulaController(IConfiguration config, IRequestCedulaRepository repo, IAccountRepository loginrepo)
        {
            _repo = repo;
            _config = config;
            _loginrepo = loginrepo;
        }

        [HttpPost]
        [Route("request/cedula/save")]
        public async Task<IActionResult> Save([FromBody] BrgyCedula data)
        {
            var result = await _repo.Save(data);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("request/cedula/load")]
        public async Task<IActionResult> Load(string filterId, string from, string to)
        {
            var result = await _repo.Load(filterId, from, to);
            if (result.result == Results.Success)
                return Ok(result.list);
            else
                return NotFound();

        }

        [HttpPost]
        [Route("request/cedula/release")]
        public async Task<IActionResult> Release(string ctcno, string date, string releasedby)
        {
            var result = await _repo.Release(ctcno, date, releasedby);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("request/cedula/release/update")]
        public async Task<IActionResult> Reschedule(string ctcno, string date)
        {
            var result = await _repo.Reschedule(ctcno, date);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("request/cedula/cancel")]
        public async Task<IActionResult> Cancel(string ctcno, string date, string canceledby)
        {
            var result = await _repo.Cancel(ctcno, date, canceledby);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("request/cedula/update")]
        public async Task<IActionResult> Update([FromBody] BrgyCedula data)
        {
            var result = await _repo.Update(data);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("cedula/report")]
        public async Task<IActionResult> LoadCedulaReport(string from, string to)
        {
            var result = await _repo.LoadCedulaReport(from, to);
            if (result.result == Results.Success)
                return Ok(result.report);
            else
                return NotFound();

        }
    }
}
