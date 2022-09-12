using Infrastructure.Repositories;
using System.IO;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using webapi.App.STLDashboardModel;
using webapi.Commons.AutoRegister;
using Dapper;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System;
using Comm.Commons.Extensions;
using Newtonsoft.Json;
using Dapper;
using System.Linq;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(AppointedMemberRepository))]
    public interface IAppointedMemberRepository
    {
        Task<(Results result, string message)> SaveAppointedMember(string jsonString);
        Task<(Results result, object appoint)> LoadAppointMember();
    }

    public class AppointedMemberRepository : IAppointedMemberRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }

        public AppointedMemberRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message)> SaveAppointedMember(string jsonString)
        {

            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYAPPNTDMMBRS", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmjson", jsonString}
                }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, "Already exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object appoint)> LoadAppointMember()
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYAPPNTDMMBRS", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid", account.PGRP_ID}
            });

            if (result != null)
                return (Results.Success, result);
            return (Results.Failed, null);
        }
    }
}
