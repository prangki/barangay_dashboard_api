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
    [Service.ITransient(typeof(RequestCedulaRepository))]
    public interface IRequestCedulaRepository
    {
        Task<(Results result, string message)> Save(BrgyCedula param);
        Task<(Results result, object list)> Load(string filterId, string from, string to);
        Task<(Results result, string message)> Release(string ctcno, string releasedDate, string releasedBy);
        Task<(Results result, string message)> Cancel(string ctcno, string canceledDate, string canceledBy);
    }

    public class RequestCedulaRepository : IRequestCedulaRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }

        public RequestCedulaRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message)> Save(BrgyCedula param)
        {

            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYRQCTCBDB", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmreqby", param.RequestBy},
                    {"parmbdt", param.Birthdate},
                    {"parmgndr", param.Gender},
                    {"parmaddr", param.Address},
                    {"parmctznsp", param.Citizenship},
                    {"parmcvlst", param.CivilStatus},
                    {"parmbct", param.BCT},
                    {"parmgeb", param.GEB},
                    {"parmgebtx", param.GEBTax},
                    {"parmgep", param.GEP},
                    {"parmgeptx", param.GEPTax},
                    {"parmirp", param.IRP},
                    {"parmirptx", param.IRPTax},
                    {"parmttltx", param.TotalTax},
                    {"parmirt", param.InterestRate},
                    {"parmintrst", param.Interest},
                    {"parmttlamtpd", param.AmountPaid},
                    {"parmprcsby", param.ProcessBy},
                    {"parmprcsdt", param.ProcessDate}
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

        public async Task<(Results result, object list)> Load(string filterId, string from, string to)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYRQCTC00", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid", account.PGRP_ID},
                {"parmfltr", int.Parse(filterId)},
                {"parmfrom", from},
                {"parmto", to}
            });

            if (result != null)
                return (Results.Success, result);
            return (Results.Failed, null);
        }

        public async Task<(Results result, string message)> Release(string ctcno, string releasedDate, string releasedBy)
        {

            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYRQCTC01", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmctcno", ctcno},
                    {"parmdtrls", releasedDate},
                    {"parmoprtr", releasedBy}
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

        public async Task<(Results result, string message)> Cancel(string ctcno, string canceledDate, string canceledBy)
        {

            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYRQCTC01", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmctcno", ctcno},
                    {"parmdtcncl", canceledDate},
                    {"parmoprtr", canceledBy}
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
    }
}
