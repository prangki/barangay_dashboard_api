using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.RequestModel.AppRecruiter;
using Comm.Commons.Extensions;
using webapi.Commons.AutoRegister;
using webapi.App.Model.User;
using webapi.App.Aggregates.Common.Dto;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using System.Globalization;
using webapi.App.RequestModel.Common;
using webapi.App.STLDashboardModel;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(STLMembershipRepository))]
    public interface ISTLMembershipRepository
    {
        Task<(Results result, String message, String UserID, String AcctID)> MembershipAsync(STLMembership membership, bool isUpdate = false);
        Task<(Results result, object account)> LoadAccount(string search);
        Task<(Results result, object account)> LoadAccountSearch(string search);
        Task<(Results result, object account)> LoadAccountAccess();
        Task<(Results result, object access)> LoadUserAccess(string userid);
        Task<(Results result, object account)> LoadMasterList();
        Task<(Results result, object account)> TotalREgister();
        Task<(Results result, object account)> LoadMasterList1(MasterListRequest request);
        Task<(Results result, string message)> AssignedAccess(string userid, string access);
        Task<(Results result, String message)> UpdateValidityAccount(ValidityAccount request);
        Task<(Results result, string message)> ChangeAccounttype(ChangeAccountType request);
        Task<(Results result, object parent)> Load_Parent(FilterRequest request);
        Task<(Results result, object children)> Load_Children(string userid);
        Task<(Results result, object educatt)> Load_EducationalAttainment(string plid, string pgrpid, string userid);
        Task<(Results result, object emphist)> Load_EmploymentHistory(string plid, string pgrpid, string userid);
        Task<(Results result, object orgz)> Load_Organization(string plid, string pgrpid, string userid);
        Task<(Results result, object prfpic)> Load_ProfilePictureHistory(string plid, string pgrpid, string userid);
        Task<(Results result, object prof)> Load_Profession(string search);
        Task<(Results result, object occ)> Load_Occupation(string search);
        Task<(Results result, object skl)> Load_Skills(string search);
        Task<(Results result, String message)> AssigendSkin(string skin);
        Task<(Results result, String message)> ResidenceDODAsyn(DOD req, bool isUpdate = false);
    }
    public class STLMembershipRepository : ISTLMembershipRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public STLMembershipRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }


        public async Task<(Results result, object account)> TotalREgister()
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDABDB07", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object account)> LoadMasterList()
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDABDB04", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID }
            });
            if (results != null)
                return (Results.Success, SubscriberDto.GetMasterList(results, 100));
            return (Results.Null, null);
        }
        public async Task<(Results result, object account)> LoadMasterList1(MasterListRequest request)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDABDB06", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmrownum",request.num_row },
                {"parmsrch",request.search },
                {"parmreg",request.reg },
                {"parmprov",request.prov },
                {"parmmun",request.mun },
                {"parmbrgy",request.brgy },
                {"parmsitio",request.sitio }
            });
            if (results != null)
                return (Results.Success, SubscriberDto.GetMasterList1(results, 100));
                //return (Results.Success, results);
            return (Results.Null, null);
        }
        public async Task<(Results result, object account)> LoadAccountAccess()
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDABDB02", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }
        public async Task<(Results result, object account)> LoadAccount(string search)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDABDB03", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmsearch", search }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object account)> LoadAccountSearch(string search)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDABDB08", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid", search }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message, string UserID, string AcctID)> MembershipAsync(STLMembership membership, bool isUpdate = false)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            //var results = _repo.DSpQueryMultiple("dbo.spfn_BDABDBCAACBB02", new Dictionary<string, object>()
            var results = _repo.DSpQueryMultiple("dbo.spfn_BDABDBBEBCAACBB02", new Dictionary<string, object>()
            {
                {"parmplid",membership.PLID },
                {"parmpgrpid",membership.PGRPID },
                {"parmtitlename", membership.TitleName },
                {"parmfnm", membership.Firstname },
                {"parmlnm",membership.Lastname },
                {"parmmnm",membership.Middlename },
                {"parmextensionname", membership.ExtensionName },
                {"parmnnm",membership.Nickname },
                {"parmreligion", membership.Religion },

                {"parmregistervoter",membership.RegisterVoter },
                {"parmprecentno", membership.PrecentNumber},
                {"parmclusterno", membership.ClusterNumber },

                {"parmmobno",membership.MobileNumber },
                {"parmemladd",membership.EmailAddress },

                {"parmwithdependent", membership.WChildren },
                {"parmwithdisability",membership.WDisability },
                {"parmtypedisability",membership.TypeDisability },
                {"parmparentresidebrgy", membership.ParentResideBrgy },

                {"parmhouseno", membership.HouseNo },
                {"parmhousehold", membership.HouseholdNo },
                {"parmfamily", membership.Family },

                {"parmbldType",membership.BloodType },
                {"parmbdate",membership.BirthDate },
                {"parmplaceofbirth" , membership.PlaceOfBirth },
                {"parmgender",membership.Gender },
                {"parmmstastus",membership.MaritalStatus },
                {"parmpartnerid", membership.PartnerID },
                {"parmntnlty",membership.Nationality },
                {"parmctznshp",membership.Citizenship },
                {"parmprofession", membership.Profession },
                {"parmoccptn",membership.Occupation },
                {"parmsklls",membership.Skills },
                {"parmheight", membership.Height },
                {"parmweight", membership.Weight },
                {"parmlivingwithparent", membership.LivingWParent },

                {"parmreg",membership.Region },
                {"parmprov",membership.Province },
                {"parmmun",membership.Municipality },
                {"parmbrgy",membership.Barangay },
                {"parmsitio",membership.Sitio },
                {"parmhmeadd",membership.HomeAddress },
                {"parmprsntadd",membership.PresentAddress },

                {"parmfr_id", membership.FR_ID },
                {"parmmo_id", membership.MO_ID },
                {"parmfr_firstname", membership.FrFirstName },
                {"parmfr_middlename", membership.FrMiddleInitial },
                {"parmfr_lastname", membership.FrLastname },
                {"parmfr_address", membership.FrAddress },
                {"parmfr_contactno", membership.FrContactNo },
                {"parmfr_email", membership.FrEmail },
                {"parmmo_firstname", membership.MoFirstname },
                {"parmmo_middlename", membership.MoMiddleInitial },
                {"parmmo_lastname", membership.MoLastname },
                {"parmmo_contactno", membership.MoContactNo },
                {"parmmo_email", membership.MoEmail },
                {"parmmo_address", membership.MoAddress },


                {"parmmonthlyincone", membership.MonthlyIncome },
                {"parmresidenttype", membership.ResidentType },
                {"parmdatereside", membership.DateReside },
                {"parmcompleteaddress", membership.CompleteAddress },
                {"parmpermanentresident", membership.PermanentResidence },
                {"parmseniorcitizenmember", membership.SeniorCitizenMember },
                {"parmsingleparent", membership.SingleParent },
                {"parmindigentfamily", membership.IndigentFamily },

                //{"parmchildren", membership.iChildren },
                {"parmeducationattainment", membership.iEducationalAttainment },
                {"parmvalidgovernmentid", membership.iValidGovernmentID },
                {"parmorganization", membership.iOrganization },
                {"parmemployementhistory", membership.iEmployement },

                {"parmisprfpicChange", membership.isProfilePictureChange },
                {"parmprfpic",membership.ImageUrl },
                {"parmisSignatureChange", membership.isSignatureChange },
                {"parmImgUrl",membership.ImageUrl },
                {"parmsignature",membership.SignatureURL },

                {"parmusername",membership.Username },
                {"parmusrid",(isUpdate?membership.Userid:"") },
                {"parmoptrid",account.USR_ID },
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Succesfull save", row["USR_ID"].Str(), row["ACT_ID"].Str());
                else if (ResultCode == "2")
                    return (Results.Failed, "Invalid Mobile Number", null, null);
                else if (ResultCode == "3")
                    return (Results.Failed, "Mobile Number already exist", null, null);
                else if (ResultCode == "4")
                    return (Results.Failed, "You are already Register", null, null);
                else if (ResultCode == "5")
                    return (Results.Failed, "Username already exist", null, null);
            }
            return (Results.Null, null, null, null);
        }

        public async Task<(Results result, object access)> LoadUserAccess(string userid)
        {
            var results = _repo.DQuery<dynamic>($"select PGS from STLAAD where USR_ID='{userid}'");
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> AssignedAccess(string userid, string access)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_AAD001", new Dictionary<string, object>()
            {
                {"parmusrid",userid},
                {"parmpgs",access }
            }).FirstOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully update!");
                return (Results.Failed, "Something wrong in your data, Please try again");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, String message)> UpdateValidityAccount(ValidityAccount request)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BDB03", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid", account.PGRP_ID },
                {"parmmvalfrmdt", request.DateFrom},
                {"parmvaltodt", request.DateTo},
                {"parmusrID", request.iUSR_ID},
                {"parmoptrid",account.USR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    return (Results.Success, "Successfully save!");
                }
                return (Results.Failed, "Somethings wrong in your data. Please try again");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> ChangeAccounttype(ChangeAccountType request)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BDB0C", new Dictionary<string, object>()
            {
                {"parmplid",request.PL_ID },
                {"parmpgrpid", request.PGRP_ID },
                {"parmoptrid",account.USR_ID },
                {"parmuserid", request.USR_ID},
                {"parmusername", request.USR_NM},
                {"parmaccttype", request.AccountType}
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    return (Results.Success, "Successfully save!");
                }
                else if(ResultCode=="2")
                        return (Results.Failed, "Username already exist. Please try again");
                return (Results.Failed, "Somethings wrong in your data. Please try again");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object parent)> Load_Parent(FilterRequest request)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDBPAR", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmbrgy",request.BrgyCode },
                {"parmgndr",request.Gender }
            });
            if (results != null)
                return (Results.Success, SubscriberDto.GetParentList(results, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, object children)> Load_Children(string userid)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BEBBDB0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid", userid }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object emphist)> Load_EmploymentHistory(string plid, string pgrpid, string userid)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDBEMPHST0A", new Dictionary<string, object>()
            {
                {"parmplid",plid},
                {"parmpgrpid",pgrpid },
                {"parmuserid", userid }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object educatt)> Load_EducationalAttainment(string plid, string pgrpid, string userid)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDBEDUCAT0A", new Dictionary<string, object>()
            {
                {"parmplid",plid},
                {"parmpgrpid",pgrpid },
                {"parmuserid", userid }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object orgz)> Load_Organization(string plid, string pgrpid, string userid)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDBORGZ0A", new Dictionary<string, object>()
            {
                {"parmplid",plid},
                {"parmpgrpid",pgrpid },
                {"parmuserid", userid }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object prfpic)> Load_ProfilePictureHistory(string plid, string pgrpid, string userid)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_PRFPIC0A", new Dictionary<string, object>()
            {
                {"parmplid",plid},
                {"parmpgrpid",pgrpid },
                {"parmuserid", userid }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object prof)> Load_Profession(string search)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDBPROF", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmsearch", search }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object occ)> Load_Occupation(string search)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDBOCC0A", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmsearch", search }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object skl)> Load_Skills(string search)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDBSKLA", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmsearch", search }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> AssigendSkin(string skin)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_AAD002", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID},
                {"parmpgrpid",account.PGRP_ID},
                {"parmusrid",account.USR_ID},
                {"parmskin",skin }
            }).FirstOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully update!");
                return (Results.Failed, "Something wrong in your data, Please try again");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, String message)> ResidenceDODAsyn(DOD req, bool isUpdate = false)
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BIMSDC0A", new Dictionary<string, object>()
            {
                {"parmplid",req.PLID },
                {"parmpgrpid",req.PGRPID },
                {"parmdcid", req.DCID },
                {"parmcontrolno", req.ControlNo },
                {"parmuserid", req.UserID },
                {"parmcauseofdeath", req.CauseofDeath },
                {"parmdieddate", req.DiedDate },
                {"parmdiedtime", req.DiedTime },
                {"parmorno",req.ORNumber },
                {"parmordoi",req.ORIssuedDate },
                {"parmamountpaid",req.AmountPaid },
                {"parmdocstamp",req.DocStamp },

                {"parmenablectc",req.EnableCommunityTax },
                {"parmctcno",req.CTCNo },
                {"parmctcissuedat",req.CTCIssuedAt },
                {"parmctcissuedon",req.CTCIssuedOn },
                {"parmoptrid",account.USR_ID }
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    if (!isUpdate)
                    {
                        req.DCID = row["DC_ID"].Str();
                        req.DeathCertificateNo = row["DC_ID"].Str();
                        req.ControlNo = row["CNTRL_NO"].Str();
                        req.VerifiedBy = row["VERIFIEDBY"].Str();
                        req.CertifiedBy = row["CERTIFIEDBY"].Str();
                    }
                    return (Results.Success, "Clearance succesfully save!");
                }
                else if (ResultCode == "0")
                    return (Results.Failed, "Check your Data, Please try again!");
                else if (ResultCode == "2")
                    return (Results.Failed, "This Account was In-Active!");
            }
            return (Results.Null, null);
        }
    }
}
