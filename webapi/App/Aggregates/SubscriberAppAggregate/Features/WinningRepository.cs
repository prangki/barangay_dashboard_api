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
using webapi.App.RequestModel.Common;
using webapi.App.RequestModel.SubscriberApp.Features.Winning;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.Services.Firebase;
using webapi.App.Features.UserFeature;

namespace webapi.App.Aggregates.SubscriberAppAggregate.Features
{
    [Service.ITransient(typeof(WinningRepository))] 
    public interface IWinningRepository
    {
        Task<(Results result, String message)> ClaimingAsync(ClaimingRequest request);
        Task<(Results result, object items)> ProcessedAsync(FilterRequest filter);
        Task<(Results result, object items)> CompletedAsync(FilterRequest filter);
        Task<(Results result, object items)> RemittancesAsync();
    }

    public class WinningRepository : IWinningRepository
    {
        private readonly ISubscriber _identity;
        private readonly IRepository _repo;
        public STLAccount account { get{ return _identity.AccountIdentity(); } } 
        public WinningRepository(ISubscriber identity, IRepository repo){
            _identity = identity; 
            _repo = repo; 
        }

        public async Task<(Results result, String message)> ClaimingAsync(ClaimingRequest request){
            dynamic result = _repo.DSpQuery<dynamic>("dbo.spfn_BHBLW0CW", new Dictionary<string, object>(){
                { "parmsssid", account.SessionID },
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },

                { "parmtransno", request.ReferenceID },
                //{ "parmamount", request.Amount  }, 
                { "parmencashtyp", request.TypeCode  }, 
                { "parmencashcom", request.Remittance  }, 
                { "parmname", request.Name },
                { "parmnumber", request.Number },
                { "parmaddr", (request.Address??"") },
            }).FirstOrDefault();
            if(result != null){
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if(ResultCode == "1"){
                    bool IsAutoConvert = (row["S_AUTO"].Str()=="1");
                    String message = "Your encashment successfully submitted with Amount {0}. Your Encashment Transaction No is {1}. We are working processing your request and you will received notification from app once the encashment is done.";
                    if(request.IsConvertToGameCredit){
                        if(IsAutoConvert) message = "Your request converting game credit successfully converted with Amount {0} with Transaction No {1}.";
                        else message = "Your request converting game credit successfully submitted with Amount {0}. Your Request Transaction No is {1}. We are working processing your request and you will received notification from app once the request is done.";
                    }
                    /*if(IsAutoConvert){
                        await notifyAutoConvertClaimWinnings(row["TRN_NO"].Str(), request.IsConvertToGameCredit);
                    }*/
                    return (Results.Success, String.Format(message, request.Amount.ToAccountingFormat("0.00"), row["TRN_NO"].Str())); 
                }
                return (Results.Failed, "An error encountered while processing your request. please try again"); 
            }
            return (Results.Null, null); 
        }

        public async Task<(Results result, object items)> ProcessedAsync(FilterRequest filter){
            var results = _repo.DSpQueryMultiple($@"dbo.spfn_BHBACA0A", new Dictionary<string, object>(){
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmftrns", filter.BaseFilter },
            });
            if(results != null)
                return (Results.Success, WinningDto.FilterEncashOnProcesses(results.Read(), 25));
            return (Results.Null, null); 
        }

        public async Task<(Results result, object items)> CompletedAsync(FilterRequest filter){
            var results = _repo.DSpQueryMultiple($@"dbo.spfn_BHBACA0B", new Dictionary<string, object>(){
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmftrns", filter.BaseFilter },
            });
            if(results != null)
                return (Results.Success, WinningDto.FilterEncashCompletions(results.Read(), 25));
            return (Results.Null, null); 
        }
        public async Task<(Results result, object items)> RemittancesAsync(){
            var results = _repo.DSpQueryMultiple($@"dbo.spfn_ACAACB0A", new Dictionary<string, object>(){
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
            });
            if(results != null)
                return (Results.Success, WinningDto.Remittances(results.Read<dynamic>()));
            return (Results.Null, null); 
        }
    }
}