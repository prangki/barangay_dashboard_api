﻿using System;
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
using webapi.App.Aggregates.Common.Dto;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(BarangaySignatoryRepository))]
    public interface IBarangaySignatoryRepository
    {
        Task<(Results result, object signatory)> LoadSignatory();
        Task<(Results result, object signatory)> LoadSignatoryA();
        Task<(Results result, String message)> SignatoryAsync(BrgySignatory request);

        Task<(Results result, String message)> SignatoryAsync02(BarangaySignatures signatory);
        Task<(Results result, Object sig)> Load_Signature(BarangaySignatory req);
        Task<(Results result, Object col)> Load_Col_Signature();
    }
    public class BarangaySignatoryRepository:IBarangaySignatoryRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public BarangaySignatoryRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object signatory)> LoadSignatory()
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
        public async Task<(Results result, object signatory)> LoadSignatoryA()
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGY_SIGNATORY0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
            });
            if (result != null)
                //return (Results.Success, result);
                return (Results.Success, STLSubscriberDto.GetAllBarangaySignatoryList(result, "", 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> SignatoryAsync(BrgySignatory request)
        {
            var results = _repo.DSpQueryMultiple("dbo.spfn_BRGY_SIGNATORY", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID },
                {"parmpgrpid", account.PGRP_ID },

                {"parmbrgycaptain", request.Brgy_Captain },
                {"parmbrgycaptain_sig", request.Brgy_Captain_sig_URL },
                {"parmcaptevent", request.Capt_Events },

                {"parmfirstbrgycouncilor", request.Brgy_FirstCouncilor },
                {"parmfirstbrgycouncilor_sig", request.Brgy_FirstCouncilor_sig_URL },
                {"parmfirstevent", request.First_Events },

                {"parmsecondbrgycouncilor", request.Brgy_SecondCouncilor },
                {"parmsecondbrgycouncilor_sig", request.Brgy_SecondCouncilor_sig_URL },
                {"parmsecondevent", request.Second_Events },

                {"parmthirdbrgycouncilor", request.Brgy_ThirdCouncilor },
                {"parmthirdbrgycouncilor_sig", request.Brgy_ThirdCouncilor_sig_URL },
                {"parmthirdevent", request.Third_Events },

                {"parmfourthbrgycouncilor", request.Brgy_FourthCouncilor },
                {"parmfourhtbrgycouncilor_sig", request.Brgy_FourthCouncilor_sig_URL },
                {"parmfourthevent", request.Fourth_Events },

                {"parmfifthbrgycouncilor", request.Brgy_FifthCouncilor },
                {"parmfifthbrgycouncilor_sig", request.Brgy_FifthCouncilor_sig_URL },
                {"parmfifthevent", request.Fifth_Events },

                {"parmsixthbrgycouncilor", request.Brgy_SixthCouncilor },
                {"parmsixthbrgycouncilor_sig", request.Brgy_SixthCouncilor_sig_URL },
                {"parmsixthevent", request.Sixth_Events },

                {"parmseventhbrgycouncilor", request.Brgy_SeventhCouncilor },
                {"parmseventhbrgycouncilor_sig", request.Brgy_SeventhCouncilor_sig_URL },
                {"parmseventhevent", request.Seventh_Events },

                {"parmskchairman", request.SK_Chairman },
                {"parmskchairman_sig", request.SK_Chairman_sig_URL },
                {"parmskchairmanevent", request.SKChairman_Events },

                {"parmbrgytreasurer", request.Brgy_Treasurer },
                {"parmbrgytreasurer_sig", request.Brgy_Treasurer_sig_URL },
                {"parmtreasurerevent", request.Treasurer_Events },

                {"parmbrgysecretary", request.Brgy_Secretary },
                {"parmbrgysecretary_sig", request.Brgy_Secretary_sig_URL },
                {"parmsecretaryevent", request.Secretary_Events },

                {"parmbrgychieftanod ", request.Brgy_Chief_Tanod },
                {"parmbrgychieftanod_sig", request.Brgy_Chief_Tanod_sig_URL },
                {"parmchieftanodevent", request.Tanod_Events },

                {"parmoptrid",account.USR_ID },
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully saved!");
                else if (ResultCode == "0")
                    return (Results.Failed, "Something wrong in your data, Please try again!");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> SignatoryAsync02(BarangaySignatures signatory)
        {
            var results = _repo.DSpQueryMultiple("dbo.spfn_BRGY_SIGNATORY02", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID },
                {"parmpgrpid", account.PGRP_ID },

                {"paramxml",signatory.isignatories},
                {"parmusrid",account.USR_ID},

            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Succesfully saved");
                else if (ResultCode == "0")
                    return (Results.Failed, "Something wrong in your data, Please try again");
            }
            return (Results.Null, null);
        }


        public async Task<(Results result, object sig)> Load_Signature(BarangaySignatory req)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYSIG0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmcolumname",req.ColName }
            });
            if (result != null)
                return (Results.Success, SubscriberDto.GetSignatoryList(result,100));
            return (Results.Null, null);
        }

        public async Task<(Results result, object col)> Load_Col_Signature()
        {
            var result = _repo.DQuery<dynamic>($"dbo.spnf_COLSIG0A");
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }
    }
}
