using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.STLDashboardModel;
using Comm.Commons.Extensions;
using Newtonsoft.Json;
using System;
using webapi.App.Features.UserFeature;
using System.Collections.Generic;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class PBXController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PBXController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        [Route("pbx/credential")]
        public async Task<IActionResult> LoadPBXAPICredential()
        {
            dynamic credential = new
            {
                Username = _config["PBX:Username"].Str(),
                Password = _config["PBX:Password"].Str(),
                Url = _config["PBX:Url"].Str()
            };
            
            return Ok(credential);
        }

        [HttpPost]
        [Route("pbx/database/credential")]
        public async Task<IActionResult> LoadPBXDBCredential()
        {
            dynamic credential = new
            {
                ConnectionString = _config["PBXDatabase:ConnectionString"].Str()
            };

            return Ok(credential);
        }


    }
}
