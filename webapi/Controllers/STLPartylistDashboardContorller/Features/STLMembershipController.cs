using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.RequestModel.AppRecruiter;
using Comm.Commons.Extensions;
using webapi.App.Features.UserFeature;
using Newtonsoft.Json;
using webapi.App.Aggregates.STLPartylistDashboard;
using webapi.App.Model.User;
using System.Security.Claims;
using Comm.Commons.Advance;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using webapi.App.Aggregates.Common.Dto;
using System.Text;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.Common;
using webapi.App.STLDashboardModel;

namespace webapi.Controllers.STLPartylistMembership.Features
{
    [Route("app/v1/stldashboard")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class STLMembershipController:ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly ISTLMembershipRepository _repo;
        private readonly IAccountRepository _loginrepo;
        public STLMembershipController(IConfiguration config,  ISTLMembershipRepository repo, IAccountRepository loginrepo)
        {
            _config = config;
            _repo = repo;
            _loginrepo = loginrepo;
        }
        [HttpPost]
        [Route("accountmenu")]
        public async Task<IActionResult> Task0a()
        {
            var result = await _repo.LoadAccountAccess();
            if (result.result == Results.Success)
                return Ok(result.account);
            return NotFound();
        }
        [HttpPost]
        [Route("membership/new")]
        public async Task<IActionResult> Task0b([FromBody] STLMembership request)
        {
            var reporesult = await _repo.MembershipAsync(request);
            if (reporesult.result == Results.Success)
                return Ok(new { result = reporesult.result, Message = reporesult.message });
            else if (reporesult.result == Results.Failed)
                return Ok(new { Status = "error", Message = reporesult.message });
            return NotFound();
        }

        [HttpPost]
        [Route("membership/decd")]
        public async Task<IActionResult> Task0b1([FromBody] DOD req)
        {
            var reporesult = await _repo.ResidenceDODAsyn(req);
            if (reporesult.result == Results.Success)
                return Ok(new { Status = "ok", Message = reporesult.message, Content=req });
            else if (reporesult.result == Results.Failed)
                return Ok(new { Status = "error", Message = reporesult.message });
            return NotFound();
        }

        [HttpPost]
        [Route("registration/new")]
        public async Task<IActionResult> Task0c([FromBody] STLMembership request)
        {
            //var valresult = await validity(request);
            //if (valresult.result == Results.Failed)
            //    return Ok(new { Status = "error", Message = valresult.message });
            //if (valresult.result != Results.Success)
            //    return NotFound();
            var valresult = await validitysignatue(request);
            if (valresult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valresult.message });
            if (valresult.result != Results.Success)
                return NotFound();

            var reporesult = await _repo.MembershipAsync(request, true);
            if (reporesult.result == Results.Success)
                return Ok(new { result = reporesult.result, Message = reporesult.message, User_ID = reporesult.UserID ,AcctID = reporesult.AcctID });
            else if (reporesult.result == Results.Failed)
                return Ok(new { result = reporesult.result, Message = reporesult.message });
            return NotFound();
        }

        [HttpPost]
        [Route("registration/edit")]
        public async Task<IActionResult> Task0c1([FromBody] STLMembership request)
        {
            //var valresult = await validity(request);
            //if (valresult.result == Results.Failed)
            //    return Ok(new { Status = "error", Message = valresult.message });
            //if (valresult.result != Results.Success)
            //    return NotFound();

            var valresult = await validitysignatue(request);
            if (valresult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valresult.message });
            if (valresult.result != Results.Success)
                return NotFound();

            var reporesult = await _repo.MembershipAsync(request,true);
            if (reporesult.result == Results.Success)
                return Ok(new { result = reporesult.result, Message = reporesult.message, User_ID = reporesult.UserID, AcctID = reporesult.AcctID });
            else if (reporesult.result == Results.Failed)
                return Ok(new { result = reporesult.result, Message = reporesult.message });
            return NotFound();
        }

        [HttpPost]
        [Route("account/access")]
        public async Task<IActionResult> Task0d(string userid)
        {
            var result = await _repo.LoadUserAccess(userid);
            if (result.result == Results.Success)
                return Ok(result.access);
            return NotFound();
        }

        [HttpPost]
        [Route("account/skin")]
        public async Task<IActionResult> Task0d01(string skin)
        {
            var result = await _repo.AssigendSkin(skin);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message=result.message});
            return NotFound();
        }
        [HttpPost]
        [Route("account/assignedaccess")]
        public async Task<IActionResult> Task0e(string userid, string menu)
        {
            var result = await _repo.AssignedAccess(userid, menu);
            if (result.result == Results.Success)
                return Ok(new { result=result.result,message=result.message});
            return NotFound();
        }

        [HttpPost]
        [Route("memberaccount")]
        public async Task<IActionResult> Task0f(string search)
        {
            var result = await _repo.LoadAccount(search);
            if (result.result == Results.Success)
                return Ok(result.account);
            return NotFound();
        }

        [HttpPost]
        [Route("householdaccount")]
        public async Task<IActionResult> Task01(string search)
        {
            var result = await _repo.LoadAccountSearch(search);
            if (result.result == Results.Success)
                return Ok(result.account);
            return NotFound();
        }
        [HttpPost]
        [Route("masterlist")]
        public async Task<IActionResult> Task0g()
        {
            var result = await _repo.LoadMasterList();
            if (result.result == Results.Success)
                return Ok(result.account);
            return NotFound();
        }
        [HttpPost]
        [Route("masterlist1")]
        public async Task<IActionResult> Task0g1([FromBody] MasterListRequest request) 
        {
            var result = await _repo.LoadMasterList1(request);
            if (result.result == Results.Success)
                return Ok(result.account);
            return NotFound();
        }



        [HttpPost]
        [Route("totalregister")]
        public async Task<IActionResult> Task0g2()
        {
            var result = await _repo.TotalREgister();
            if (result.result == Results.Success)
                return Ok(result.account);
            return NotFound();
        }

        [HttpPost]
        [Route("parent")]
        public async Task<IActionResult> Task0g3([FromBody] FilterRequest request)
        {
            var result = await _repo.Load_Parent(request);
            if (result.result == Results.Success)
                return Ok(result.parent);
            return NotFound();
        }
        [HttpPost]
        [Route("account/validity")]
        public async Task<IActionResult> Task0h([FromBody] ValidityAccount request)
        {
            var result = await _repo.UpdateValidityAccount(request);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("changeaccountype")]
        public async Task<IActionResult> Task0i([FromBody] ChangeAccountType request)
        {
            var result = await _repo.ChangeAccounttype(request);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if(result.result!=Results.Null)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("children")]
        public async Task<IActionResult> Task0j(string userid)
        {
            var result = await _repo.Load_Children(userid);
            if (result.result == Results.Success)
                return Ok(result.children);
            return NotFound();
        }
        [HttpPost]
        [Route("educatt")]
        public async Task<IActionResult> Task0k(string plid, string pgrpid, string userid)
        {
            var result = await _repo.Load_EducationalAttainment(plid, pgrpid, userid);
            if (result.result == Results.Success)
                return Ok(result.educatt);
            return NotFound();
        }
        [HttpPost]
        [Route("emphist")]
        public async Task<IActionResult> Task0l(string plid, string pgrpid, string userid)
        {
            var result = await _repo.Load_EmploymentHistory(plid, pgrpid, userid);
            if (result.result == Results.Success)
                return Ok(result.emphist);
            return NotFound();
        }
        [HttpPost]
        [Route("residence/organization")]
        public async Task<IActionResult> Task0m(string plid, string pgrpid, string userid)
        {
            var result = await _repo.Load_Organization(plid, pgrpid, userid);
            if (result.result == Results.Success)
                return Ok(result.orgz);
            return NotFound();
        }
        [HttpPost]
        [Route("profilepicture/history")]
        public async Task<IActionResult> Task0n(string plid, string pgrpid, string userid)
        {
            var result = await _repo.Load_ProfilePictureHistory(plid, pgrpid, userid);
            if (result.result == Results.Success)
                return Ok(result.prfpic);
            return NotFound();
        }
        [HttpPost]
        [Route("proffesion")]
        public async Task<IActionResult> Task0o(string search)
        {
            var result = await _repo.Load_Profession(search);
            if (result.result == Results.Success)
                return Ok(result.prof);
            return NotFound();
        }
        [HttpPost]
        [Route("occupation")]
        public async Task<IActionResult> Task0p(string search)
        {
            var result = await _repo.Load_Occupation(search);
            if (result.result == Results.Success)
                return Ok(result.occ);
            return NotFound();
        }
        [HttpPost]
        [Route("skills")]
        public async Task<IActionResult> Task0q(string search)
        {
            var result = await _repo.Load_Skills(search);
            if (result.result == Results.Success)
                return Ok(result.skl);
            return NotFound();
        }

        //validitysignature
        private async Task<(Results result, string message)> validitysignatue(STLMembership request)
        {
            if (request == null)
                return (Results.Null, null);
            if (!request.SignatureURL.IsEmpty())
                return (Results.Success, null);
            if (request.Signature.IsEmpty())
                return (Results.Failed, "Please select an Signature.");
            byte[] bytes = Convert.FromBase64String(request.Signature.Str());
            if (bytes.Length == 0)
                return (Results.Failed, "Make sure selected image is invalid.");
            var res = await ImgService.SendAsync(bytes);
            bytes.Clear();
            if (res == null)
                return (Results.Failed, "Please contact to admin.");
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
            if (json["status"].Str() != "error")
            {
                request.SignatureURL = json["url"].Str();
                return (Results.Success, null);
            }
            return (Results.Null, "Make sure selected image is invalid");
        }

        private async Task<(Results result, string message)> validity(STLMembership request)
        {
            if (request == null)
                return (Results.Null, null);
            if (!request.ImageUrl.IsEmpty())
                return (Results.Success, null);
            if (request.Img.IsEmpty())
                return (Results.Failed, "Please select an image.");
            byte[] bytes = Convert.FromBase64String(request.Img.Str());
            if (bytes.Length == 0)
                return (Results.Failed, "Make sure selected image is invalid.");
            var res = await ImgService.SendAsync(bytes);
            bytes.Clear();
            if (res == null)
                return (Results.Failed, "Please contact to admin.");
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
            if (json["status"].Str() != "error")
            {
                request.ImageUrl = json["url"].Str();
                return (Results.Success, null);
            }
            return (Results.Null, "Make sure selected image is invalid");
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
                    USR_ID=user.USR_ID,
                    ACT_ID=user.ACT_ID,
                    MOB_NO=user.MOB_NO,
                    ACT_TYP=user.ACT_TYP,
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
    }
}
