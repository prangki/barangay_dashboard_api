﻿using Microsoft.AspNetCore.Http;
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
    public class HouseNoController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IHouseNoRepository _repo;
        private readonly IAccountRepository _loginrepo;

        public HouseNoController(IConfiguration config, IHouseNoRepository repo, IAccountRepository loginrepo)
        {
            _repo = repo;
            _config = config;
            _loginrepo = loginrepo;
        }

        [HttpPost]
        [Route("house/persitio")]
        public async Task<IActionResult> LoadHousesPerSitio(string code)
        {
            var result = await _repo.GetNumOfHousesPerSitio(code);
            if (result.result == Results.Success)
                return Ok(result.hsspersit);
            return NotFound();
        }

        //[HttpPost]
        //[Route("house/households")]
        //public async Task<IActionResult> LoadHouseholds()
        //{
        //    var result = await _repo.LoadHouseholds();
        //    if (result.result == Results.Success)
        //        return Ok(result.households);
        //    return NotFound();
        //}

        [HttpPost]
        [Route("house/houseno/save")]
        public async Task<IActionResult> SaveHouseNo([FromBody]HouseDetails details)
        {
            var result = await _repo.SaveHouseNo(details);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("house/houseno/load")]
        public async Task<IActionResult> LoadHouses()
        {
            var result = await _repo.LoadHouses();
            if (result.result == Results.Success)
                return Ok(result.houses);
            return NotFound();
        }

        [HttpPost]
        [Route("residents")]
        public async Task<IActionResult> LoadResidents()
        {
            var result = await _repo.LoadResidents();
            if (result.result == Results.Success)
                return Ok(result.residents);
            return NotFound();
        }

        [HttpPost]
        [Route("house/household/save")]
        public async Task<IActionResult> SaveHousehold([FromBody] HouseDetails details)
        {
            var result = await _repo.SaveHousehold(details);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("house/household/load")]
        public async Task<IActionResult> LoadHouseholds()
        {
            var result = await _repo.LoadHouseholds();
            if (result.result == Results.Success)
                return Ok(result.households);
            return NotFound();
        }

        [HttpPost]
        [Route("house/household/loadby")]
        public async Task<IActionResult> LoadSpecificHouseholds(string houseid)
        {
            var result = await _repo.LoadSpecificHouseholds(houseid);
            if (result.result == Results.Success)
                return Ok(result.households);
            return NotFound();
        }

        [HttpPost]
        [Route("house/family/save")]
        public async Task<IActionResult> SaveFamilyMember([FromBody] HouseDetails details)
        {
            var result = await _repo.SaveFamilyMember(details);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("house/family/load")]
        public async Task<IActionResult> LoadFamilyMembers()
        {
            var result = await _repo.LoadFamilyMembers();
            if (result.result == Results.Success)
                return Ok(result.family);
            return NotFound();
        }

        [HttpPost]
        [Route("house/family/loadby")]
        public async Task<IActionResult> LoadSpecificFamilyMembers(string householdid)
        {
            var result = await _repo.LoadSpecificFamilyMembers(householdid);
            if (result.result == Results.Success)
                return Ok(result.family);
            return NotFound();
        }

        [HttpPost]
        [Route("house/info")]
        public async Task<IActionResult> LoadHouseInfo(string houseno,string householdid)
        {
            var result = await _repo.LoadHouseInfo(houseno,householdid);
            if (result.result == Results.Success)
                return Ok(result.houseinfo);
            return NotFound();
        }

        [HttpPost]
        [Route("house/houseno/remove")]
        public async Task<IActionResult> RemoveHouseNo(string houseno)
        {
            var result = await _repo.RemoveHouseNo(houseno);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("house/household/remove")]
        public async Task<IActionResult> RemoveHousehold(string householdid)
        {
            var result = await _repo.RemoveHousehold(householdid);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("house/family/remove")]
        public async Task<IActionResult> RemoveFamilyMember(string famId,string memId)
        {
            var result = await _repo.RemoveFamilyMember(famId, memId);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("house/houseno/update")]
        public async Task<IActionResult> UpdateHouseNo([FromBody] HouseDetails details)
        {
            var result = await _repo.UpdateHouseNo(details);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("house/household/update")]
        public async Task<IActionResult> UpdateHousehold([FromBody] HouseDetails details)
        {
            var result = await _repo.UpdateHousehold(details);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("house/family/update")]
        public async Task<IActionResult> UpdateFamily([FromBody] HouseDetails details)
        {
            var result = await _repo.UpdateFamily(details);
            if (result.result == Results.Success)
                return Ok(new { result = result.result, message = result.message });
            else if (result.result == Results.Failed)
                return Ok(new { result = result.result, message = result.message });
            return NotFound();
        }

    }
}