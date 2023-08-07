using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using Comm.Commons.Extensions;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using webapi.Commons.AutoRegister;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(PoliticalPositionRepository))]
    public interface IPoliticalPositionRepository
    {
        Task<(Results result, object psn)> LoadPoliticalPosition();
        Task<(Results result, String message)> PoliticalPositionAsyn(string psn, string psncd="", bool isUpdate = false);
        Task<(Results result, object psn)> LoadBarangayPosition();
        Task<(Results result, object position)> SearchBarangayPosition(string userid);
    }
    public class PoliticalPositionRepository : IPoliticalPositionRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public PoliticalPositionRepository (ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }
        public async Task<(Results result, object psn)> LoadPoliticalPosition()
        {
            var result = _repo.DQuery<dynamic>($"dbo.spfn_CBB01");
            if (result != null)
            {
                return (Results.Success, result);
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> PoliticalPositionAsyn(string psn,string psncd="", bool isUpdate = false)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_CBBGBA01", new Dictionary<string, object>()
            {
                {"@parmpsncd",(isUpdate?psncd:"") },
                {"@parmpsnnm",psn },
                {"@parmusrid", account.USR_ID},
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    return (Results.Success, "Successfully save");
                }
                else if (ResultCode == "0")
                {
                    return (Results.Failed, "Already Exist");
                }
                else if (ResultCode == "2")
                {
                    return (Results.Null, null);
                }
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object psn)> LoadBarangayPosition()
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_POSTION0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmbrgy", account.LOC_BRGY }
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object position)> SearchBarangayPosition(string userid)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BIMSSIDPOS", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmusrid", userid }
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }
    }
}
