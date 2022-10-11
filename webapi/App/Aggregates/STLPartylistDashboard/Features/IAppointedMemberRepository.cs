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
        Task<(Results result, string message)> Approve(string appointid);
        Task<(Results result, string message)> ApproveBySelection(string groupid);
        Task<(Results result, string message)> Withdraw(string appointid);
        Task<(Results result, string message)> WithdrawBySelection(string groupid);
        Task<(Results result, string message)> RemoveAppointedMember(string appointid);
        Task<(Results result, string message)> RemoveBySelection(string groupid);
        Task<(Results result, string message)> UpdateDate(string date, bool isStmt = false);
        Task<(Results result, object details)> ViewAppointedMemberDetails(string userid);
        Task<(Results result, object details)> ViewDocumentTags(string id);
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

        public async Task<(Results result, string message)> Approve(string appointid)
        {

            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYAPPNTDMMBRS01", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmappntid", appointid},
                    {"parmsaprv", 1}
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

        public async Task<(Results result, string message)> ApproveBySelection(string groupid)
        {

            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYAPPNTDMMBRS01", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmsaprv", 1},
                    {"parmgrpapntid", groupid}
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

        public async Task<(Results result, string message)> WithdrawBySelection(string groupid)
        {

            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYAPPNTDMMBRS01", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmswdrwn", 1},
                    {"@parmgrpapntid", groupid}
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

        public async Task<(Results result, string message)> Withdraw(string appointid)
        {

            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYAPPNTDMMBRS01", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmappntid", appointid},
                    {"parmswdrwn", 1}
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

        public async Task<(Results result, string message)> RemoveAppointedMember(string appointid)
        {

            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYAPPNTDMMBRS01", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmappntid", appointid},
                    {"parmsrmv", 1}
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

        public async Task<(Results result, string message)> RemoveBySelection(string groupid)
        {

            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYAPPNTDMMBRS01", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmsrmv", 1},
                    {"parmgrpapntid", groupid},
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

        public async Task<(Results result, string message)> UpdateDate(string date, bool isStmt = false)
        {

            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYAPPNTDMMBRS01", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {(isStmt) ? "parmsapntmtstmt" : "parmsoath", 1},
                    {"parmoathdt", date}
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

        public async Task<(Results result, object details)> ViewAppointedMemberDetails(string userid)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYAPPNTDMMBRS02", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid", account.PGRP_ID},
                {"parmusrid", userid}
            });

            if (result != null)
                return (Results.Success, result);
            return (Results.Failed, null);
        }

        public async Task<(Results result, object details)> ViewDocumentTags(string id)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYAPNTMNTPSTNTC", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid", account.PGRP_ID},
                {"parmdoctyp", id}
            });

            if (result != null)
                return (Results.Success, result);
            return (Results.Failed, null);
        }
    }
}
