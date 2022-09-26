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
    [Service.ITransient(typeof(BusinessRepository))]
    public interface IBussinessRepository
    {
        Task<(Results result, String message, object biz)> BussinessAsync(BusinesseRequest request);
        Task<(Results result, String message)> UpdateBussinessAsync(BusinesseRequest request);
        Task<(Results result, object business)> Load_RegisteredBusiness(FilterRequest request);
        Task<(Results result, object dochistory)> Load_BusinessDocHistory(FilterRequest request);
        Task<(Results result, object type)> LoadTYpe();
        Task<(Results result, String message, String bizownrshptypid)> BusinessOwnershipTypeAsync(BusinessOwnershipType request, bool isEdit = false);
        Task<(Results result, object bizowrnshptyp)> Load_BusinessOwnershipType();
        Task<(Results result, String message)> RemoveBusinessOwnershiptype(BusinessOwnershipType request);
    }
    public class BusinessRepository:IBussinessRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public BusinessRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, String message , object biz)> BussinessAsync(BusinesseRequest request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIZ0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmregisterno",request.RegisteredNo },
                {"parmnatureofbusiness",request.NatureofBusiness },
                {"parmbizname",request.BusinessName },
                {"parmbizemail",request.Email },
                {"parmcontactno",request.ContactNo },
                {"parmbizaddress",request.BusinessAddress },
                {"parmdateoperate",request.DateOperate },
                {"parmownershiptype",request.BusinessOwnershipTypeID },
                {"parmownerid",request.OwnerID },
                {"parmbizstatus",request.BizStatus },
                
                //{"parmfirstname",request.FirstName },
                //{"parmminm",request.MiddleInitial },
                //{"parmlastname",request.LastName },
                //{"parmowneraddress",request.OwnerAddress },
                //{"parmownemail",request.OwnerEmail },
                //{"parmowncontactno",request.OwnerContactNo },
                {"parmoptrid",account.USR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save", result);
                else if (ResultCode == "2")
                    return (Results.Failed, "Business Name already Exist, Please try again", null);
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Business Details, Please try again", null);
                return (Results.Failed, "Something wrong in your data, Please try again", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, string message)> UpdateBussinessAsync(BusinesseRequest request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIZ0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmbizid",request.BussinessID },
                {"parmbizonctrolno",request.ControlNo },

                {"parmregisterno",request.RegisteredNo },
                {"parmnatureofbusiness",request.NatureofBusiness },
                {"parmbizname",request.BusinessName },
                {"parmbizemail",request.Email },
                {"parmcontactno",request.ContactNo },
                {"parmbizaddress",request.BusinessAddress },
                {"parmdateoperate",request.DateOperate },
                {"parmownershiptype",request.BusinessOwnershipTypeID },
                {"parmownerid",request.OwnerID },
                {"parmbizstatus",request.BizStatus },

                {"parmoptrid",account.USR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "2")
                    return (Results.Failed, "Business Name already Exist, Please try again");
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Business Details, Please try again");
                return (Results.Failed, "Something wrong in your data, Please try again");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object type)> LoadTYpe()
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BIZTYP", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object business)> Load_RegisteredBusiness(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIZOC", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmrownum", request.num_row},
                {"parmsearch", request.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllRegisterBusinessList(result.Read<dynamic>(), request.Userid, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, object dochistory)> Load_BusinessDocHistory(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSBIZDOC", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmbizid",request.BusinessId },
                {"parmdatefrom", request.FromDt},
                {"parmdateto", request.ToDt},
                {"parmstatus", request.Status},
                {"parmrownum", request.num_row}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllBusinessDocumentRequestList(result.Read<dynamic>(), request.Userid, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, String message, String bizownrshptypid)> BusinessOwnershipTypeAsync(BusinessOwnershipType request, bool isEdit = false)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIZTYPOWRNSHP0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmbusinesstypeownershipid", request.BusinessOwnershiptypeID },
                {"parmdescription", request.BusinessOwnershipDescription },
                {"parmoptrid",account.USR_ID },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    if (!isEdit)
                        request.BusinessOwnershiptypeID = row["BIZTYPOWNRSHP_ID"].Str();
                    return (Results.Success, "Successfully save", request.BusinessOwnershiptypeID);
                }

                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again", null);
                else if (ResultCode == "3")
                    return (Results.Failed, "Business Ownership Type already exist, Please try again", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, object bizowrnshptyp)> Load_BusinessOwnershipType()
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIZTYPOWRNSHP0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetBusinessOwnerTypeList(result.Read<dynamic>(), 1000));

            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> RemoveBusinessOwnershiptype(BusinessOwnershipType request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIZTYPOWRNSHP0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmpurposeid", request.BusinessOwnershiptypeID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully Remove Selected Item");

                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again");
            }
            return (Results.Null, null);
        }
    }
}
