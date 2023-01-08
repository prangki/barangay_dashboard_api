
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using Comm.Commons.Extensions;
using webapi.App.Features.UserFeature;
using webapi.App.STLDashboardModel;
using webapi.Commons.AutoRegister;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;


namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(ReligionRepository))]
    public interface IReligionRepository
    {
        Task<(Results result, string message)> AddReligion(Religion religion);
        Task<(Results result, object message)> DeleteReligion(string code);
        Task<(Results result, string message)> UpdateReligion(Religion religion);
        Task<(Results result, object position)> LoadReligion(string plid);
    }

    public class ReligionRepository : IReligionRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;

        public STLAccount account { get { return _identity.AccountIdentity(); } }

        public ReligionRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message)> AddReligion(Religion religion)
        {
            var code = religion.Description.Substring(religion.Description.Length - 3);
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_RELIGIONINSERT", new Dictionary<string, object>()
            {
                
                //{ "json", jsonString },
                {"paramcode", code},
                {"paramplid",  account.PL_ID},
                {"parampgrpid", account.PGRP_ID},
                {"paramdescription", religion.Description},
                {"paramuserid", account.USR_ID},


            }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully saved");
                else if (ResultCode == "2")
                    return (Results.Failed, "Already Exists");

            }
            return (Results.Null, null);

        }

        public async Task<(Results result, object message)> DeleteReligion(string description)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_RELIGIONDELETE", new Dictionary<string, object>()
            {
                {"paramplid", account.PL_ID},
                {"parampgrpid", account.PGRP_ID},
                {"paramdescription", description},
                
            });

            if (result != null)
                return (Results.Success, result);
            return (Results.Failed, null);
        }

        public async Task<(Results result, string message)> UpdateReligion(Religion religion)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_RELIGIONUPDATE", new Dictionary<string, object>()
            {
                {"paramcode", religion.Code },
                {"paramdescription", religion.Description},
                {"paramplid", account.PL_ID},
                {"parampgrpid", account.PGRP_ID},
                {"paramuserid", account.USR_ID},
            }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully updated");
                else if (ResultCode == "2")
                    return (Results.Failed, "Already Exists");

            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object position)> LoadReligion(string plid)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_RELIGIONSHOW", new Dictionary<string, object>()
            {

                {"paramplid", account.PL_ID},
                {"parampgrpid", account.PGRP_ID},

            }).FirstOrDefault();
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }
    }

   
}
