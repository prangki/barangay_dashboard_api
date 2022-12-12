
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using Comm.Commons.Extensions;
using webapi.App.Features.UserFeature;
using webapi.App.STLDashboardModel;
using webapi.Commons.AutoRegister;
using Microsoft.AspNetCore.Mvc;


namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(PositionRepository))]
    public interface IPositionRepository
    {
        Task<(Results result, string message)> AddPosition(Position position);
        Task<(Results result, object message)> DeletePosition(int plid);
        Task<(Results result, string message)> UpdatePosition(Position position);
        Task<(Results result, object position)> LoadPosition();

    }

    public class PositionRepository : IPositionRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;

        public STLAccount account { get { return _identity.AccountIdentity(); } }

        public PositionRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message)> AddPosition(Position position)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_POSITIONINSERT", new Dictionary<string, object>()
            {
                //{ "json", jsonString },
                //{"paramid", position.PositionId },
                {"paramplid", account.PL_ID},
                {"parampgrpid", account.PGRP_ID},
                {"paramlocbrgy", account.LOC_BRGY},
                {"paramposition", position.Positionn },
                {"paramcategory", position.Category},
                {"paramuserid", account.USR_ID }


            }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully saved");
                else if (ResultCode == "2")
                    return (Results.Failed, "Already Exists");

            }
            return (Results.Null, null);

        }

        public async Task<(Results result, object message)> DeletePosition(int plid) // refer to IAppointedMemberRepository
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_POSITIONDELETE", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid", account.PGRP_ID},
                {"paramid", plid},

            });

            if (result != null)
                return (Results.Success, result);
            return (Results.Failed, null);
        }

        public async Task<(Results result, string message)> UpdatePosition(Position position)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_POSITIONUPDATE", new Dictionary<string, object>()
            {
                {"paramid", position.PositionId },
                {"paramplid", account.PL_ID},
                {"parampgrpid", account.PGRP_ID},
                {"paramlocbrgy", account.LOC_BRGY},
                {"paramposition", position.Positionn},
                {"paramcategory", position.Category},
                {"paramuserid", account.USR_ID}
            }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully updated");
                else if (ResultCode == "2")
                    return (Results.Failed, "Already Exists");

            }
            return (Results.Null, null);

        }

        public async Task<(Results result, object position)> LoadPosition()
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_POSITIONSHOW", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid", account.PGRP_ID},

            }).FirstOrDefault();
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }
    }

   
}
