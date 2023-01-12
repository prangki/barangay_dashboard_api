using Infrastructure.Repositories;
using System.IO;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using webapi.App.STLDashboardModel;
using webapi.Commons.AutoRegister;
using Dapper;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System;
using Comm.Commons.Extensions;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(FingerprintScannerRepository))]
    public interface IFingerprintScannerRepository
    {
        Task<(Results result, string message)> SaveFingerprints(Fingerprint info);
        Task<(Results result, string message)> UpdateFingerprints(Fingerprint info);
        Task<(Results result, object fingerprints)> LoadFingerprints(string userId);
    }

    public class FingerprintScannerRepository : IFingerprintScannerRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }

        public FingerprintScannerRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message)> SaveFingerprints(Fingerprint info)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYFINGERPRINTCR", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID},
                {"parmpgrpid",account.PGRP_ID},
                {"parmusrid",info.UserId},
                {"parmjson",info.Json},
                {"parmrgsdt",info.RegisterDate},
                {"parmrgstm",info.RegisterTime},
                {"parmrgsts",info.RegisterTimeStamp},
                {"parmoprtr",info.Operator}
            }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, "Already Exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);

        }

        public async Task<(Results result, string message)> UpdateFingerprints(Fingerprint info)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYFINGERPRINTU", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID},
                {"parmpgrpid",account.PGRP_ID},
                {"parmusrid",info.UserId},
                {"parmjson",info.Json},
                {"parmupddt",info.UpdateDate},
                {"parmupdtm",info.UpdateTime},
                {"parmupdts",info.UpdateTimeStamp},
                {"parmupdoprtr",info.Operator}
            }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, "Already Exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);

        }

        public async Task<(Results result, object fingerprints)> LoadFingerprints(string userId)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYFINGERPRINTCR", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid", account.PGRP_ID},
                {"parmusrid", userId}
            });

            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }
    }
}
