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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class PBXController : ControllerBase
    {
        private readonly IConfiguration _config;
        private static string token = null;
        private static string APIUrl = null;

        public PBXController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        [Route("pbx/login")]
        public async Task<IActionResult> LoadPBXAPICredential()
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                APIUrl = _config["PBX:Url"].Str() + "/openapi/v1.0/";

                dynamic request = new
                {
                    username = _config["PBX:Username"].Str(),
                    password = _config["PBX:Password"].Str()
                };

                var jsonString = JsonConvert.SerializeObject(request, Formatting.None);
                var response = SendAPIPostRequest(APIUrl + "get_token", jsonString);

                var result = JsonConvert.DeserializeObject<dynamic>(response);

                if (result.errcode != 0)
                    return Ok(new { errCode = 2004, errMessage = "API connection failed" });

                token = result.access_token;
                return Ok(new
                {
                    errCode = 0,
                    respMessage = "SUCCESS"
                });
            }
            catch (Exception)
            {
                return Ok(new
                {
                    errCode = 2001,
                    respMessage = "API connection failure"
                });
            }
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

        [HttpPost]
        [Route("pbx/extension/total")]
        public async Task<IActionResult> GetTotalExtensions()
        {
            string path = $"extension/list?access_token={token}";
            var response = SendAPIGetRequest(APIUrl + path);

            var result = new
            {
                errCode = 0,
                respMessage = "SUCCESS",
                totalNumber = JsonConvert.DeserializeObject<dynamic>(response).total_number
            };

            return Ok(result);
        }

        [HttpPost]
        [Route("pbx/extension/search")]
        public async Task<IActionResult> SearchExtension(string extension)
        {
            string path = $"extension/search?search_value={extension}&access_token={token}";
            var response = SendAPIGetRequest(APIUrl + path);
            var jObject = JsonConvert.DeserializeObject<dynamic>(response);
            if(jObject.total_number != 0)
            {
                return Ok(new {
                    errCode = 0,
                    respMessage = "SUCCESS",
                    extension = jObject.data[0].number,
                    name = jObject.data[0].caller_id_name,
                    type = "extension",
                    id = jObject.data[0].id
                });
            }

            return Ok(new {
                errCode = 2002,
                respMessage = "NO DATA FOUND"
            });
        }
        
        [HttpPost]
        [Route("pbx/extension/add")]
        public async Task<IActionResult> AddExtension(object request)
        {
            var path = $"extension/create?access_token={token}";
            var jsonString = JsonConvert.SerializeObject(request, Formatting.None);
            var response = SendAPIPostRequest(APIUrl + path, jsonString);
            var jObject = JsonConvert.DeserializeObject<dynamic>(response);
            if (jObject.errcode != 0)
                return Ok(new { errCode = 2002, respMessage = "NO DATA FOUND" });

            return Ok(new
            {
                errCode = 0,
                respMessage = "SUCCESS"
            });
        }

        [HttpPost]
        [Route("pbx/outbound_route/update")]
        public async Task<IActionResult> UpdateOutboundRoute(object data)
        {
            var result = GetListOutboundRoute();

            if(result.Value.errCode != 0)
                return Ok(new { errCode = 2002, respMessage = "NO DATA FOUND" });

            var list = result.Value.extensionList;
            list.Add(data);

            var request = new
            {
                id = 1,
                ext_list = list
            };

            var path = $"outbound_route/update?access_token={token}";
            var jsonString = JsonConvert.SerializeObject(request, Formatting.None);
            var response = SendAPIPostRequest(APIUrl + path, jsonString);
            var jObject = JsonConvert.DeserializeObject<dynamic>(response);
            if(jObject.errcode != 0)
                return Ok(new { errCode = 2003, respMessage = "FAILED" });
            
            return Ok(new
            {
                errCode = 0,
                respMessage = "SUCCESS"
            });
        }

        private dynamic GetListOutboundRoute()
        {
            var path = $"outbound_route/get?id=1&access_token={token}";
            var response = SendAPIGetRequest(APIUrl + path);
            var jObject = JsonConvert.DeserializeObject<dynamic>(response);
            if (jObject.data == null)
                return Ok(new { errCode = 2002, respMessage = "NO DATA FOUND" });

            return Ok(new { 
                errCode = 0,
                respMessage = "SUCCESS",
                extensionList = jObject.data.ext_list.ToObject(typeof(List<dynamic>))
            });
        }

        [HttpPost]
        [Route("pbx/delete/token")]
        public async Task<IActionResult> DisposeToken()
        {
            var path = $"del_token?access_token={token}";
            var response = SendAPIGetRequest(APIUrl + path);
            var jObject = JsonConvert.DeserializeObject<dynamic>(response);
            if (jObject.errcode != 0)
                return Ok(new { errCode = 2003, respMessage = "FAILED" });

            return Ok(new
            {
                errCode = 0,
                respMessage = "SUCCESS"
            });
        }

        private string SendAPIPostRequest(string URL, string body)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "Google Chrome");
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var res = client.PostAsync(URL, content).Result;
            var result = res.Content.ReadAsStringAsync().Result;

            return result;
        }

        private string SendAPIGetRequest(string param)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "Softphone");
            var res = client.GetAsync(param).Result;
            var result = res.Content.ReadAsStringAsync().Result;

            return result;
        }

    }
}
