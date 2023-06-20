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
using webapi.App.Aggregates.Common.Dto;
using webapi.App.STLDashboardModel;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(EmergencyAlertRepository))]
    public interface IEmergencyAlertRepository
    {
        Task<(Results result, object emgyalert)> Load_EmergencyAlert(FilterRequest req);
        Task<(Results result, object emgyalert)> Load_EmergencyAlertID(FilterRequest req);
        Task<(Results result, object emgyalert)> Load_EmergencyAlertRequest(FilterRequest req);
        Task<(Results result, String message)> ClosedEmergencyAlertAsync(EmergencyAlert req);
        Task<(Results result, String total_alert)> TotaldEmergencyAlertAsync(FilterRequest req);
        Task<(Results result, object profile)> LoadEmergencyPofile(string userId);
    }
    public class EmergencyAlertRepository : IEmergencyAlertRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public EmergencyAlertRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object emgyalert)> Load_EmergencyAlert(FilterRequest req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSEMGY0B02", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmemergencytypeid",req.EmgyTypID },
                {"parmrownum",req.num_row },
                {"parmsrch",req.Search }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllEmergencyAlertList(result.Read<dynamic>(), req.Userid, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, object emgyalert)> Load_EmergencyAlertID(FilterRequest req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSEMGY0B02C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmemergencyid",req.EmgyID }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllEmergencyAlertList(result.Read<dynamic>(), req.Userid, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, object emgyalert)> Load_EmergencyAlertRequest(FilterRequest req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSEMGY0B02A", new Dictionary<string, object>()
            {
                {"parmplid",req.PL_ID },
                {"parmpgrpid",req.PGRP_ID },
                {"parmrownum",req.num_row },
                {"parmrequeststatus",req.Status }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllEmergencyAlertRequestList(result.Read<dynamic>(), req.Userid, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> ClosedEmergencyAlertAsync(EmergencyAlert req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIMSEMGY0B03", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmemergencyid", req.EmgyID },
                {"parmemergencytypeid",req.EmgyTypID },
                {"parmuserid",req.UserID },
                {"parmcloseddetails",req.Closed_Details },
                {"parmclosedtype",req.Closed_Type },
                {"parmoptrid",account.USR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    req.ClosedDate = row["CLOSED_DATE"].Str();
                    if(req.Closed_Type == 1)
                        req.Closed_TypeName = "Closed";
                    else if (req.Closed_Type == 2)
                        req.Closed_TypeName = "False Alarm";
                    return (Results.Success, "Successfully saved!");
                }
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Emergency Alert Details, Please try again!");
                return (Results.Failed, "Something wrong in your data, Please try again!");
            }
            return (Results.Null, null);
        }
        public async Task<(Results result, string total_alert)> TotaldEmergencyAlertAsync(FilterRequest req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIMSEMGY0B02B", new Dictionary<string, object>()
            {
                {"parmplid",req.PL_ID },
                {"parmpgrpid",req.PGRP_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    return (Results.Success, row["TTL_ALERT"].Str());
                }
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object profile)> LoadEmergencyPofile(string userId)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BIMSEMGYPRF", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmusrid",userId}
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }
    }
}
