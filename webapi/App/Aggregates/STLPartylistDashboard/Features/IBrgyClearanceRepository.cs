using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using Comm.Commons.Extensions;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using Infrastructure.Repositories;
using webapi.App.Model.User;
using webapi.Commons.AutoRegister;
using webapi.App.Features.UserFeature;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.Common.Dto;
using webapi.App.STLDashboardModel;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(BrgyClearanceRepository))]
    public interface IBrgyClearanceRepository
    {
        Task<(Results result, String message, String brgyclrid, String cntrlno)> BrgyClearanceAsync(BrgyClearance req, bool isUpdate = false);
        Task<(Results result, object bryclrid)> Load_BrgyClearance(BrgyClearance req);
        Task<(Results result, String message, object release)> ReleaseAsync(BrgyClearance req);
        Task<(Results result, String message, object cancel)> CancellAsync(BrgyClearance req);
        Task<(Results result, String message)> ReceivedBrgyClearanceRequestAsync(BrgyClearance req);
        Task<(Results result, String message)> ProcessRecivedBrgyClearanceRequestAsync(BrgyClearance req);
    }
    public class BrgyClearanceRepository:IBrgyClearanceRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public BrgyClearanceRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message, string brgyclrid, string cntrlno)> BrgyClearanceAsync(BrgyClearance req, bool isUpdate = false)
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BRGYCLR0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmbrgyclrid", req.ClearanceNo },
                {"parmcontrolno", req.ControlNo },
                {"parmuserid", req.UserID },
                {"parmtypeclearanceid",req.TypeofClearance },
                {"parmpurposeid",req.Purpose },
                {"parmorno",req.ORNumber },
                {"parmamountpaid",req.AmountPaid },
                {"parmdocstamp",req.DocStamp },


                {"parmissueddate",req.IssuedDate },
                {"parmexpirydate",req.ExpiryDate },
                {"parmvalidity",req.MosValidity },

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
                        req.ClearanceNo = row["BRGYCLR_ID"].Str();
                        req.ControlNo = row["CNTRL_NO"].Str();
                    }
                    return (Results.Success, "Clearance succesfully save!", req.ClearanceNo, req.ControlNo);
                }
                else if (ResultCode == "0")
                    return (Results.Failed, "Check your Data, Please try again!", null, null);
            }
            return (Results.Null, null, null, null);
        }

        public async Task<(Results result, object bryclrid)> Load_BrgyClearance(BrgyClearance req)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYCLR0C", new Dictionary<string, object>()
            {
                {"parmplid", req.PLID},
                {"parmpgrpid",req.PGRPID },
                {"parmuserid",req.UserID },
                {"parmrequeststatus", req.StatusRequest },
            });
            if (results != null)
                return (Results.Success, SubscriberDto.GetBrygClearanceList(results, 100));
            //return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message, object release)> ReleaseAsync(BrgyClearance req)
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BRGYCLR0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmbrgyclrid", req.ClearanceNo },
                {"parmcontrolno", req.ControlNo },
                {"parmoptrid",account.USR_ID }
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    return (Results.Success, "Clearance succesfully released !", results);
                }
                else if (ResultCode == "0")
                    return (Results.Failed, "Check your Data, Please try again!", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, string message, object cancel)> CancellAsync(BrgyClearance req)
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BRGYCLR0D", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmbrgyclrid", req.ClearanceNo },
                {"parmcontrolno", req.ControlNo },
                {"parmoptrid",account.USR_ID }
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    return (Results.Success, "Selected Item succesfully cancelled", results);
                }
                else if (ResultCode == "0")
                    return (Results.Failed, "Check your Data, Please try again!", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, string message)> ReceivedBrgyClearanceRequestAsync(BrgyClearance req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BRGYCLR0E", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmbrgyclearanceid",req.ClearanceID },
                {"parmdateappointment", req.AppointmentDate },
                {"parmoptrid",account.USR_ID },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> ProcessRecivedBrgyClearanceRequestAsync(BrgyClearance req)
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BRGYCLR0F", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },

                {"parmbrgyclrid", req.ClearanceNo },
                {"parmcontrolno", req.ControlNo },
                {"parmuserid", req.UserID },
                {"parmtypeclearanceid",req.TypeofClearance },
                {"parmpurposeid",req.Purpose },
                {"parmorno",req.ORNumber },
                {"parmamountpaid",req.AmountPaid },
                {"parmdocstamp",req.DocStamp },


                {"parmissueddate",req.IssuedDate },
                {"parmexpirydate",req.ExpiryDate },
                {"parmvalidity",req.MosValidity },

                {"parmenablectc",req.EnableCommunityTax },
                {"parmctcno",req.CTCNo },
                {"parmctcissuedate",req.CTCIssuedAt },
                {"parmctcissuedon",req.CTCIssuedOn },

                {"parmoptrid",account.USR_ID }
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    return (Results.Success, "Clearance succesfully save!");
                }
                else if (ResultCode == "0")
                    return (Results.Failed, "Check your Data, Please try again!");
            }
            return (Results.Null, null);
        }
    }
}
