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
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid", request.Userid }
            });
            if (result != null)
                return (Results.Success, SubscriberDto.GetAllGovenmentValidIDList(result));
            return (Results.Null, null);
        }
    }
}
