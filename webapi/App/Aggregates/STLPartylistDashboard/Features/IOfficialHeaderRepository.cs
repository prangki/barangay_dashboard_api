using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using webapi.App.RequestModel.Feature;
using webapi.Commons.AutoRegister;
using webapi.Services.Dependency;

using Comm.Commons.Extensions;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.Common.Dto;
using webapi.App.RequestModel.AppRecruiter;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(OfficialHeaderRepository))]
    public interface IOfficialHeaderRepository
    {
        Task<(Results result, String message)> OfficialHaeaderAsyn(OfficialHeader request);
        Task<(Results result, object ofllogo)> LoadOfficialLogo();
    }
    public class OfficialHeaderRepository :IOfficialHeaderRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public OfficialHeaderRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message)> OfficialHaeaderAsyn(OfficialHeader request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_OFLHEADER0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmcountry",request.Country },
                {"parmprovince",request.Province },
                {"parmmunicipality",request.Municipality },
                {"parmbarangay",request.Barangay },
                {"parmissuedlocation", request.IssuedLocation },
                {"parmoflbrgylogo",request.BrgyOfficialLogo },
                {"parmoflmunicipallogo",request.MunicipalLogo },
                {"parmcontactnumber", request.ContactNo },
                {"parmdefaultvalidity",request.DefaultValidity },
                {"parmmonhtvalidity",request.MonthValidity },
                {"parmoptrid",account.USR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully saved!");
                else if (ResultCode == "0")
                    return (Results.Failed, "Something wrong in your data, Please try again!");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object ofllogo)> LoadOfficialLogo()
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_OFLHEADER0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetOfficialHeaderList(result.Read<dynamic>(), 100));
            return (Results.Null, null);
        }
    }

}
