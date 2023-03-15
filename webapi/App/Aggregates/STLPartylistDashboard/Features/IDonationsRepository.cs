using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using webapi.App.STLDashboardModel;
using Comm.Commons.Extensions;
using webapi.Commons.AutoRegister;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(DonationRepository))]
    public interface IDonationsRepository
    {
        Task<(Results result, String message)> DonationAsync(Donation request);
        Task<(Results result, String message)> UpdateDonationAsync(Donation request);
        Task<(Results result, String message)> ResendOTP(Donation request);
        Task<(Results result, String message)> ClaimDonation(Donation request);
        Task<(Results result, object donation)> LoadDonations();
    }
    public class DonationRepository : IDonationsRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public DonationRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message)> DonationAsync(Donation request)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DAABDBCBA0B", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID },
                {"parmpgrpid", account.PGRP_ID },
                {"parmoptrid", account.USR_ID },
                //{"parmreceiveddate", request.DateReceived },
                {"parmamnt", request.Amount },
                {"parmpurpose", request.Purpose },
                {"parmaccount", request.iUSR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully saved!");
                else if (ResultCode == "3")
                    return (Results.Failed, "Donation already exist!");
                else if (ResultCode == "4")
                    return (Results.Failed, "Donation already exist!");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> UpdateDonationAsync(Donation request)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DAABDBCBA0A", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID },
                {"parmpgrpid", account.PGRP_ID },
                {"parmoptrid", account.USR_ID },
                //{"parmreceiveddate", request.DateReceived },
                {"parmusrid", request.iUSR_ID },
                {"parmamnt", request.Amount },
                {"parmpurpose", request.Purpose },
                {"parmdonoid", request.DonoID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully saved!");
                else if (ResultCode == "3")
                    return (Results.Failed, "Donation already exist!");
                else if (ResultCode == "4")
                    return (Results.Failed, "Donation already exist!");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, String message)> ResendOTP(Donation request)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DAABDBCBA0C", new Dictionary<string, object>()
            {
                {"parmplid", request.PL_ID },
                {"parmpgrpid", request.PGRP_ID },
                {"parmoptrid", account.USR_ID },
                {"parmusrmobno", request.MobileNo },
                {"parmdonoid", request.DonoID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully Resend Code!");
                else if (ResultCode == "0")
                    return (Results.Failed, "Something wrong in your data, Please try again!");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, String message)> ClaimDonation(Donation request)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DAABDBCBA0D", new Dictionary<string, object>()
            {
                {"parmplid", request.PL_ID },
                {"parmpgrpid", request.PGRP_ID },
                {"parmoptrid", account.USR_ID },
                {"parmusrmobno", request.MobileNo },
                {"parmdonoid", request.DonoID },
                {"parmotp", request.OTP }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully saved!");
                else if (ResultCode == "2")
                    return (Results.Failed, "Code already Expired!");
                else if (ResultCode == "0")
                    return (Results.Failed, "Something wrong in your data, Please try again!");
            }
            return (Results.Null, null);
        }
        public async Task<(Results result, object donation)> LoadDonations()
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DAADBDCBACBB0B", new Dictionary<string, object>() 
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

    }
}
