using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using webapi.App.RequestModel.AppRecruiter;
using Comm.Commons.Extensions;
using webapi.Commons.AutoRegister;
using webapi.App.Features.UserFeature;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(APKRepository))]
    public interface IAPKRepository
    {
        Task<(Results result, String message, String TRNNo)> APKAsync(APK request, bool isUpdate=false);
        Task<(Results result, String message)> SetPrimaryUpdate(APK request);
        Task<(Results result, object apk)> LoadAPK();
    }
    public class APKRepository : IAPKRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public APKRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message, String TRNNo)> APKAsync(APK request, bool isUpdate = false)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_APK0A", new Dictionary<string, object>()
            {
                {"parmplid",request.PL_ID },
                {"parmpgrpid",request.PGRP_ID },
                {"parmoptrid",account.USR_ID },
                {"parmapkverno",request.APKVerno },
                {"parmapknm",request.APKName },
                {"parmapkpath",request.APKPath },
                {"parmapkpathcba",request.APKPathCBA },
                {"parmapktrn",request.TRNNo }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully saved!", row["APK_TRN"].Str());
                else if (ResultCode == "0")
                    return (Results.Failed, "APK Version Already Exist, Please try again!",null);
                else if (ResultCode == "2")
                    return (Results.Failed, "Invalid Apk Version, Please try again!",null);
            }
            return (Results.Null, null,null);
        }

        public async Task<(Results result, object apk)> LoadAPK()
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_APK0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
            });
            if (result != null)
                return (Results.Success, SubscriberDto.GetAPKList(result));
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> SetPrimaryUpdate(APK request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_APK0C", new Dictionary<string, object>()
            {
                {"parmplid",request.PL_ID },
                {"parmpgrpid",request.PGRP_ID },
                {"parmoptrid",account.USR_ID },
                {"parmapktrn",request.TRNNo }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    var description = row["NOTIF_DESC"].Str();
                    await ComapantySettings(null, new { Update = description }, request.PL_ID, request.PGRP_ID);
                    return (Results.Success, "Successfully set this for Update!");
                }
                else if (ResultCode == "0")
                    return (Results.Failed, "Invalid APK Version, Please try again!");
                else if (ResultCode == "2")
                    return (Results.Failed, "Invalid APK Version, Please try again!");
            }
            return (Results.Null, null);
        }
        public async Task<bool> ComapantySettings(IDictionary<string, object> row, object content, string plid, string pgrpid)
        {
            //var settings = STLSubscriberDto.GetGroup(row);
            //var notifications = SubscriberDto.EventNofitication(row);
            await Pusher.PushAsync($"/{plid}/{pgrpid}/notify"
                , new { type = "app-update", content = content });
            return false;
        }
    }
}
