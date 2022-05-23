using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Comm.Commons.Extensions;
using Infrastructure.Repositories;
using webapi.Commons.AutoRegister;
using webapi.App.Aggregates.Common;
using webapi.App.RequestModel.SubscriberApp;
using webapi.App.RequestModel.SubscriberApp.Common;
using webapi.App.Model.User;
using webapi.App.Globalize.Company;

using Newtonsoft.Json;
using System.IO;
using System.Net;
using webapi.App.RequestModel.SubscriberApp.ActiveAccount;
using webapi.Services.Firebase;
using Comm.Commons.Advance;

using webapi.App.RequestModel.SubscriberApp.Features;
using webapi.App.RequestModel.Common;
using webapi.App.Features.UserFeature;

using webapi.App.RequestModel.SubscriberApp.Features.LotteryGame;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;

namespace webapi.App.Aggregates.SubscriberAppAggregate.Features
{
    [Service.ITransient(typeof(LotteryGameRepository))] 
    public interface ILotteryGameRepository
    {
        Task<(Results result, object item)> ProfileSettingsAsync();
        //Task<(Results result, object item)> GameSettingsAsync();
        Task<(Results result, String message, object Tickets)> PlaceBetAsync(PlaceBetRequest request);
        Task<(Results result, object tickets)> BetHistoryAsync(FilterRequest filter);
        Task<(Results result, object items)> ResultsAsync(FilterRequest filter);
        Task<(Results result, object items)> WinningResultsAsync(FilterRequest filter);
        Task<(Results result, String  message, object Ticket)> ReprintLogsAsync(String TransactionNo);
    }

    public class LotteryGameRepository : ILotteryGameRepository
    {
        private readonly ISubscriber _identity;
        private readonly IRepository _repo;
        public STLAccount account { get{ return _identity.AccountIdentity(); } } 
        public LotteryGameRepository(ISubscriber identity, IRepository repo){
            _identity = identity; 
            _repo = repo; 
        }
        public async Task<(Results result, object item)> ProfileSettingsAsync(){
            var result = _repo.DSpQuery<dynamic>("dbo.spfn_CAA01", new Dictionary<string, object>(){
                { "parmplid", account.PL_ID },
            }).FirstOrDefault();
            if(result != null)
                return (Results.Success, LotteryGameDto.PartyListSettings(result)); 
            return (Results.Null, null);
        }
        /*public async Task<(Results result, object item)> GameSettingsAsync(){
            var results = _repo.DSpQuery<dynamic>("dbo.spfn_ABA0A", new Dictionary<string, object>(){
                { "parmcompid", account.CompanyID },
                { "parmbrcd", account.BranchID },
            }).FirstOrDefault();
            if(results != null)
                return (Results.Success, LotteryGameDto.GameSettings(results.Read())); 
            return (Results.Null, null);
        }*/

        public async Task<(Results result, String message, object Tickets)> PlaceBetAsync(PlaceBetRequest request){
            var placebets = request.PlaceBets;
            var keys = placebets.Keys.ToArray();
            //List<dynamic> comms = new List<dynamic>();
            Dictionary<string, object> commissions = new Dictionary<string, object>();
            foreach(string key in keys){
                var placebet = placebets[key];
                if(!placebet.Status.Equals("ok")) continue;
                var bets = placebet.Bets;
                var input = AggrUtils.Xml.toXmlString(bets.Select(dg=>dg.ToXml())).Str().Trim();
                var result = _repo.DSpQuery<dynamic>("dbo.spfn_1CALG9PB", new Dictionary<string, object>(){
                    { "parmsssid", account.SessionID },
                    { "parmcompid", account.PL_ID },
                    { "parmbrcd", account.PGRP_ID },
                    { "parmuserid", account.USR_ID },
                    //{ "parmgeoloc", "" }, 
                    { "parmgmtyp", placebet.GameType }, 
                    { "parminput", input }, 
                    //{ "parmtrmnl", (account.IsTerminal?"1":"0") },
                    //{ "parmusrmobno", (account.IsTerminal?request.MobileNumber:"") }
                }).FirstOrDefault();
                if(result != null){
                    var row = ((IDictionary<string, object>)result);
                    var ResultCode = row["RESULT"].Str();
                    if((ResultCode=="1")){
                        //await Pusher.PushAsync($"/{account.CompanyID}/{account.BranchID}/lottery/bet/update" , "{}");
                        string managerid = row["MNGR_ID"].Str(), supervisorid = row["SPVR_ID"].Str();
                        if(!managerid.IsEmpty()) commissions[managerid] = (commissions.GetValue(managerid).To<double>(false) + row["MNGR_ADDCOMM"].Str().ToDecimalDouble());
                        if(!supervisorid.IsEmpty()) commissions[supervisorid] = (commissions.GetValue(supervisorid).To<double>(false) + row["SPVR_ADDCOMM"].Str().ToDecimalDouble());
                        placebet.Message = "Your Bet Successfully Posted";
                        placebet.TransactionNo = row["TRN_NO"].Str();
                    }else{
                        placebet.Status = "error";
                        if(ResultCode=="9") placebet.Message = "Game not active!";
                        else if(ResultCode=="3") placebet.Message = String.Format("Bets not accepted! Cut-off {0} draw", row["CutOff"].Str());
                        else if(ResultCode=="2") placebet.Message = "Game not yet posted! please wait for the result";
                        else if(ResultCode=="71") placebet.Message = "Game not active!";
                        else if(ResultCode=="72") placebet.Message = "Game not applicable!";
                        else if(ResultCode=="51") placebet.Message = "Insufficient Balance";
                        else placebet.Message = "Your Bet Failed to Post";
                    }
                }
            }
            
            //get Tickets
            bool hasValidPlaceBet = false;
            foreach(string key in keys){
                var placebet = placebets[key];
                if(!placebet.Status.Equals("ok")) continue;
                hasValidPlaceBet = true;
                var results = _repo.DSpQueryMultiple($@"dbo.spfn_ACAACBAAA0A", new Dictionary<string, object>(){
                    { "parmcompid", account.PL_ID },
                    { "parmbrcd", account.PGRP_ID },
                    { "parmuserid", account.USR_ID },
                    { "parmgmtyp", placebet.GameType },
                    { "parmtrnno", placebet.TransactionNo },
                });
                if(results!=null){
                    dynamic ticket = LotteryGameDto.GameSummary(results.ReadSingleOrDefault<dynamic>());
                    ticket.Draws = LotteryGameDto.GameDetails(results.Read<dynamic>()).ToList();
                    placebet.Ticket = ticket;
                }
            }
            var placebetTickets = placebets.Values.ToList();
            if(hasValidPlaceBet){
                await Pusher.PushAsync($"/{account.PL_ID}/{account.PGRP_ID}/lottery/bet/update" , "{}");
                if(commissions.Count!=0){
                    foreach(KeyValuePair<string, object> kvp in commissions){
                        if(!(kvp.Value.To<double>(false)>0)) continue;
                        await Pusher.PushAsync($"/{account.PL_ID}/{account.PGRP_ID}/{kvp.Key}/balance" , new {
                            type = "add-commission", content = kvp.Value
                        });
                    }
                }
                return (Results.Success, "Your Bet Successfully Posted", formated(placebetTickets));
            }else if(keys.Length!=0){ 
                if(placebetTickets.Count==1)
                    return (Results.Failed, placebetTickets.FirstOrDefault().Message, formated(placebetTickets));
                return (Results.Failed, "Your Bet Failed to Post", formated(placebetTickets));
            }
            return (Results.Null, null, null); 
        }
        private object formated(List<PlaceBetEntry> placebetTickets){
            return placebetTickets.Select(t=>new { 
                Ticket=t.Ticket, BetType=t.GameType, //LotteryGameDto.Static.BetType.GetValue(t.GameType.Str()),
                Message=t.Message, Status=t.Status, 
            }).ToList();
        }

        public async Task<(Results result, object tickets)> BetHistoryAsync(FilterRequest filter){
            if(filter.IsCurrent)
                return await CurrentPlaceBetAsync(filter);

            var results = _repo.DSpQueryMultiple("dbo.spfn_ACAACBAAA0B", new Dictionary<string, object>(){
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmgmtyp", filter.Type },
                { "parmovrall", (filter.IsOverall?"1":"0") },
                { "parmftrns", filter.BaseFilter },
            });
            if(results != null){
                var summaries = results.Read<dynamic>();
                if(summaries.Count()==0) return (Results.Success, summaries);
                return (Results.Success, LotteryGameDto.FilterGameSummariesWithDetails(summaries, results.Read<dynamic>(),25)); 
            }
            return (Results.Null, null); 
        }

        private async Task<(Results result, object tickets)> CurrentPlaceBetAsync(FilterRequest filter){
            var results = _repo.DSpQueryMultiple("dbo.spfn_ACAACBAAA0D", new Dictionary<string, object>(){
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmftrns", filter.BaseFilter },
            });
            if(results != null){
                var summaries = results.Read<dynamic>();
                if(summaries.Count()==0)
                    return (Results.Success, summaries);
                return (Results.Success, LotteryGameDto.FilterGameSummariesWithDetails(summaries, results.Read<dynamic>(),25)); 
            }
            return (Results.Null, null); 
        }

        /*private async Task<(Results result, object tickets)> CurrentPlaceBetAsync(){
            var results = _repo.DSpQueryMultiple("dbo.spfn_ACAACBAAA0C", new Dictionary<string, object>(){
                { "parmcompid", account.CompanyID },
                { "parmbrcd", account.BranchID },
                { "parmuserid", account.SubscriberID },
            });
            if(results != null){
                var summaries = results.Read<dynamic>();
                if(summaries.Count()==0)
                    return (Results.Success, summaries);
                return (Results.Success, LotteryGameDto.GameSummariesWithDetails(summaries, results.Read<dynamic>())); 
            }
            return (Results.Null, null); 
        }*/

        public async Task<(Results result, object items)> ResultsAsync(FilterRequest filter){
            if(filter.IsCurrent)
                return await CurrentResultsAsync(filter);

            var results = _repo.DSpQueryMultiple("dbo.spfn_ADBAAA0A", new Dictionary<string, object>(){
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmgmtyp", filter.Type },
                { "parmovrall", (filter.IsOverall?"1":"0") },
                { "parmftrns", filter.BaseFilter },
            });
            if(results != null)
                return (Results.Success, LotteryGameDto.FilterGameResults(results.Read<dynamic>(), 25)); 
            return (Results.Null, null); 
        }

        public async Task<(Results result, object items)> CurrentResultsAsync(FilterRequest filter){
            var results = _repo.DSpQueryMultiple("dbo.spfn_ADBAAA0E", new Dictionary<string, object>(){
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmgmtyp", filter.Type },
                { "parmovrall", (filter.IsOverall?"1":"0") },
                { "parmftrns", filter.BaseFilter },
            });
            if(results != null)
                return (Results.Success, LotteryGameDto.FilterGameResults(results.Read<dynamic>(), 25)); 
            return (Results.Null, null); 
        }

        public async Task<(Results result, object items)> WinningResultsAsync(FilterRequest filter){
            var results = _repo.DSpQueryMultiple($@"dbo.spfn_BHBACAACB0A", new Dictionary<string, object>(){
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmftrns", filter.BaseFilter },
            });
            if(results != null){
                var winnings = results.Read<dynamic>();
                if(winnings.Count()==0) return (Results.Success, winnings);
                return (Results.Success, LotteryGameDto.FilterGameWinningsWithSummaryDetails(winnings, results.Read<dynamic>(), results.Read<dynamic>(), 25)); 
            }
            return (Results.Null, null); 
        }

        public async Task<(Results result, String  message, object Ticket)> ReprintLogsAsync(String TransactionNo){
            var results = _repo.DSpQueryMultiple("dbo.spfn_1C1LG0RPT", new Dictionary<string, object>(){
                { "parmsssid", account.SessionID },
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmtransno", TransactionNo }, 
            });
            if(results != null){
                var summary = results.Read();
                if(summary.Count() == 1)
                    return (Results.Success, null, LotteryGameDto.GameSummariesWithDetails(summary, results.Read()).FirstOrDefault()); 
                return (Results.Failed, "Invalid to reprint ticket", null);
            }
            return (Results.Null, null, null); 
        }
    }
}