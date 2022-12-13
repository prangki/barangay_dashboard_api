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
using webapi.App.STLDashboardModel;
using webapi.App.Features.UserFeature;


namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(BarangayReportRepository))]
    public interface IBarangayReportRepository
    {

        Task<(Results result, object household)> LoadReportperSitio(Report report);
        Task<(Results result, object household)> DataAnalytics(Report report);

        Task<(Results result, object household)> GetStatisticalData(StatisticalData data);
        Task<(Results result, object household)> DynamicReportData(Report report);


    }
    public class BarangayReportRepository : IBarangayReportRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public BarangayReportRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }


        public async Task<(Results result, object household)> LoadReportperSitio(Report report)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_REPORTINGperSitio", new Dictionary<string, object>()
            {
                //{"parmplid",account.PL_ID },
                {"parmplid", account.PL_ID },
                {"parmpgrpid", account.PGRP_ID },

                {"parmxml", report.XML },

                {"parmbrgycode", "" },

                {"parmnum", report.typeOfReport },

                {"parmcode", report.code },
                {"parmloctype", report.loctype },
                {"parmaccttype", account.ACT_TYP },

            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object household)> DataAnalytics(Report report)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_DATAANALYTICS", new Dictionary<string, object>()
            {
                //{"parmplid",account.PL_ID },
                {"parmxml", report.XML },
                {"parmbrgycode", report.brgyCode },

            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object household)> GetStatisticalData(StatisticalData data)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_STATISTICS", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmaccttype",account.ACT_TYP },
                {"parmcode", data.code },
                {"parmloctype", data.loctype },
                {"parmxml", data.xml },


            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object household)> DynamicReportData(Report report)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_DynamicSelectPractice", new Dictionary<string, object>()
            {
                //{"parmplid",account.PL_ID },
                {"parmplid", account.PL_ID },
                {"parmpgrpid", account.PGRP_ID },
                {"parmxml", report.XML },
                {"parmbrgycode", report.brgyCode },
                {"parmcolumnBitString", report.columnBits },
                {"parmcode", report.code },
                {"parmloctype", report.loctype },
                {"parmaccttype", account.ACT_TYP },

            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

    }
}
