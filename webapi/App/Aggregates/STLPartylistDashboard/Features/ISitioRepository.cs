using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using Comm.Commons.Extensions;
using webapi.Commons.AutoRegister;
using webapi.App.STLDashboardModel;
using webapi.App.RequestModel.AppRecruiter;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(SitioRepository))]
    public interface ISitioRepository
    {
        Task<(Results result, String message, String sitcode)> SitioAsync(Sitio sit, bool isUpdate = false);
        Task<(Results result, object sit)> LoadSitio(string code);
        Task<(Results result, object brgy)> LoadBrgy(BarangayList reques);
        Task<(Results result, object brgy)> LoadBrgyPerTown(string strmun);
        Task<(Results result, object sit)> LoadSitioPerBarangay(string strcode);
        Task<(Results result, String message, String brgycode)> BrgyAsync(Barangay brgy, bool isUpdate = true);
        Task<(Results result, object brgy)> LoadBarangay(Barangay brgy);
    }
    public class SitioRepository : ISitioRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public SitioRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }
        public async Task<(Results result, string message, string sitcode)> SitioAsync(Sitio sit, bool isUpdate = false)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_LLL0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmregion",sit.Region },
                {"parmprov",sit.Province },
                {"parmmun",sit.Municipality },
                {"parmbrgy",sit.Brgy },
                {"parmsitid",(isUpdate?sit.SitioID:"") },
                {"parmsitioname",sit.SitioName },
                {"parmoptrid", account.USR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save", row["SIT_ID"].Str());
                else if(ResultCode=="0")
                    return (Results.Failed, "Already Exist", null);
                else if(ResultCode=="2")
                    return (Results.Null, null, null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, object sit)> LoadSitio(string code)
        {
            //var result = _repo.DQuery<dynamic>($"dbo.spfn_LLL0B");
            //if (result != null)
            //{
            //    return (Results.Success, result);
            //}
            //return (Results.Null, null);

            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_LLL0B", new Dictionary<string, object>()
            {
                {"parmcode",code },
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object brgy)> LoadBrgy(BarangayList request)
        {
            //var result = _repo.DQuery<dynamic>($"dbo.spfn_BRGY0A");
            //if (result != null)
            //{
            //    return (Results.Success, result);
            //}
            //return (Results.Null, null);
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGY0A", new Dictionary<string, object>()
            {
                {"parmrownum",request.num_row },
                {"parmislandcode",request.islandcode },
                {"parmreg",request.reg },
                {"parmprov",request.prov },
                {"parmmun",request.mun },
                {"parmbrgy",request.brgy }
            });
            if(result!=null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message, string brgycode)> BrgyAsync(Barangay brgy, bool isUpdate = true)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGY0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmislandcode",brgy.IslandCode },
                {"parmregion",brgy.Region },
                {"parmprov",brgy.Province },
                {"parmmun",brgy.Municipality },
                {"parmmuncode",brgy.MunicipalityCode },
                {"parmbrgy",(isUpdate?brgy.BarangayCode:"") },
                {"parmbrgyname",brgy.BarangayName },
                {"parmoptrid", account.USR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save", row["BRGY_CODE"].Str());
                else if (ResultCode == "0")
                    return (Results.Failed, "Already Exist",null);
                else if (ResultCode == "2")
                    return (Results.Null, null,null);
            }
            return (Results.Null, null,null);
        }

        public async Task<(Results result, object brgy)> LoadBarangay(Barangay brgy)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BRGY0C", new Dictionary<string, object>()
            {
                {"@parmcode",brgy.ID }
            });
            if (result != null)
            {
                return (Results.Success, SubscriberDto.GetBrgyList(result.Read<dynamic>()));
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object brgy)> LoadBrgyPerTown(string strmun)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BRGY0D", new Dictionary<string, object>()
            {
                {"@parmcode",strmun }
            });
            if (result != null)
            {
                return (Results.Success, SubscriberDto.GetBrgyList(result.Read<dynamic>()));
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object sit)> LoadSitioPerBarangay(string strcode)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_LLL0D", new Dictionary<string, object>() 
            {
                {"parmbrgy", strcode }
            });
            if (result != null)
            {
                return (Results.Success, result);
            }
            return (Results.Null, null);
        }
    }
}
