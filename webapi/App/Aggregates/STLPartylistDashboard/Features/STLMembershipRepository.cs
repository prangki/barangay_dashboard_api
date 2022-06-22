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

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(STLMembershipRepository))]
    public interface ISTLMembershipRepository
    {
        Task<(Results result, String message, String AcctID)> MembershipAsync(STLMembership membership, bool isUpdate = false);
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
                {"parmbrgy",request.brgy }
            });
            if (results != null)
                return (Results.Success, SubscriberDto.GetMasterList1(results, 100));
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

        public async Task<(Results result, string message, string AcctID)> MembershipAsync(STLMembership membership, bool isUpdate = false)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            var results = _repo.DSpQueryMultiple("dbo.spfn_BDABDBCAACBB02", new Dictionary<string, object>()
            {
                {"parmplid",membership.PLID },
                {"parmpgrpid",membership.PGRPID },
                {"parmfnm", textInfo.ToTitleCase(membership.Firstname) },
                {"parmlnm",textInfo.ToTitleCase(membership.Lastname) },
                {"parmmnm",textInfo.ToTitleCase(membership.Middlename) },
                {"parmnnm",textInfo.ToTitleCase(membership.Nickname) },
                {"parmmobno",membership.MobileNumber },
                {"parmgender",membership.Gender },
                {"parmmstastus",membership.MaritalStatus },
                {"parmemladd",membership.EmailAddress },
                {"parmhmeadd",textInfo.ToTitleCase(membership.HomeAddress) },
                {"parmprsntadd",textInfo.ToTitleCase(membership.PresentAddress) },
                {"parmprecentno", membership.PrecentNumber},
                {"parmclusterno", membership.ClusterNumber },
                {"parmsquenceno", membership.SequnceNumber },
                {"parmreg",membership.Region },
                {"parmprov",membership.Province },
                {"parmmun",membership.Municipality },
                {"parmbrgy",membership.Barangay },
                {"parmsitio",membership.Sitio },
                {"parmbdate",membership.BirthDate },
                {"parmctznshp",textInfo.ToTitleCase(membership.Citizenship) },
                {"parmbldType",textInfo.ToTitleCase(membership.BloodType) },
                {"parmntnlty",textInfo.ToTitleCase(membership.Nationality) },
                {"parmoccptn",textInfo.ToTitleCase(membership.Occupation) },
                {"parmsklls",textInfo.ToTitleCase(membership.Skills) },
                {"parmprfpic",membership.ImageUrl },
                {"parmImgUrl",membership.ImageUrl },
                {"parmsignature",membership.SignatureURL },
                {"parmusertype",membership.AccountType },
                {"parmEmployed",membership.isEmployed },

                {"parmusername",membership.Username },
                {"parmpassword",membership.Userpassword },
                {"parmusrid",(isUpdate?membership.Userid:"") },
                {"parmoptrid",account.USR_ID },
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Succesfull save",row["ACT_ID"].Str());
                else if (ResultCode == "2")
                    return (Results.Failed, "Invalid Mobile Number", null);
                else if (ResultCode == "3")
                    return (Results.Failed, "Mobile Number already exist", null);
                else if (ResultCode == "4")
                    return (Results.Failed, "You are already Member of this Group", null);
                else if (ResultCode == "5")
                    return (Results.Failed, "Username already exist", null);
            }
            return (Results.Null, null, null);
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

    }
}
