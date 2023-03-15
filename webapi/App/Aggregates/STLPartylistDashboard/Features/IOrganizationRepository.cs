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
    [Service.ITransient(typeof(OrganizationRepository))]
    public interface IOrganizationRepository
    {
        Task<(Results result, object org)> Load_Organization(string plid, string pgrpid, string organization);
        Task<(Results result, string message, string orgid)> OrganizationAsync(Organization req, bool isUpdate = false);
    }
    public class OrganizationRepository:IOrganizationRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public OrganizationRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object org)> Load_Organization(string plid, string pgrpid, string organization)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_ORGZ0C", new Dictionary<string, object>()
            {
                {"parmplid",plid },
                {"parmpgrpid",pgrpid},
                {"parmsrch", organization }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllOrganizationList(result.Read<dynamic>()));
            return (Results.Null, null);
        }

        public async Task<(Results result, string message, string orgid)> OrganizationAsync(Organization req, bool isUpdate = false)
        {
            var results = _repo.DSpQueryMultiple((isUpdate ? "dbo.spfn_ORGZ0B" : "dbo.spfn_ORGZ0A"), new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmorgzid", (isUpdate ? req.OrganizationID : "") },
                {"parmorgnm", req.OrganizationNM },
                {"parmorgabbr",req.OrganizatioAbbr },
                {"parmorgest",req.Estabalished },
                {"parmoptrid",account.USR_ID }
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully saved!", row["ORG_ID"].Str());
                else if (ResultCode == "2")
                    return (Results.Failed, "Organization already Registered!", null);
                else if (ResultCode == "0")
                    return (Results.Failed, "Check your Data, Please try again!", null);
            }
            return (Results.Null, null, null);
        }
    }
}
