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

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(EstablishmentRepository))]
    public interface IEstablishmentRepository
    {
        Task<(Results result, String message, string estid)> EstablishmentAsync(Establishment req, bool isUpdate = false);
        Task<(Results result, object est)> Load_Establishment(Establishment_Request req);
    }
    public class EstablishmentRepository : IEstablishmentRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public EstablishmentRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message, string estid)> EstablishmentAsync(Establishment req, bool isUpdate = false)
        {
            var results = _repo.DSpQueryMultiple((!isUpdate) ? "dbo.spfn_BIMSEST0A": "dbo.spfn_BIMSEST0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmestid", (isUpdate ? req.Est_ID : "") },
                {"parmestname", req.Est_Name },
                {"parmesttype",req.Est_Type },
                {"parmcontactdetails",req.ContactDetails },
                {"parmaddress",req.Address },
                {"parmemailaddress",req.EmailAddress },
                {"parmoptrid",account.USR_ID }
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully saved!", row["ESTID"].Str());
                else if (ResultCode == "2")
                    return (Results.Failed, "Organization already Registered!", null);
                else if (ResultCode == "0")
                    return (Results.Failed, "Check your Data, Please try again!", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, object est)> Load_Establishment(Establishment_Request req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSEST0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID},
                {"parmrownum",req.num_row},
                {"parmesttype",req.type},
                {"parmsrch", req.search }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllEstablishmentList(result.Read<dynamic>()));
            return (Results.Null, null);
        }
    }
}
