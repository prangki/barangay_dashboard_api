using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using webapi.Commons.AutoRegister;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.STLDashboardModel;
using Comm.Commons.Extensions;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using Microsoft.AspNetCore.Hosting;

namespace webapi.Controllers.STLPartylistDashboardContorller.PartyListController.ActiveAccount
{
    [ApiController]
    [Route("app/v1/stl")]
    public class ActivatorController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IActivatorData _activatorData;
        public ActivatorController(IConfiguration config, IActivatorData activatorData)
        {
            _config = config;
            _activatorData = activatorData;
        }
        [HttpPost]
        [Route("activator")]
        public async Task<IActionResult> SingIn([FromBody] STLRequestValidator request)
        {
            var validityResult = await validityAsync(request);
            if (validityResult.result == Results.Success)
                //return Ok(CreateToken(request));
                return Ok(new { Status = Results.Success, Message = "Successfully Connected", SERVERNAME = validityResult.servername });
            if (validityResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = "Invalid system owner!" });
            return NotFound();
        }
        private async Task<(Results result, object item, String servername)> validityAsync(STLRequestValidator request)
        {
            if (request == null) return (Results.Null, null, null);
            if (request.Username.IsEmpty() || request.Password.IsEmpty()) return (Results.Null, null, null);
            var IsAllow = _activatorData.String("IsAllow");
            if (IsAllow.IsEmpty() || !IsAllow.ToLower().Equals("true")) return (Results.Null, null, null);
            var hostAPI = _activatorData.String("Host");
            var Username = _activatorData.String("Username");
            var Password = _activatorData.String("Password");
            if (hostAPI.IsEmpty() || Username.IsEmpty() || Password.IsEmpty()) return (Results.Null, null, null);
            if (!(request.Host.Equals(hostAPI) && request.Username.Equals(Username) && request.Password.Equals(Password)))
                return (Results.Failed, null, null);
            return (Results.Success, null, Servername());
        }
        private String Servername()
        {
            string conString = _config.GetConnectionString("MLM04");
            DbConnection con = new SqlConnection(conString);
            string strServername = con.Database;
            return strServername;
        }
    }


    [Service.ITransient(typeof(ActivatorData))]
    public interface IActivatorData
    {
        T Data<T>(string key, bool isnullable = true);
        string String(string key);
    }
    public class ActivatorData : IActivatorData
    {
        //private Dictionary<string, object> fileData;
        public IConfiguration Configuration { get; }
        public ActivatorData(IWebHostEnvironment env)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("activator.json", optional: true, reloadOnChange: true);
                Configuration = builder.Build();
            }
            catch (Exception e) { }
        }
        public string String(string key)
        {
            if (Configuration == null) return null;
            return Configuration[key].Str();
        }
        public T Data<T>(string key, bool isnullable = true)
        {
            if (Configuration == null) return default(T);
            object source = null;
            try
            {
                string str = String(key);
                if (!str.IsEmpty())
                    try { source = JsonConvert.DeserializeObject<T>(str); } catch { }
                if (!isnullable && source == null)
                    source = System.Activator.CreateInstance(typeof(T));
            }
            catch { source = default(T); }
            return (T)source;
        }
    }
}
