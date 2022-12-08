using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using Comm.Commons.Extensions;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using Infrastructure.Repositories;
using webapi.App.Model.User;
using webapi.Commons.AutoRegister;
using webapi.App.Features.UserFeature;
using webapi.App.RequestModel.Common;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(GovernmentValidIDRepository))]
    public interface IGovernmentValidIDRepository
    {
        Task<(Results result, object govvalid)> LoadGovermentValidID(FilterRequest request);
        Task<(Results result, object govvalid)> Load_GovermentID(FilterRequest request);
        Task<(Results result, string message, string govid)> GovernmentValidIDAsync(GovernmentValidID request, bool isUpdate = false);
    }
    public class GovernmentValidIDRepository:IGovernmentValidIDRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public GovernmentValidIDRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object govvalid)> LoadGovermentValidID(FilterRequest request)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BDBGOVVALID0A", new Dictionary<string, object>()
            {
                {"parmplid",request.PL_ID },
                {"parmpgrpid",request.PGRP_ID },
                {"parmuserid", request.Userid }
            });
            if (result != null)
                return (Results.Success, SubscriberDto.GetAllGovenmentValidIDList(result));
            return (Results.Null, null);
        }

        public async Task<(Results result, object govvalid)> Load_GovermentID(FilterRequest request)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_GOVVALID0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmrownum", request.num_row },
                {"parmsearch", request.Search }
            });
            if (result != null)
                return (Results.Success, SubscriberDto.GetAllGovenmentIDList(result));
            return (Results.Null, null);
        }

        public async Task<(Results result, string message, string govid)> GovernmentValidIDAsync(GovernmentValidID request, bool isUpdate=false)
        {
            var result = _repo.DSpQuery<dynamic>(!isUpdate ? $"spfn_GOVVALID0A" : $"spfn_GOVVALID0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmgovernmentid", (!isUpdate ? "" :request.ID) },
                {"parmgovernmentidnm",request.GovernmentID },
                {"parmoptrid",account.USR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save", row["ID"].Str());
                else if (ResultCode == "2")
                    return (Results.Failed, "Government ID already Exist, Please try again", null);
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Government ID Details, Please try again", null);
                return (Results.Failed, "Something wrong in your data, Please try again", null);
            }
            return (Results.Null, null, null);
        }
    }
}
