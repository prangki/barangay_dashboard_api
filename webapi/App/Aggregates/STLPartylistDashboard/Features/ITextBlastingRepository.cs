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
using webapi.App.Aggregates.Common.Dto;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(TextBlastingRepository))]
    public interface ITextBlastingRepository
    {
        Task<(Results result, object mob)> Load_Mobile(AcctStatistic acct);
        Task<(Results result, object mob)> Load_MobileMemberAccount(TextBlastingMemberAccount acct);
        Task<(Results result, object mob)> TotalMemberAccount(TextBlastingMemberAccount acct);
        Task<(Results result, object prefix)> Load_PhilPrefix(string search);
        Task<(Results result, String message)> TextBlast(TextBlasting mob);
        Task<(Results result, String message)> TextBlast_Gen_Number(TextBlasting mob);
        Task<(Results result, String message)> IndividualText(IndividualText mob);
        Task<(Results result, String message)> GenerateTextBlast(GenerateBlasting mob);
        Task<(Results result, object mob)> GeneratedMobileNumber(int row);
        Task<(Results result, String message)> SMSReadInbox(IndividualText req);
        Task<(Results result, String message, String smsid)> SMSSendMessage(IndividualText req);
        Task<(Results result, String message)> SMSReSendMessage(IndividualText req);
        Task<(Results result, object inbox,object smsinbox)> LoadSMSInbox(IndividualText req);
    }
    public class TextBlastingRepository:ITextBlastingRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public TextBlastingRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object mob)> Load_Mobile(AcctStatistic acct)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDB0B", new Dictionary<string, object>()
            {
                {"parmpgrpid",acct.PGRP_ID }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }
        public async Task<(Results result, object mob)> Load_MobileMemberAccount(TextBlastingMemberAccount acct)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDABDBCAACBB08", new Dictionary<string, object>()
            {
                {"parmaccntid",acct.AccntID },
                {"parmrownum", acct.Row_Num },
                {"parmrownumto",acct.Row_Num_To },
                {"@parmsearch", acct.Search }
            });
            if (results != null)
                return (Results.Success, SubscriberDto.GetMobileMemberAccount(results));
            return (Results.Null, null);
        }
        public async Task<(Results result, object mob)> TotalMemberAccount(TextBlastingMemberAccount acct)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDABDBCAACBB09", new Dictionary<string, object>()
            {
                {"parmaccntid",acct.AccntID },
                {"@parmsearch", acct.Search }
            }).FirstOrDefault();
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> GenerateTextBlast(GenerateBlasting mob)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_EAA0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmprefix",mob.Prefix },
                {"paramstartnum",mob.FromNo },
                {"paramendnum",mob.ToNo },
                {"parmmessage",mob.TextMessage },
                {"parmoptrid",account.USR_ID }
            }).FirstOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully Send Message");
                else if (ResultCode == "2")
                    return (Results.Failed, "Failed Send Message");
                else
                    return (Results.Failed, "Error Send Message");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> IndividualText(IndividualText mob)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_JQAB0TB0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmoptrid",account.USR_ID },
                {"parmstxtmsg",mob.TextMessage },
                {"parmsmobs",mob.MobileNumber }
            }).FirstOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully Send Message");
                else if (ResultCode == "2")
                    return (Results.Failed, "Failed Send Message");
                else
                    return (Results.Failed, "Error Send Message");
            }
            return (Results.Null, null);
        }


        public async Task<(Results result, string message)> TextBlast_Gen_Number(TextBlasting mob)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_EAA0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmoptrid",account.USR_ID },
                {"parmmessage",mob.TextMessage },
                {"parmpmobno",mob.MobileNumber }
            }).FirstOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully Send Message");
                else if (ResultCode == "2")
                    return (Results.Failed, "Failed Send Message");
                else
                    return (Results.Failed, "Error Send Message");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> TextBlast(TextBlasting mob)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_JQAB0TB", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmoptrid",account.USR_ID },
                {"parmstxtmsg",mob.TextMessage },
                {"parmsmobs",mob.MobileNumber }
            }).FirstOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully Send Message");
                else if (ResultCode == "2")
                    return (Results.Failed, "Failed Send Message");
                else 
                    return (Results.Failed, "Error Send Message");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object prefix)> Load_PhilPrefix(string search)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_EAA01", new Dictionary<string, object>()
            {
                {"parmsearch", search }
            });
            if (result != null)
            {
                return (Results.Success, result);
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object mob)> GeneratedMobileNumber(int row)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spnf_AEC0A", new Dictionary<string, object>()
            {
                {"numrow", row }
            });
            if (result != null)
            {
                return (Results.Success, result);
            }
            return (Results.Null, null);


            //var result = _repo.DQuery<dynamic>($"dbo.spnf_AEC0A");
            //if (result != null)
            //{
            //    return (Results.Success, result);
            //}
            //return (Results.Null, null);
        }

        public async Task<(Results result, string message)> SMSReadInbox(IndividualText req)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_SMSIN0C", new Dictionary<string, object>()
            {
                {"parmmobno",req.MobileNumber },
                {"parmid",req.Id },
                {"parmsmsid", req.SMS_ID }
            }).FirstOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully Save");
                else if (ResultCode == "2")
                    return (Results.Failed, "Failed Save");
                else
                    return (Results.Failed, "Error Save");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object inbox, object smsinbox)> LoadSMSInbox(IndividualText req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_SMSIN0B", new Dictionary<string, object>()
            {
                {"parmmobno", req.MobileNumber },
                {"parmsmstype", req.SMSType },
                {"parmuserid", account.USR_ID }
            });
            if (result != null)
            {
                var inbox = STLSubscriberDto.GetInbox(result.ReadSingleOrDefault());
                var smsinbox = STLSubscriberDto.GetSMSInboxList(result.Read<dynamic>());
                return (Results.Success, inbox, smsinbox);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, string message, String smsid)> SMSSendMessage(IndividualText req)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_SMSOUT0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmmessageto",req.MobileNumber },
                {"parmmessagetext",req.TextMessage },
                {"parmessagetype", req.SMSType },
                {"parmuserid", account.USR_ID }
            }).FirstOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    req.SMS_ID = row["SMS_ID"].Str();
                    return (Results.Success, "Successfully Sent", req.SMS_ID);
                }
                    
                else if (ResultCode == "2")
                    return (Results.Failed, "Failed Sent", null);
                else
                    return (Results.Failed, "Error Sent", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, string message)> SMSReSendMessage(IndividualText req)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_SMSOUT0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmmessageto",req.MobileNumber },
                {"parmmessagetext",req.TextMessage },
                {"parmessagetype", req.SMSType },
                {"parmsmsid", req.SMS_ID },
                {"parmuserid", account.USR_ID }
            }).FirstOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully Sent");
                else if (ResultCode == "2")
                    return (Results.Failed, "Failed Sent");
                else
                    return (Results.Failed, "Error Sent");
            }
            return (Results.Null, null);
        }
    }
}
