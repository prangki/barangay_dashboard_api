using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using Comm.Commons.Extensions;
using webapi.Commons.AutoRegister;
using webapi.App.Features.UserFeature;
using webapi.App.STLDashboardModel;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.Common.Dto;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(RequestDocumentRepository))]
    public interface IRequestDocumentRepository
    {
        Task<(Results result, String message, String reqdocid)> RequestDocumentAsync(RequestDocument request);
        Task<(Results result, String message, String reqdocid, String controlno)> ResidenceRequestDocumentAsync(RequestDocument request);
        Task<(Results result, String message, String reqdocid)> RequestBrgyClearanceAsync(RequestDocument request);
        Task<(Results result, String message, String reqdocid)> UpdateRequestDocumentAsync(RequestDocument request);
        Task<(Results result, String message, String reqdocid)> UpdateRequestBrgyClearanceAsync(RequestDocument request);
        Task<(Results result, String message)> PrintRequestDocumentAsync(RequestDocument request);
        Task<(Results result, object reqdoc)> LoadRequestDocument(FilterRequest request);
        Task<(Results result, object reqdoc)> LoadIndividualRequestDocument(FilterRequest request);
        Task<(Results result, object reqdoc)> LoadIssuesConcernAttachment(RequestDocument request);
        Task<(Results result, String message)> ReceivedRequestDocument(RequestDocument request);
        Task<(Results result, object purpose)> LoadPurpose();
        Task<(Results result, object bizname)> LoadBusinessName();
        Task<(Results result, object bizowner)> LoadBusinessOwner();
        Task<(Results result, object businesstype)> LoadBusinessType(string businessname);
    }
    public class RequestDocumentRepository:IRequestDocumentRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public RequestDocumentRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message, string reqdocid)> RequestDocumentAsync(RequestDocument request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_REQ_DOC0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmdoctypeid", request.DoctypeID },
                {"parmcategorydocument", request.Category_Document },
                {"parmpapplicationdate", request.ApplicationDate },
                {"parmbizname", request.BusinessName},
                {"parmbizaddress",request.BusinessAddress },
                {"parmbizownername",request.BusinessOwnerName },
                {"parmbizowneraddress", request.BusinessOwnerAddress },
                {"parmbiztype", request.Type },
                {"parmrequestorid", request.RequestorID },
                {"parmrequestorname", request.RequestorNM },
                {"parmcategory", request.CategoryID },
                {"parmotherdocument", request.OTRDocumentType },
                {"parmxattchmnt",request.iAttachments },
                {"parmisfree", request.isFree },
                {"parmoptrid",account.USR_ID },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save", row["REQ_DOC_ID"].Str());
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again", null);
                else if (ResultCode == "3")
                    return (Results.Failed, "Elected Barngay Official already exist, Please try again", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, object reqdoc)> LoadRequestDocument(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_REQ_DOC0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmstatus",request.Status },
                {"parmrownum", request.num_row},
                {"parmsearch", request.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllRequestDocumentList(result.Read<dynamic>(), request.Userid, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, string message, String reqdocid)> UpdateRequestDocumentAsync(RequestDocument request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_REQ_DOC0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmreqdocid", request.ReqDocID },
                {"parmdoctypeid", request.DoctypeID },
                {"@parmcategorydocument", request.Category_Document },
                {"parmpapplicationdate", request.ApplicationDate },
                {"parmbizname", request.BusinessName},
                {"parmbizaddress",request.BusinessAddress },
                {"parmbizownername",request.BusinessOwnerName },
                {"parmbizowneraddress", request.BusinessOwnerAddress },
                {"parmbiztype", request.Type },
                {"parmrequestorid", request.RequestorID },
                {"parmrequestorname", request.RequestorNM },
                {"parmcategory", request.CategoryID },
                {"parmotherdocument", request.OTRDocumentType },
                {"parmattahcment",request.URLAttachment },
                {"parmIsPaid", request.isPaid },
                {"parmctcno", request.CTCNo },
                {"parmorno", request.ORNO },
                {"parmamount", request.Amount },
                {"parmstatus", request.STATUS },
                {"parmxattchmnt",request.iAttachments },
                {"parmisfree", request.isFree },
                {"parmoptrid",account.USR_ID },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    request.ReqDocID = row["REQ_DOC_ID"].Str();
                    return (Results.Success, "Successfully save", request.ReqDocID);
                }
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again", null);
                else if (ResultCode == "2")
                    return (Results.Failed, "Check Payment Detials, Please try again", null);
                else if (ResultCode == "3")
                    return (Results.Failed, "Requested Document already exist, Please try again", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, string message)> PrintRequestDocumentAsync(RequestDocument request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_REQ_DOC0D", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmreqdocid", request.ReqDocID },
                {"parmreqdocurlpath", request.URL_DocPath },
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
                else if (ResultCode == "3")
                    return (Results.Failed, "Already Generated, Please try again");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message, string reqdocid)> RequestBrgyClearanceAsync(RequestDocument request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_REQ_DOC0E", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmpapplicationdate", request.ApplicationDate },
                {"parmdoctypeid", request.DoctypeID },
                {"parmcategorydocument", request.Category_Document },
                {"parmrequestorid", request.RequestorID },
                {"parmrequestorname", request.RequestorNM },
                {"parmpurpose", request.Purpose },
                {"parmcategory", request.CategoryID },
                {"parmotherdocument", request.OTRDocumentType },
                {"parmattahcment",request.URLAttachment },
                {"parmxattchmnt",request.iAttachments },
                {"parmisfree", request.isFree },
                {"parmoptrid",account.USR_ID },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    request.ReqDocID = row["REQ_DOC_ID"].Str();
                    return (Results.Success, "Successfully save", request.ReqDocID);
                }
                    
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again", null);
                else if (ResultCode == "3")
                    return (Results.Failed, "Requested Document already exist, Please try again", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, string message, string reqdocid)> UpdateRequestBrgyClearanceAsync(RequestDocument request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_REQ_DOC0F", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmreqdocid",request.ReqDocID },
                {"parmpapplicationdate", request.ApplicationDate },
                {"parmdoctypeid", request.DoctypeID },
                {"parmcategorydocument", request.Category_Document },
                {"parmrequestorid", request.RequestorID },
                {"parmrequestorname", request.RequestorNM },
                {"parmpurpose", request.Purpose },
                {"parmcategory", request.CategoryID },
                {"parmotherdocument", request.OTRDocumentType },
                {"parmattahcment",request.URLAttachment },
                {"parmxattchmnt",request.iAttachments },
                {"parmctcno", request.CTCNo },
                {"parmorno", request.ORNO },
                {"parmamount", request.Amount },
                {"parmstatus", request.STATUS },
                {"parmisfree", request.isFree },
                {"parmoptrid",account.USR_ID },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save", request.ReqDocID);
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again", null);
                else if (ResultCode == "3")
                    return (Results.Failed, "Requested Document already exist, Please try again", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, object reqdoc)> LoadIssuesConcernAttachment(RequestDocument request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_REQDOCATTM", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmreqdocid", request.ReqDocID}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAttachementReqDocAttmList(result.Read<dynamic>(), 100));
            else
                return (Results.Success, STLSubscriberDto.GetAttachementReqDocAttmList(result.Read<dynamic>(), 100));

            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> ReceivedRequestDocument(RequestDocument request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_REQ_DOC0G", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmreqdocid",request.ReqDocID },
                {"parmdateappointment", request.AppointmentDate },
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

        public async Task<(Results result, object purpose)> LoadPurpose()
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_PURPOSE0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetPurposeList(result.Read<dynamic>(), 1000));

            return (Results.Null, null);
        }

        public async Task<(Results result, object bizname)> LoadBusinessName()
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BUSINESS0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetBusinessNameList(result.Read<dynamic>(), 1000));

            return (Results.Null, null);
        }

        public async Task<(Results result, object bizowner)> LoadBusinessOwner()
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BUSINESSOWNER0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetBusinessOwnerList(result.Read<dynamic>(), 1000));

            return (Results.Null, null);
        }

        public async Task<(Results result, object businesstype)> LoadBusinessType(string businessname)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BUSINESS0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmbusinessname",businessname }

            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetBusinessTypeList(result.Read<dynamic>(), 1000));

            return (Results.Null, null);

        }

        public async Task<(Results result, object reqdoc)> LoadIndividualRequestDocument(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_REQ_DOC0C01", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid", request.Userid },
                {"parmstatus", request.Status },
                {"parmrownum", request.num_row},
                {"parmsearch", request.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllRequestDocumentList(result.Read<dynamic>(), request.Userid, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, string message, string reqdocid, string controlno)> ResidenceRequestDocumentAsync(RequestDocument request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_REQ_DOC0A02", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmpapplicationdate", request.ApplicationDate },
                {"parmdoctypeid", request.DoctypeID },
                {"parmcategorydocument", request.Category_Document },
                {"parmpurpose", request.Purpose },
                {"parmbizid", request.BusinessID },
                {"parmbizname", request.BusinessName},
                {"parmbizaddress",request.BusinessAddress },
                {"parmbizownername",request.BusinessOwnerName },
                {"parmbizowneraddress", request.BusinessOwnerAddress },
                {"parmbiztype", request.Type },
                {"parmrequestorid", request.RequestorID },
                {"parmrequestorname", request.RequestorNM },
                {"parmcategory", request.CategoryID },
                {"parmotherdocument", request.OTRDocumentType },
                {"parmxattchmnt",request.iAttachments },
                {"parmctcno", request.CTCNo },
                {"parmorno", request.ORNO },
                {"parmamount", request.Amount },
                {"parmisfree", request.isFree },
                {"parmstatus", request.STATUS },
                {"parmoptrid",account.USR_ID },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save", row["REQ_DOC_ID"].Str(), row["CNTRL_NO"].Str());
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again", null, null);
                else if (ResultCode == "3")
                    return (Results.Failed, "Elected Barngay Official already exist, Please try again", null, null);
            }
            return (Results.Null, null, null, null);
        }
    }
}
