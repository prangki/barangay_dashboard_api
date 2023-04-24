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
    [Service.ITransient(typeof(EmergencyTypeRepository))]
    public interface IEmergencyTypeRepository
    {
        Task<(Results result, String message)> EmergencyTypeAsync(Emergency_Type req, bool isEdit = false);
        Task<(Results result, object emgyp)> Load_EmergencyType(FilterRequest req);
        Task<(Results result, String message)> ChangeStatusAsync(Emergency_Type req);
    }
    public class EmergencyTypeRepository : IEmergencyTypeRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public EmergencyTypeRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message)> EmergencyTypeAsync(Emergency_Type req, bool isEdit = false)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIMSEMGY0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmemergencytypeid",(isEdit) ? req.EmgyTypID: "" },
                {"parmemergencytype",req.EmgyType },
                {"parmmessage",req.Message },
                {"parmoptrid",account.USR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    if (isEdit)
                        req.EmgyTypID = row["EMGY_TYP_ID"].Str();
                    return(Results.Success, "Successfully saved!");
                }
                else if (ResultCode == "2")
                    return (Results.Failed, "Emergency Type already Exist, Please try again!");
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Emergency Type Details, Please try again!");
                return (Results.Failed, "Something wrong in your data, Please try again!");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object emgyp)> Load_EmergencyType(FilterRequest req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSEMGY0A02", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmrownum",req.num_row },
                {"parmsrch",req.Search }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllEmergencyTpeList(result.Read<dynamic>(), req.Userid, 100));
            return (Results.Null, null);

        }
        public async Task<(Results result, string message)> ChangeStatusAsync(Emergency_Type req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIMSEMGY0A01", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmemergencytypeid",req.EmgyTypID },
                {"parmoptrid",account.USR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully Change Status!");
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Emergency Type Details, Please try again!");
                return (Results.Failed, "Something wrong in your data, Please try again!");
            }
            return (Results.Null, null);
        }
    }
}
