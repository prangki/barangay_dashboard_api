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
                {"parmxml", report.XML },
                {"parmbrgycode", report.brgyCode },
                {"parmnum", report.typeOfReport }

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

        public async Task<(Results result, object household)> DynamicReportData(Report report)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_DynamicSelectPractice", new Dictionary<string, object>()
            {
                //{"parmplid",account.PL_ID },
                {"parmxml", report.XML },
                {"parmbrgycode", report.brgyCode },
                {"parmcolumnBitString", report.columnBits },

            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

    }
}
