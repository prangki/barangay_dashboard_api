using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using webapi.App.STLDashboardModel;
using webapi.Commons.AutoRegister;
using Comm.Commons.Extensions;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.Common.Dto;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(BarangayOfficialsRepository))]
    public interface IBrangayOfficialsRepository
    {
        Task<(Results result, String message, String oflid)> BarangayOfficialAsync(BarangayOfficial request);
        Task<(Results result, String message)> UpdateBarangayOfficialAsync(BarangayOfficial request);
        Task<(Results result, object brgyofl)> LoadMBarangayOfficial(FilterRequest request);
        Task<(Results result, object brgyofl)> LoadMBarangayOfficial02(FilterRequest request);
        Task<(Results result, String message)> ElectedOfficialEndTerm(BarangayOfficial request);
    }
    public class BarangayOfficialsRepository:IBrangayOfficialsRepository
    {
        private readonly ISubscriber _identity;
        private readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public BarangayOfficialsRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message, string oflid)> BarangayOfficialAsync(BarangayOfficial request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BRGYOFL0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmoptrid",account.USR_ID },
                {"parmuserid", request.Userid },
                {"parmbarangaypstnid", request.BarangayPositionID},
                {"parmrankno",request.RankNo },
                {"parmcommittee",request.Committee },
                {"parmtermstart", request.TermStart },
                {"parmtermend", request.TermEnd },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully saved!", row["BRGY_OFL_ID"].Str());
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again!", null);
                else if (ResultCode == "2")
                    return (Results.Failed, "Please End the term of Previous Position, Please try again!", null);
                else if (ResultCode == "3")
                    return (Results.Failed, "Please End the term of Previous Elected Personnel of this Position, Please try again!", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, object brgyofl)> LoadMBarangayOfficial(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BRGYOFLBDB0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmstatus", request.Status},
                {"parmrownum", request.num_row},
                {"parmsearch", request.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllBrgyOfficialList(result.Read<dynamic>(), request.Userid, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, object brgyofl)> LoadMBarangayOfficial02(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BRGYOFLBDB0A02", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmstatus", request.Status},
                {"parmrownum", request.num_row},
                {"parmsearch", request.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllBrgyOfficialList(result.Read<dynamic>(), request.Userid, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> UpdateBarangayOfficialAsync(BarangayOfficial request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BRGYOFL0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmgbrgyoflid", request.BrgyOfficialID },
                {"parmoptrid",account.USR_ID },
                {"parmuserid", request.Userid },
                {"parmbarangaypstnid", request.BarangayPositionID},
                {"parmrankno",request.RankNo },
                {"parmcommittee",request.Committee },
                {"parmtermstart", request.TermStart },
                {"parmtermend", request.TermEnd },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully saved!");
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again!");
                else if (ResultCode == "2")
                    return (Results.Failed, "Please End the term of Previous Position, Please try again!");
                else if (ResultCode == "3")
                    return (Results.Failed, "Please End the term of Previous Elected Personnel of this Position, Please try again!");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> ElectedOfficialEndTerm(BarangayOfficial request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BRGYOFL0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid",request.Userid },
                {"parmoptrid",account.USR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully saved!");
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again!");
            }
            return (Results.Null, null);
        }
    }
}
