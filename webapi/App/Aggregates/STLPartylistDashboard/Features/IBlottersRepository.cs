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
    [Service.ITransient(typeof(BlotterRepository))]
    public interface IBlottersRepository
    {
        Task<(Results result, string message)> SaveBlotter(Blotter info);
        Task<(Results result, string message)> SaveBlotterV2(Blotter info);
        Task<(Results result, string message)> UpdateBlotter(Blotter info);
        Task<(Results result, string message)> UpdateBlotterV2(Blotter info);
        Task<(Results result, object blotter)> LoadBlotter(int id, int currentRow, string from, string to);
        Task<(Results result, object blotter)> LoadBlotterV2(int status);
        Task<(Results result, string message)> SaveSummon(Blotter info);
        Task<(Results result, string message)> UpdateSummon(Blotter info);
        Task<(Results result, string message)> ResolveSummon(Blotter info);
        Task<(Results result, string message)> RemoveSummon(Blotter info);
        Task<(Results result, object summon)> LoadSummon(int currentRow, string from, string to);
        Task<(Results result, object caseNo)> GetControlNo();
        Task<(Results result, object brgycpt)> GetBrgyCpt(string plid, string pgrpid);
        Task<(Results result, object docpath)> Reprint(string plid, string pgrpid, string brgycsno, string colname);
        Task<(Results result, object signatures)> GetSignature(string plid, string pgrpid);
        Task<(Results result, object header)> GetHeader(string plid, string pgrpid);
        Task<(Results result, object residents)> GetAllResidents();
        Task<(Results result, object document)> GetDocumentTemplate(string docname);
        Task<(Results result, string message)> Complaint(string caseno, string createby, string createdate);
        Task<(Results result, string message)> Cancel(string caseno, string reason, string createby, string createdate);
        Task<(Results result, object report)> LoadReport();
        Task<(Results result, object report)> LoadCaseIdentifier(string name);
        Task<(Results result, object attachment)> LoadAttachment(string caseno);
        Task<(Results result, object attachment)> LoadComplainants(string caseNo, string complaintId);
        Task<(Results result, object attachment)> LoadRespondents(string caseNo, string respondentId);
        Task<(Results result, string message)> SaveForm(Blotter info);
        Task<(Results result, object template)> LoadForm();
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

        public async Task<(Results result, string message)> SaveBlotter(Blotter info)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLOTTERIO", new Dictionary<string, object>()
            {
                //{"paramplid",account.PL_ID},
                //{"parampgrpid",account.PGRP_ID},
                //{"parambrgyid",account.LOC_BRGY},
                //{"parambrgycsno",info.BarangayCaseNo},
                //{"paramprk",info.PurokOrSitio},
                //{"parambrgycpt",info.BarangayCaptain},
                //{"paramcmpnm",info.ComplainantsName},
                //{"paramrspnm",info.RespondentsName},
                //{"paramcmplnttyp",info.ComplaintType},
                //{"paramjsonacmplc",info.JsonStringAccomplice},
                //{"paramjsonattchmnt",info.JsonAttachment},
                //{"paramacstn",info.Accusations},
                //{"paramincp",info.PlaceOfIncident},
                //{"paramstmt",info.NarrativeOfIncident},
                //{"paramincdt",info.DateOfIncident},
                //{"paraminctm",info.TimeOfIncident},
                //{"paramcrtdby",info.BarangaySecretary},
                //{"paramcrtddt",info.DateCreated}
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

        public async Task<(Results result, string message)> SaveBlotterV2(Blotter info)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLTR00", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID},
                {"parmpgrpid",account.PGRP_ID},
                {"parmbrgyid",account.LOC_BRGY},
                {"parmjsoncmplnnt",info.JsonStringComplainant},
                {"parmcsit",info.ComplainantSitio},
                {"parmjsonrspndt",info.JsonRespondent},
                {"parmdsit",info.RespondentSitio},
                {"parmwtns",info.Witness},
                {"parmacstn",info.Accusations},
                {"parmcstyp",info.CaseType},
                {"parmplcinc",info.PlaceOfIncident},
                {"parmincdt",info.DateOfIncident},
                {"parminctm",info.TimeOfIncident},
                {"parmstmt",info.Statement},
                {"parmjsonattchmt",info.JsonAttachment},
                {"parmcrtdby",info.BarangaySecretary}
                //{"paraminctm",info.TimeOfIncident},
                //{"paramcrtdby",info.BarangaySecretary},
                //{"paramcrtddt",info.DateCreated}
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

        public async Task<(Results result, string message)> UpdateBlotter(Blotter info)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLOTTERUO", new Dictionary<string, object>()
            {
                //{"paramplid",account.PL_ID},
                //{"parampgrpid",account.PGRP_ID},
                //{"parambrgycsno",info.BarangayCaseNo},
                //{"paramprk",info.PurokOrSitio},
                //{"paramcmpnm",info.ComplainantsName},
                //{"paramrspnm",info.RespondentsName},
                //{"paramcmplnttyp",info.ComplaintType},
                //{"paramacstn",info.Accusations},
                //{"paramincp",info.PlaceOfIncident},
                //{"paramincdt",info.DateOfIncident},
                //{"paraminctm",info.TimeOfIncident},
                //{"paramjsonattchmnt",info.JsonAttachment},
                //{"paramstmt",info.NarrativeOfIncident},
                //{"parammodby", info.ModifiedBy},
                //{"parammoddt", info.DTModified}
            }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully updated");
                else if (ResultCode == "0")
                    return (Results.Failed, "Already Exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);

        }

        public async Task<(Results result, string message)> UpdateBlotterV2(Blotter info)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLTR02", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID},
                {"parmpgrpid",account.PGRP_ID},
                {"parmbrgycsno",info.BarangayCaseNo},
                //{"parmjsoncmplnnt",info.JsonStringComplainant},
                {"parmcsit",info.ComplainantSitio},
                //{"parmjsonrspndt",info.JsonRespondent},
                {"parmdsit",info.RespondentSitio},
                {"parmwtns",info.Witness},
                {"parmacstn",info.Accusations},
                {"parmcstyp",info.CaseType},
                {"parmplcinc",info.PlaceOfIncident},
                {"parmincdt",info.DateOfIncident},
                {"parminctm",info.TimeOfIncident},
                {"parmstmt",info.Statement},
                //{"parmjsonattchmt",info.JsonAttachment},
                {"parmstlmt",info.TermsOfSettlement},
                {"parmawrd",info.Award},
                {"parmsumdt",info.SummonDate},
                {"parmsumtm",info.SummonTime},
                {"parmpktchrmn",info.PangkatChairman},
                {"parmpktscrty",info.PangkatSecretary},
                {"parmpktfmbr",info.PangkatFMember},
                {"parmpktsmbr",info.PangkatSMember},
                {"parmcrtdby",info.BarangaySecretary},
                {"parmsts",info.Status}
            }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully update");
                else if (ResultCode == "0")
                    return (Results.Failed, "Already Exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);

        }

        public async Task<(Results result, object blotter)> LoadBlotter(int id, int currentRow, string from, string to)
        {
            try
            {
                var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLOTTERIO", new Dictionary<string, object>()
                {
                    {"paramplid",account.PL_ID },
                    {"parampgrpid",account.PGRP_ID },
                    {"paramvwtyp", id },
                    {"paramfrom", from },
                    {"paramto", to },
                    {"paramcurrow", currentRow }
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

        public async Task<(Results result, object blotter)> LoadBlotterV2(int status)
        {
            try
            {
                var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLTR01", new Dictionary<string, object>()
                {
                    {"parmplid",account.PL_ID },
                    {"parmpgrpid",account.PGRP_ID },
                    {"parmsts", status}
                    //{"paramvwtyp", id },
                    //{"paramfrom", from },
                    //{"paramto", to },
                    //{"paramcurrow", currentRow }
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

        public async Task<(Results result, object summon)> LoadSummon(int currentRow, string from, string to)
        {
            try
            {
                var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLOTTERUO", new Dictionary<string, object>()
            {
                {"paramplid", account.PL_ID },
                {"parampgrpid", account.PGRP_ID },
                {"paramfrom", from },
                {"paramto", to },
                {"parmcurrow", currentRow }
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

        public async Task<(Results result, string message)> SaveSummon(Blotter info)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLTRCSRC", new Dictionary<string, object>()
            {
                {"paramplid",account.PL_ID },
                {"parampgrpid",account.PGRP_ID },
                {"parambrgycsno",info.BarangayCaseNo },
                {"paramissmn", 1 },
                {"paramsmndt", info.SummonDate},
                {"parammodby",info.ModifiedBy },
                {"parammoddt",info.DTModified }
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

        public async Task<(Results result, string message)> UpdateSummon(Blotter info)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLOTTERUO", new Dictionary<string, object>()
            {
                {"paramplid",account.PL_ID},
                {"parampgrpid",account.PGRP_ID },
                {"parambrgycsno",info.BarangayCaseNo },
                {"paramissmn", 1},
                {"paramsmndt", info.SummonDate },
                {"parammodby", info.ModifiedBy },
                {"parammoddt", info.DTModified }
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

        public async Task<(Results result, string message)> ResolveSummon(Blotter info)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLTRCSRC", new Dictionary<string, object>()
            {
                {"paramplid", account.PL_ID},
                {"parampgrpid", account.PGRP_ID},
                {"parambrgycsno", info.BarangayCaseNo},
                {"parammodby", info.ModifiedBy},
                {"parammoddt", info.DTModified}
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

        public async Task<(Results result, string message)> RemoveSummon(Blotter info)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLOTTERUO", new Dictionary<string, object>()
            {
                {"paramplid",account.PL_ID},
                {"parampgrpid",account.PGRP_ID },
                {"parambrgycsno",info.BarangayCaseNo },
                {"paramissmn", 0 },
                {"parammodby", info.ModifiedBy }
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

        public async Task<(Results result, object brgycpt)> GetBrgyCpt(string plid, string pgrpid)
        {
            var result = _repo.DQuery<dynamic>("with bea as (select PL_ID, PGRP_ID,USR_ID,ACT_ID,LDR_TYP from dbo.STLBEA) " +
            "select TOP(1) a.*, b.FLL_NM, b.S_ACTV from bea a left join dbo.STLBDB b on b.PL_ID = a.PL_ID and b.PGRP_ID = a.PGRP_ID and b.USR_ID = a.USR_ID and b.ACT_ID = a.ACT_ID " +
            $"where a.PL_ID='{account.PL_ID}' and a.PGRP_ID='{account.PGRP_ID}' and a.LDR_TYP = '1' and b.S_ACTV = '1';");
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object caseNo)> GetControlNo()
        {
            try
            {
                var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLTRCNTRL", new Dictionary<string, object>()
                {
                    {"parmplid",account.PL_ID },
                    {"parmpgrpid",account.PGRP_ID },
                    {"parmbrgyid",account.LOC_BRGY }
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

        public async Task<(Results result, object docpath)> Reprint(string plid, string pgrpid, string brgycsno, string colname)
        {
            var result = _repo.DQuery<dynamic>($"Select TOP(1) {colname} as DOCPATH from dbo.BIMSBLTR where PL_ID='{account.PL_ID}' and PGRP_ID='{account.PGRP_ID}' and BRGY_CASE_NO='{brgycsno}'");
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object signatures)> GetSignature(string plid, string pgrpid)
        {
            try
            {
                var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGY_SIGNATORY0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
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

        public async Task<(Results result, object header)> GetHeader(string plid, string pgrpid)
        {
            var result = _repo.DQuery<dynamic>($"Select TOP(1) * from dbo.OFFICIAL_HEADER where PL_ID='{account.PL_ID}' and PGRP_ID='{account.PGRP_ID}'");
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object residents)> GetAllResidents()
        {
            try
            {
                var result = _repo.DSpQuery<dynamic>($"dbo.spfn_GETALLRESIDENTS", new Dictionary<string, object>()
                {
                    {"parmplid",account.PL_ID },
                    {"parmpgrpid",account.PGRP_ID }
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

        public async Task<(Results result, object document)> GetDocumentTemplate(string docname)
        {
            try
            {
                var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYTPLDOC", new Dictionary<string, object>()
                {
                    {"parmplid",account.PL_ID },
                    {"parmpgrpid",account.PGRP_ID },
                    {"parmdocnm",docname }
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

        public async Task<(Results result, string message)> Complaint(string caseno, string createby, string createdate)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLTRCSRC", new Dictionary<string, object>()
            {
                {"paramplid",account.PL_ID},
                {"parampgrpid",account.PGRP_ID },
                {"parambrgycsno",caseno },
                {"paramscmplnt", 1 },
                {"parammodby", createby },
                {"parammoddt", createdate }
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

        public async Task<(Results result, string message)> Cancel(string caseno, string reason, string createby, string createdate)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLTRCSRC", new Dictionary<string, object>()
            {
                {"paramplid",account.PL_ID},
                {"parampgrpid",account.PGRP_ID },
                {"parambrgycsno",caseno },
                {"paramscncl", 1 },
                {"parammodby", createby },
                {"parammoddt", createdate },
                {"paramrsn", reason }
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

        public async Task<(Results result, object report)> LoadReport()
        {
            try
            {
                var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLTRRPT", new Dictionary<string, object>()
                {
                    {"parmplid",account.PL_ID },
                    {"parmpgrpid",account.PGRP_ID },
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

        public async Task<(Results result, object report)> LoadCaseIdentifier(string name)
        {
            try
            {
                var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYCSID", new Dictionary<string, object>()
                {
                    {"parmplid",account.PL_ID },
                    {"parmpgrpid",account.PGRP_ID },
                    {"parmname",name },
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

        public async Task<(Results result, object attachment)> LoadAttachment(string caseno)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLOTTERLA", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid", account.PGRP_ID},
                {"parmcaseno", caseno}
            });

            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object attachment)> LoadComplainants(string caseNo, string complaintId)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLTR0C01", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid", account.PGRP_ID},
                {"parmcsno", caseNo},
                {"parmcmplntid", complaintId}
            });

            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object attachment)> LoadRespondents(string caseNo, string respondentId)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLTR0R01", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid", account.PGRP_ID},
                {"parmcsno", caseNo},
                {"parmrspdtid", respondentId}
            });

            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> SaveForm(Blotter info)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLTR0F00", new Dictionary<string, object>()
            {
                {"parmfrmid", info.TemplateId },
                {"parmtmplnm", info.TemplateName },
                {"parmtmplcnt", info.TemplateContent },   
                {"parmrgsby", account.USR_ID }
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

        public async Task<(Results result, object template)> LoadForm()
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYBLTR0F01", new Dictionary<string, object>(){ });

            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

    }
}
