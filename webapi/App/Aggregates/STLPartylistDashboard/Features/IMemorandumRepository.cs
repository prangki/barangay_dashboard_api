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
using System.Net.Mail;
using System.Net;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.Common.Dto;
using webapi.App.STLDashboardModel;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(MemorandumRepository))]
    public interface IMemorandumRepository
    {
        Task<(Results result, String message, String memoid)> MemorandumAsync(Memorandum request);
        Task<(Results result, String message)> UpdateMemorandumAsync(Memorandum request);
        Task<(Results result, object memo)> LoadMemorandum(FilterRequest request);
    }
    public class MemorandumRepository:IMemorandumRepository
    {
        private readonly ISubscriber _identity;
        private readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public MemorandumRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message, string memoid)> MemorandumAsync(Memorandum request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_MEMO_0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmbrgy", request.BrgyCode},
                {"parmmemono",request.MemorandumNo },
                {"parmsubject",request.Subject },
                {"parmpath",request.MemorandumURL },
                {"parmuserid",account.USR_ID },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save", row["MEMO_ID"].Str());
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again",null);
                else if (ResultCode == "3")
                    return (Results.Failed, "Memorandum No. already exist, Please try again", null);
            }
            return (Results.Null, null ,null);
        }

        public async Task<(Results result, string message)> UpdateMemorandumAsync(Memorandum request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_MEMO_0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmbrgy", request.BrgyCode},
                {"parmmemoid",request.MemoId },
                {"parmmemono",request.MemorandumNo },
                {"parmsubject",request.Subject },
                {"parmpath",request.MemorandumURL },
                {"parmuserid",account.USR_ID },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again");
                else if (ResultCode == "3")
                    return (Results.Failed, "Memorandum No. already exist, Please try again");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object memo)> LoadMemorandum(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_MEMO0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmrownum", request.num_row},
                {"parmsearch", request.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllMemoList(result.Read<dynamic>(), request.Userid, 100));
            return (Results.Null, null);
        }
    }
}
