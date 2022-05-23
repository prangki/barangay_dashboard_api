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

using webapi.App.RequestModel.SubscriberApp.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.Common;
using webapi.App.Features.UserFeature;

namespace webapi.App.Aggregates.SubscriberAppAggregate.Features
{
    [Service.ITransient(typeof(CreditServiceRepository))] 
    public interface ICreditServiceRepository
    {
        Task<(Results result, String message)> BuyCreditAsync(BuyCreditRequest request);
        Task<(Results result, String message)> PasaCreditAsync(PasaCreditRequest request);
        Task<(Results result, object ledger)> CreditLedgerAsync(FilterRequest filter);
    }

    public class CreditServiceRepository : ICreditServiceRepository
    {
        private readonly ISubscriber _identity;
        private readonly IRepository _repo;
        public STLAccount account { get{ return _identity.AccountIdentity(); } } 
        public CreditServiceRepository(ISubscriber identity, IRepository repo){
            _identity = identity; 
            _repo = repo; 
        }

        public async Task<(Results result, String message)> BuyCreditAsync(BuyCreditRequest request){
            var results = _repo.DSpQueryMultiple("dbo.spfn_BHAS0CS1RL", new Dictionary<string, object>(){
                { "parmsssid", account.SessionID },
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmamnt", request.Amount },
            });
            if(results != null){
                var row = ((IDictionary<string, object>)results.ReadSingleOrDefault());
                string ResultCode = row["RESULT"].Str();
                if(ResultCode == "1"){
                    if(row["NOTIFY"].Str().Equals("1"))
                        await notifySubscriber(account, row["SUBSCR_ID"].Str(), "request-credit", results.Read());
                    return (Results.Success, "Your request has been successfully submitted"); 
                }
                return (Results.Failed, "An error encountered while processing your request. please try again"); 
            }
            return (Results.Null, null); 
        }


        public async Task<(Results result, String message)> PasaCreditAsync(PasaCreditRequest request){
            var results = _repo.DSpQueryMultiple("dbo.spfn_BHAS0PA1CS", new Dictionary<string, object>(){
                { "parmsssid", account.SessionID },
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                //
                { "parmusrnm", request.Subscriber },
                { "parmamnt", request.Amount },
            });
            if(results != null){
                var row = ((IDictionary<string, object>)results.ReadSingleOrDefault());
                string ResultCode = row["RESULT"].Str();
                if(ResultCode == "1"){
                    if(row["NOTIFY"].Str().Equals("1"))
                        await notifySubscriber(account, row["SUBSCR_ID"].Str(), "load-credit", results.Read());
                    return (Results.Success, String.Format("Send successfully to '{0}' with worth credit of {1}", request.Subscriber, request.Amount.ToString("0.00") )); 
                }else if(ResultCode == "72")
                    return (Results.Failed, "Account doesn't exist"); 
                else if(ResultCode == "73")
                    return (Results.Failed, "Account doesn't exist"); 
                else if(ResultCode == "21")
                    return (Results.Failed, "You cannot share to your own account!"); 
                else if(ResultCode == "22")
                    return (Results.Failed, "Failed sending credit to this account!"); 
                else if(ResultCode == "23")
                    return (Results.Failed, "Your account has been blocked"); 
                else if(ResultCode == "24")
                    return (Results.Failed, "Account has been blocked"); 
                else if(ResultCode == "62")
                    return (Results.Failed, "Insufficient Balance"); 
                return (Results.Failed, "An error encountered while processing your request. please try again"); 
            }
            return (Results.Null, null); 
        }
        
        public async Task<(Results result, object ledger)> CreditLedgerAsync(FilterRequest filter){
            var results = _repo.DSpQueryMultiple("dbo.spfn_BGAACABDB0A", new Dictionary<string, object>(){
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmdtfrom", filter.From },
                { "parmdtto", filter.To },
                { "parmovrall", (filter.IsAll?"1":"0") },
                { "parmftrns", filter.BaseFilter },
            });
            if(results != null)
                return (Results.Success, LedgerDto.FilterCreditLedgers(results.Read<dynamic>(), 50)); 
            return (Results.Null, null); 
        }
        private static async Task<bool> notifySubscriber(STLAccount account, string subscriberID, string type, IEnumerable<dynamic> list){
            await Pusher.PushAsync($"/{account.PL_ID}/{account.PGRP_ID}/{subscriberID}/notify"
                ,new{ type = type, content = NotificationDto.Notifications(list) });
            return true;
        }
    }
}