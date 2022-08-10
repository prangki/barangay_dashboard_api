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
        Task<(Results result, object org)> Load_Organization();
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

        public async Task<(Results result, object org)> Load_Organization()
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_ORGZ0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllOrganizationList(result.Read<dynamic>()));
            return (Results.Null, null);
        }
    }
}
