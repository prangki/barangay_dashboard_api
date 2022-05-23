using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Comm.Commons.Extensions;
using Infrastructure.Repositories;
using webapi.Commons.AutoRegister;
using webapi.App.Aggregates.Common;
using webapi.App.Model.User;
using webapi.App.Globalize.Company;

using Newtonsoft.Json;
using webapi.Services.Firebase;
using Comm.Commons.Advance;

using webapi.App.RequestModel.Common;
using webapi.App.RequestModel.SubscriberApp.Features.Coordinator;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Features.UserFeature;

namespace webapi.App.Aggregates.SubscriberAppAggregate.Features
{
    [Service.ITransient(typeof(CoordinatorRepository))] 
    public interface ICoordinatorRepository
    {
        Task<(Results result, String message)> InvitationAsync(InvitationRequest request);
        Task<(Results result, object items)> RequestingCreditsAsync(FilterRequest filter);
        Task<(Results result, object summary, object items)> ApprovedCreditAsync(FilterRequest filter);
        Task<(Results result, String message, object data)> CreditApprovalAsync(RequestingCreditRequest request);
        Task<(Results result, String message)> CreditPaidAsync(RequestingCreditRequest request);
        Task<(Results result, object ledger)> CommissionLedgerAsync(FilterRequest filter);        
        Task<(Results result, object items)> SubscribersAsync(FilterRequest filter);
    } 

    public class CoordinatorRepository : ICoordinatorRepository
    {
        private readonly ISubscriber _identity;
        private readonly IRepository _repo;
        public STLAccount account { get{ return _identity.AccountIdentity(); } } 
        public CoordinatorRepository(ISubscriber identity, IRepository repo){
            _identity = identity; 
            _repo = repo; 
        }
        public async Task<(Results result, String message)> InvitationAsync(InvitationRequest request){
            var form = request.RequestForm;
            var result = _repo.DSpQuery<dynamic>("dbo.spfn_BCAS0RC1I", new Dictionary<string, object>(){
                { "parmsssid", account.SessionID },
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                //
                { "parmusrmobno", form.MobileNumber },
                { "parmusrfnm", form.Firstname },
                { "parmusrlnm", form.Lastname },
                { "parmemladd", form.EmailAddress },
                { "parmcmssn", form.SharedCommission },
            }).FirstOrDefault();
            if(result != null){
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str(); 
                if(ResultCode == "1")
                    return (Results.Success, "Account successfully added to registration queue. please inform your player to complete registration within 30 minutes"); 
                else if(ResultCode == "2")
                    return (Results.Failed, String.Format("Mobile number {0} is already registered!", form.MobileNumber)); 
                return (Results.Failed, "Server encounter error while processing your request. Please try again later"); 
            }
            return (Results.Null, null); 
        }


        public async Task<(Results result, object items)> RequestingCreditsAsync(FilterRequest filter){
            var results = _repo.DSpQueryMultiple("dbo.spfn_BHABDBBEA0A", new Dictionary<string, object>(){
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmstat", filter.Status },
                { "parmpaymthd", filter.Method }, 
                { "parmftrns", filter.BaseFilter },
            }); 
            if(results != null)
                return (Results.Success, CreditDto.FilterRequestingCredits(account, results.Read(), filter.Status.Equals("1"), 25)); 
            return (Results.Null, null); 
        }

        public async Task<(Results result, object summary, object items)> ApprovedCreditAsync(FilterRequest filter){
            var results = _repo.DSpQueryMultiple("dbo.spfn_BHABDBBEA0B", new Dictionary<string, object>(){
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmactid", filter.Search },
                { "parmstat", filter.Status },
                { "parmpaymthd", filter.Method }, 
                { "parmftrns", filter.BaseFilter },
            }); 
            if(results != null)
                return (Results.Success, CreditDto.ApprovedSummary(results.ReadSingleOrDefault()), CreditDto.FilterRequestingCredits(account, results.Read(), filter.Status.Equals("1"), 25)); 
            return (Results.Null, null, null); 
        }

        public async Task<(Results result, String message, object data)> CreditApprovalAsync(RequestingCreditRequest request){
            var transaction = request.Transaction;
            var results= _repo.DSpQueryMultiple("dbo.spfn_BHAS0CS1RL2AR", new Dictionary<string, object>(){
                { "parmsssid", account.SessionID },
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                //
                { "parmpono", transaction.ReferenceID },
                { "parmactid", transaction.AccountID },
                { "parmreqstat", (request.IsApproved?"1":"2") },
                { "parmbaltyp", (request.IsCash?"1":"2") },
            });
            if(results != null){
                var row = ((IDictionary<string, object>)results.ReadSingleOrDefault());
                string ResultCode = row["RESULT"].Str(); 
                if(ResultCode == "1"){ 
                    if(request.IsApproved && row["NOTIFY"].Str().Equals("1"))
                        await notifySubscriber(account, row["SUBSCR_ID"].Str(), "credit-approval", results.Read());
                    return (Results.Success, (request.IsApproved?"Request Approved!":"Request Cancelled!"), CreditDto.RequestCreditApproval(row)); 
                }else if(ResultCode == "2")
                    return (Results.Failed, "Transaction already done!", null); 
                return (Results.Failed, "Server encounter error while processing your request. Please try again later", null); 
            }
            return (Results.Null, null, null); 
        }
        public async Task<(Results result, String message)> CreditPaidAsync(RequestingCreditRequest request){
            var transaction = request.Transaction;
            var result = _repo.DSpQuery<dynamic>("dbo.spfn_BHAS0CS1PD", new Dictionary<string, object>(){
                { "parmsssid", account.SessionID },
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                //
                { "parmactid", transaction.AccountID },
                { "parmpono", transaction.ReferenceID },
            }).FirstOrDefault();
            if(result != null){
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str(); 
                if(ResultCode == "1")
                    return (Results.Success, "Change Successfull!"); 
                return (Results.Failed, "Failed to applied changes! Maybe your request is already done"); 
            }
            return (Results.Null, null); 
        }

        
        public async Task<(Results result, object ledger)> CommissionLedgerAsync(FilterRequest filter){
            var results = _repo.DSpQueryMultiple("dbo.spfn_BGBACABDB0A", new Dictionary<string, object>(){
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmdtfrom", filter.From },
                { "parmdtto", filter.To },
                { "parmovrall", (filter.IsOverall?"1":"0") },
                { "parmftrns", filter.BaseFilter },
            });
            if(results != null)
                return (Results.Success, LedgerDto.FilterCommissionLedgers(results.Read<dynamic>(), 50)); 
            return (Results.Null, null); 
        }

        public async Task<(Results result, object items)> SubscribersAsync(FilterRequest filter){
            var results = _repo.DSpQueryMultiple("dbo.spfn_BDBBEABFA0A", new Dictionary<string, object>(){
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmsrch", filter.Search },
                { "parmfid", filter.BaseFilter },
            });
            if(results != null)
                return (Results.Success, SubscriberDto.SearchSubscribers(account, results.Read<dynamic>(), 25)); 
            return (Results.Null, null); 
        }
        //
        private static async Task<bool> notifySubscriber(STLAccount account, string subscriberID, string type, IEnumerable<dynamic> list){
            await Pusher.PushAsync($"/{account.PL_ID}/{account.PGRP_ID}/{subscriberID}/notify"
                ,new{ type = type, content = NotificationDto.Notifications(list) });
            return true;
        }
    }
}