using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Comm.Commons.Extensions;
using Infrastructure.Repositories;
using webapi.Commons.AutoRegister;
using webapi.App.Aggregates.Common;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.Features.UserFeature;
using webapi.App.RequestModel.Common;
using webapi.App.RequestModel.Feature;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(SMSRespository))]
    public interface ISMSRepository
    {
        Task<(Results result, String message)> SMSINAsync(BIMSSMSIN req);
        Task<(Results result, object resident)> SMSINNotification(FilterRequest req);
        Task<(Results result, String message)> ChatMessageReadAsync(MessengerAppRequest req);
    }
    public class SMSRespository : ISMSRepository
    {
        private readonly IRepository _repo;
        public SMSRespository(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<(Results result, string message)> ChatMessageReadAsync(MessengerAppRequest req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_0BC0001", new Dictionary<string, object>()
            {
                {"parmchatid", req.ChatID},
                {"parmsenderid", req.MemberID}
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    return (Results.Success, "Successfully send!");
                }
                else if (ResultCode == "2")
                    return (Results.Failed, "License was not valid");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> SMSINAsync(BIMSSMSIN req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_SMSIN0A", new Dictionary<string, object>()
            {
                {"parmsmsid", req.SMSID},
                {"parmsendtime", req.SendTime},
                {"parmmessagefrom", req.Messagefrom},
                {"parmmessageto", req.Messageto},
                {"parmmessagetext", req.Messagetext},
                {"parmmessagetype", req.Messagetype},
                {"parmmessageread", req.MessageRead}
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    req.Id = row["Id"].Str();
                    await Pusher.PushAsync($"/smsin", new { type = "smsin", content = req });
                    return (Results.Success, "Successfully send!");
                }
                else if (ResultCode == "2")
                    return (Results.Failed, "License was not valid");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object resident)> SMSINNotification(FilterRequest req)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_SMSIN0A1", new Dictionary<string, object>()
            {
                {"parmplid", req.PL_ID},
                {"parmpgrpid",req.PGRP_ID },
                {"parmrownum", req.num_row },
                {"parmmobileno", (req.MOB_NO == "") ? null : req.MOB_NO },
            });
            if (results != null)
                return (Results.Success, SubscriberDto.GetSMSSenderList(results, 100));
            //return (Results.Success, results);
            return (Results.Null, null);
        }
    }
}
