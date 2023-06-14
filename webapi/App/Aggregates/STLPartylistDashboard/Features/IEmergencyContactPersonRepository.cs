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
    [Service.ITransient(typeof(EmergencyContactPersonRepository))]
    public interface IEmergencyContactPersonRepository
    {
        Task<(Results result, String message)> EmergencyContactPersonAsync(Emergency_Contact_Person req, bool isEdit = false);
        Task<(Results result, object emgycp)> Load_EmergencyContactPerson(FilterRequest req);
        Task<(Results result, String message)> ChangeStatusAsync(Emergency_Contact_Person req);
    }
    public class EmergencyContactPersonRepository:IEmergencyContactPersonRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public EmergencyContactPersonRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message)> EmergencyContactPersonAsync(Emergency_Contact_Person req, bool isEdit = false)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIMSEMGY0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmemergencycontactid", (isEdit) ? req.EmgyContactID: "" },
                {"parmemergencytypeid",req.EmgyTypID },
                {"parmemergencyconctacperson",req.ContactPerson },
                {"parmmobilenumber",req.MobileNumber },
                {"parmoptrid",account.USR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    if (!isEdit)
                        req.EmgyContactID = row["EMGY_CNTCT_ID"].Str();
                    return (Results.Success, "Successfully saved!");
                }
                else if (ResultCode == "2")
                    return (Results.Failed, "Emergency Contact Person already Exist, Please try again!");
                else if (ResultCode == "3")
                    return (Results.Failed, "Invalid Mobile Number, Please try again!");
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Emergency Type Details, Please try again!");
                return (Results.Failed, "Something wrong in your data, Please try again!");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object emgycp)> Load_EmergencyContactPerson(FilterRequest req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSEMGY0C02", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmemergencytypeid",req.EmgyTypID },
                {"parmrownum",req.num_row },
                {"parmsrch",req.Search }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllEmergencyContactPersonList(result.Read<dynamic>(), req.Userid, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> ChangeStatusAsync(Emergency_Contact_Person req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIMSEMGY0C01", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmemergencycontactid", req.EmgyTypID },
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
                    return (Results.Failed, "Check Emergency Contact Person Details, Please try again!");
                return (Results.Failed, "Something wrong in your data, Please try again!");
            }
            return (Results.Null, null);
        }
    }
}
