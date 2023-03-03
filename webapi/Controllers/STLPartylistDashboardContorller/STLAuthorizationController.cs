using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

using Comm.Commons.Advance;
using Comm.Commons.Extensions;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard;
using webapi.App.Model.User;
using webapi.App.RequestModel.AppRecruiter;

namespace webapi.Controllers.STLPartylistDashboardContorller
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    public class STLAuthorizationController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAccountRepository _repo;
        public STLAuthorizationController(IConfiguration config, IAccountRepository repo)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost]
        [Route("account/changepassword")]
        public async Task<IActionResult> ForgotPasswordConfirm([FromBody] RequiredChangePassword request)
        {
            var repoResult = await _repo.RequiredChangePassword(request);
            if (repoResult.result == Results.Success)
            {
                return Ok(new { Status = "ok", Message = repoResult.message });
            }
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }

        [HttpPost]
        [Route("dashboardsignin")]
        public async Task<IActionResult> Task0a([FromBody] STLSignInRequest request)
        {
            var result = await _repo.DashboardSignInAsync(request);
            if (result.result == Results.Success)
            {
                var token = CreateToken(result.account);
                var menu = await _repo.LoadPGS(result.account.USR_ID.Str(),result.account.ACT_TYP);
                var data = await _repo.MemberGroup(result.account);
                return Ok(new { result = result.result, account=result.account, auth = token, Company = data.PartyList, group=data.Group,menupage=menu.menu });
            }
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, Message = result.message });
            return NotFound();
        }
        private object CreateToken(STLAccount user)
        {
            var guid = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim("token", Cipher.Encrypt(JsonConvert.SerializeObject(new{
                    GUID = Cipher.MD5Hash(guid),
                    PL_ID = user.PL_ID,
                    PGRP_ID = user.PGRP_ID,
                    //PSNCD = user.PSN_CD,
                    LOC_BRGY=user.LOC_BRGY,
                    USR_ID=user.USR_ID,
                    ACT_ID=user.ACT_ID,
                    MOB_NO=user.MOB_NO,
                    ACT_TYP=user.ACT_TYP,
                    SUB_TYP=user.SUB_TYP,
                    CAN_CREATE = user.CAN_CREATE,
                    CAN_UPDATE = user.CAN_UPDATE,
                    CAN_DELETE = user.CAN_DELETE,
                    PROFILE_ACCESS = user.PROFILE_ACCESS

                }), guid)),
                new Claim(ClaimTypes.Name, user.FLL_NM),
                new Claim(JwtRegisteredClaimNames.Jti, guid),
            };

            DateTime now = DateTime.Now;
            string Issuer = _config["TokenSettings:Issuer"]
                , Audience = _config["TokenSettings:Audience"]
                , Key = _config["TokenSettings:Key"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
                var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                notBefore: now,
                expires: now.Add(TimeSpan.FromSeconds(30)),
                claims: claims,
                signingCredentials: signInCred
            );
            return new { Token = new JwtSecurityTokenHandler().WriteToken(token), ExpirationDate = token.ValidTo, };
        }

        [HttpPost]
        [Route("dashboardsignin02")]
        public async Task<IActionResult> Task0a02([FromBody] STLSignInRequest request)
        {
            var result = await _repo.DashboardSignInAsync02(request);
            if (result.result == Results.Success)
            {
                var token = CreateToken(result.account);
                var menu = await _repo.LoadPGS02(result.account.PROFILE_ID.Str(), result.account.ACT_TYP);
                var data = await _repo.MemberGroup(result.account);
                return Ok(new { result = result.result, account = result.account, auth = token, Company = data.PartyList, group = data.Group, menupage = menu.menu });
            }
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, Message = result.message });
            return NotFound();
        }

    }
}
