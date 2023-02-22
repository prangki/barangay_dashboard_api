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

        Task<(Results result, object household)> GetComplaints(string from, string to, string plid = "0002", string pgrpid = "002");
        Task<(Results result, object household)> GetOrgs(string xml);

        Task<(Results result, object household)> GetPreferences(ReportSettings settings);

        Task<(Results result, string message)> AddReportPreference(ReportSettings settings);

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
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_REPORTINGCustom", new Dictionary<string, object>()
            {
                //{"parmplid",account.PL_ID },
                {"parmplid", account.PL_ID },
                {"parmpgrpid", account.PGRP_ID },
                {"parmxml", report.XML },
                {"parmbrgycode", report.brgyCode },
                {"parmcolumnBitString", report.columnBits },
                {"parmfilterBitString", report.filterBits },
                {"parmagefrom", report.agefrom },
                {"parmageto", report.ageto },
                {"parmcode", report.code },
                {"parmloctype", report.loctype },
                {"parmaccttype", account.ACT_TYP },

            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object household)> GetComplaints(string from, string to , string plid = "0002", string pgrpid = "002")
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_COMPLAINTSSHOW", new Dictionary<string, object>()
            {
                //{"parmplid",account.PL_ID },
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "2") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "2") ? account.PGRP_ID : "002"},
                {"parmfrom", from },
                {"parmto", to},

            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object household)> GetOrgs(string xml)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_REPORTINGorgz", new Dictionary<string, object>()
            {
                //{"parmplid",account.PL_ID },
                {"parmplid", (account.ACT_TYP != "1" || account.ACT_TYP != "2") ? account.PL_ID : "0002"},
                {"parmpgrpid", (account.ACT_TYP != "1" || account.ACT_TYP != "2") ? account.PGRP_ID : "002"},
                {"parmxml", xml },


            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object household)> GetPreferences(ReportSettings settings)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_REPORTPREFERENCE_GET", new Dictionary<string, object>()
            {
                //{"parmplid",account.PL_ID },
                {"parmplid", settings.plid},
                {"parmpgrpid", settings.pgrpid},
                {"parmuserid", settings.userid },

            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> AddReportPreference(ReportSettings settings)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_REPORTPREFERENCE_INSERT", new Dictionary<string, object>()
            {

                {"parmplid", settings.plid},
                {"parmpgrpid", settings.pgrpid },
                {"parmuserid", settings.userid},
                {"parmpreferencedescription", settings.description},
                {"parmlayoutindex", settings.layoutIndex },
                {"parmtitle", settings.title},
                {"parmsubtitle", settings.subtitle},
                {"parmdateformatindex", settings.dateFormat },
                {"parmselectbits", settings.selectBits.ToString()},
                {"parmdistinctionbits", settings.distinctionBits.ToString()},
                {"parmagefrom", settings.from },
                {"parmageto", settings.to},
                {"parmchartindex", settings.chartIndex.ToString()},
                {"parmsignatureindex", settings.signIndex.ToString() },
                {"parmisactive", settings.isactive },


            });

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

    }
}
