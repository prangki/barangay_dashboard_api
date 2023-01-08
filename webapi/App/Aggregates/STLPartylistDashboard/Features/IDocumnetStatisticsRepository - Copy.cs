
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
using System.Text.RegularExpressions;


namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(DocumentStatisticsRepository))]
    public interface IDocumentStatisticsRepository
    {
        
        Task<(Results result, object position)> GetDocStatBrgyClearance(DocumentStatistics docstat, string plid = "0002", string pgrpid = "002");
        Task<(Results result, object position)> GetDocStatBusinessClearance(DocumentStatistics docstat, string plid = "0002", string pgrpid = "002");
        Task<(Results result, object position)> GetDocStatLegalDocs(DocumentStatistics docstat, string plid = "0002", string pgrpid = "002");
        Task<(Results result, object position)> GetDocStatDeathCertificate(DocumentStatistics docstat, string plid = "0002", string pgrpid = "002");
        //==================================ShowByStatus=============================================================//
        Task<(Results result, object position)> GetDocShowByStatusBrgyClearance(int statustype, int datetype, string fromdate, string todate, string plid = "0002", string pgrpid = "002");
        Task<(Results result, object position)> GetDocShowByStatusBusinessClearance(int statustype, int datetype, string fromdate, string todate, string plid = "0002", string pgrpid = "002");
        Task<(Results result, object position)> GetDocShowByStatusLegalDocs(int statustype, int datetype, string fromdate, string todate, string plid = "0002", string pgrpid = "002");
        Task<(Results result, object position)> GetDocShowByStatusDeathCertificate(int statustype, int datetype, string fromdate, string todate, string plid = "0002", string pgrpid = "002");
        //==================================MonthlyOrders=============================================================//
        Task<(Results result, object position)> GetDocMonthlyOrdersBrgyClearance02(string year, string plid = "0002", string pgrpid = "002");
        Task<(Results result, object position)> GetDocMonthlyOrdersBusinessClearance02(string year, string plid = "0002", string pgrpid = "002");
        Task<(Results result, object position)> GetDocMonthlyOrdersLegalDocs02(string year, string plid = "0002", string pgrpid = "002");
        Task<(Results result, object position)> GetDocMonthlyOrdersDeathCertificate02(string year, string plid = "0002", string pgrpid = "002");
        //==================================MonthlyRevenue=============================================================//
        Task<(Results result, object position)> GetDocMonthlyRevenueBrgyClearance02(string year, string plid = "0002", string pgrpid = "002");
        Task<(Results result, object position)> GetDocMonthlyRevenueBusinessClearance02(string year, string plid = "0002", string pgrpid = "002");
        Task<(Results result, object position)> GetDocMonthlyRevenueLegalDocs02(string year, string plid = "0002", string pgrpid = "002");
        Task<(Results result, object position)> GetDocMonthlyRevenueDeathCertificate02(string year, string plid = "0002", string pgrpid = "002");

    }

    public class DocumentStatisticsRepository : IDocumentStatisticsRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;

        public STLAccount account { get { return _identity.AccountIdentity(); } }

        public DocumentStatisticsRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object position)> GetDocStatBrgyClearance(DocumentStatistics docstat, string plid = "0002", string pgrpid = "002")
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DocStatsBrgyClearance", new Dictionary<string, object>()
            {
                {"parmtype", docstat.type},
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? pgrpid : "002"},
                {"parmfrom", docstat.dtfrom},
                {"parmto", docstat.dtto},

            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object position)> GetDocStatBusinessClearance(DocumentStatistics docstat, string plid = "0002", string pgrpid = "002")
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DocStatsBusinessClearance", new Dictionary<string, object>()
            {
                {"parmtype", docstat.type},
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? pgrpid : "002"},
                {"parmfrom", docstat.dtfrom},
                {"parmto", docstat.dtto},

            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object position)> GetDocStatLegalDocs(DocumentStatistics docstat, string plid = "0002", string pgrpid = "002")
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DocStatsLegalDocs", new Dictionary<string, object>()
            {
                {"parmtype", docstat.type},
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? pgrpid : "002"},
                {"parmfrom", docstat.dtfrom},
                {"parmto", docstat.dtto},

            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);

        }

        public async Task<(Results result, object position)> GetDocStatDeathCertificate(DocumentStatistics docstat, string plid = "0002", string pgrpid = "002")
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DocStatsDeathCertificate", new Dictionary<string, object>()
            {
                {"parmtype", docstat.type},
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? pgrpid : "002"},
                {"parmfrom", docstat.dtfrom},
                {"parmto", docstat.dtto},

            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        //==============Show By Status=====================//
        public async Task<(Results result, object position)> GetDocShowByStatusBrgyClearance(int statustype, int datetype, string fromdate, string todate, string plid = "0002", string pgrpid = "002")
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DocShowByStatusBrgyClearance", new Dictionary<string, object>()
            {
                {"parmtypestatus", statustype},
                {"parmtypedate", datetype},
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? pgrpid : "002"},
                {"parmfrom", fromdate},
                {"parmto", todate},

            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object position)> GetDocShowByStatusBusinessClearance(int statustype, int datetype, string fromdate, string todate, string plid = "0002", string pgrpid = "002")
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DocShowByStatusBusinessClearance", new Dictionary<string, object>()
            {
                {"parmtypestatus", statustype},
                {"parmtypedate", datetype},
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? pgrpid : "002"},
                {"parmfrom", fromdate},
                {"parmto", todate},

            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object position)> GetDocShowByStatusDeathCertificate(int statustype, int datetype, string fromdate, string todate, string plid = "0002", string pgrpid = "002")
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DocShowByStatusDeathCertificate", new Dictionary<string, object>()
            {
                {"parmtypestatus", statustype},
                {"parmtypedate", datetype},
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? pgrpid : "002"},
                {"parmfrom", fromdate},
                {"parmto", todate},

            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object position)> GetDocShowByStatusLegalDocs(int statustype, int datetype, string fromdate, string todate, string plid = "0002", string pgrpid = "002")
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DocShowByStatusLegalDocs", new Dictionary<string, object>()
            {
                {"parmtypestatus", statustype},
                {"parmtypedate", datetype},
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? pgrpid : "002"},
                {"parmfrom", fromdate},
                {"parmto", todate},

            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }
        //==============MONTHLY Orders=====================//
        public async Task<(Results result, object position)> GetDocMonthlyOrdersBrgyClearance02(string year, string plid = "0002", string pgrpid = "002")
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DocMonthlyOrdersBrgyClearance02", new Dictionary<string, object>()
            {
                {"parmyear", year},
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? pgrpid : "002"},

            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object position)> GetDocMonthlyOrdersBusinessClearance02(string year, string plid = "0002", string pgrpid = "002")
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DocMonthlyOrdersBusinessClearance02", new Dictionary<string, object>()
            {
                {"parmyear", year},
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? pgrpid : "002"},

            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }
        public async Task<(Results result, object position)> GetDocMonthlyOrdersLegalDocs02(string year, string plid = "0002", string pgrpid = "002")
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DocMonthlyOrdersLegalDocs02", new Dictionary<string, object>()
            {
                {"parmyear", year},
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? pgrpid : "002"},

            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object position)> GetDocMonthlyOrdersDeathCertificate02(string year, string plid = "0002", string pgrpid = "002")
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DocMonthlyOrdersDeathCertificate02", new Dictionary<string, object>()
            {
                {"parmyear", year},
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? pgrpid : "002"},

            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        

        //==============MONTHLY Revenue=====================//
        public async Task<(Results result, object position)> GetDocMonthlyRevenueBrgyClearance02(string year, string plid = "0002", string pgrpid = "002")
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DocMonthlyRevenueBrgyClearance02", new Dictionary<string, object>()
            {
                {"parmyear", year},
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? pgrpid : "002"},

            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object position)> GetDocMonthlyRevenueBusinessClearance02(string year, string plid = "0002", string pgrpid = "002")
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DocMonthlyRevenueBusinessClearance02", new Dictionary<string, object>()
            {
                {"parmyear", year},
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? pgrpid : "002"},

            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object position)> GetDocMonthlyRevenueLegalDocs02(string year, string plid = "0002", string pgrpid = "002")
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DocMonthlyRevenueLegalDocs02", new Dictionary<string, object>()
            {
                {"parmyear", year},
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? pgrpid : "002"},

            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object position)> GetDocMonthlyRevenueDeathCertificate02(string year, string plid = "0002", string pgrpid = "002")
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DocMonthlyRevenueDeathCertificate02", new Dictionary<string, object>()
            {
                {"parmyear", year},
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "1") ? pgrpid : "002"},

            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

    }


}
