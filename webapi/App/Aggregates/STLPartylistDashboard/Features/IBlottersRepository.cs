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

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(BlotterRepository))]
    public interface IBlottersRepository
    {
        Task<(Results result, object blotter)> SaveFile(Blotter info, string resultCode = "1");
        Task<(Results result, object blotter)> LoadBlotter(string plid, string pgrpid);
        Task<(Results result, object summon)> LoadSummon(string plid, string pgrpid, string brgycsno, string createdby, string summondt, int issummon, string modifiedby, string dtmodified);
        Task<(Results result, object caseNo)> UpdatedCaseNo();
    }

    public class BlotterRepository : IBlottersRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }

        public BlotterRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object blotter)> SaveFile(Blotter info, string resultCode = "1")
        {
            try
            {
                var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLOTTER", new Dictionary<string, object>()
                { 
                    {"paramplid",info.PL_ID},
                    {"parampgrpid",info.PGRP_ID},
                    {"parambrgycsno",info.BarangayCaseNo},
                    {"paramrgncd",info.RegionCode},
                    {"paramprvcd",info.ProvinceCode},
                    {"parammncpl",info.MunicipalCode},
                    {"parambrgycd",info.BrgyCode},
                    {"paramprk",info.PurokOrSitio},
                    {"parambrgycpt",info.BarangayCaptain},
                    {"paramcmpnm",info.ComplainantsName},
                    {"paramrspnm",info.RespondentsName},
                    {"paramwtns",info.Witness},
                    {"paramacstn",info.Accusations},
                    {"paramincp",info.PlaceOfIncedent},
                    {"paramstmt",info.NarrativeOfIncedent},
                    {"paramincdt",info.DateTimeOfIncedent},
                    {"paramcrtdby",info.BarangaySecretary},
                    {"paramissmn", null },
                    {"paramsmndt", null },
                    {"paramsmodby", null },
                    {"paramsmoddt", null },
                    {"parambmodby", info.ModifiedBy },
                    {"parambmoddt", info.DTModified }
                });

                if (result != null)
                    return (Results.Success, result);
                return (Results.Null, null);
            }
            catch (System.Exception)
            {

               return (Results.Null, "");
            }
            
        }

        public async Task<(Results result, object blotter)> LoadBlotter(string plid, string pgrpid)
        {
            try
            {
                var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLOTTER", new Dictionary<string, object>()
            {
                {"paramplid",plid },
                {"parampgrpid",pgrpid },
                {"parambrgycsno",null },
                {"paramrgncd", null },
                {"paramprvcd", null },
                {"parammncpl", null },
                {"parambrgycd", null },
                {"paramprk", null },
                {"parambrgycpt", null },
                {"paramcmpnm", null },
                {"paramrspnm", null },
                {"paramwtns", null },
                {"paramacstn", null },
                {"paramincp", null },
                {"paramstmt", null },
                {"paramincdt", null },
                {"paramcrtdby", null },
                {"paramissmn", null },
                {"paramsmndt", null },
                {"paramsmodby", null },
                {"paramsmoddt", null },
                {"parambmodby", null },
                {"parambmoddt", null }
            });
                if (result != null)
                    return (Results.Success, result);
                return (Results.Null, null);
            }
            catch (System.Exception)
            {
                return (Results.Null, null);
            }
        }

        public async Task<(Results result, object summon)> LoadSummon(string plid, string pgrpid, string brgycsno, string createdby, string summondt, int issummon, string modifiedby, string dtmodified)
        {
            try
            {
                var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLOTTER", new Dictionary<string, object>()
            {
                {"paramplid",plid },
                {"parampgrpid",pgrpid },
                {"parambrgycsno",brgycsno },
                {"paramrgncd", null },
                {"paramprvcd", null },
                {"parammncpl", null },
                {"parambrgycd", null },
                {"paramprk", null },
                {"parambrgycpt", null },
                {"paramcmpnm", null },
                {"paramrspnm", null },
                {"paramwtns", null },
                {"paramacstn", null },
                {"paramincp", null },
                {"paramstmt", null },
                {"paramincdt", null },
                {"paramcrtdby", createdby },
                {"paramissmn", issummon },
                {"paramsmndt", summondt },
                {"paramsmodby", modifiedby },
                {"paramsmoddt", dtmodified },
                {"parambmodby", null },
                {"parambmoddt", null }
            });
                if (result != null)
                    return (Results.Success, result);
                return (Results.Null, null);
            }
            catch (System.Exception)
            {
                return (Results.Null, null);
            }
        }

        public async Task<(Results result, object caseNo)> UpdatedCaseNo()
        {
            var result = _repo.DQuery<dynamic>("Select MAX(BRGY_CASE_NO) as CASENO from dbo.Blotter");
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }
    }
}
