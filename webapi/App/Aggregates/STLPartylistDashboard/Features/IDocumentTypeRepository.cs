using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using webapi.Commons.AutoRegister;

using Comm.Commons.Extensions;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.Common.Dto;
using webapi.App.STLDashboardModel;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(DocumentTypeRepository))]
    public interface IDocumentTypeRepository
    {
        Task<(Results result, String message, String doctypeid)> DocumentTypeAsync(DocumentType request);
        Task<(Results result, String message)> UpdateDocumentTypeAsync(DocumentType request);
        Task<(Results result, object doctype)> LoadMDocumentType(FilterRequest request);
        Task<(Results result, object docrqrd)> LoadMDocRequirement(FilterRequest request);
    }
    partial class DocumentTypeRepository:IDocumentTypeRepository
    {
        private readonly ISubscriber _identity;
        private readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public DocumentTypeRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message, string doctypeid)> DocumentTypeAsync(DocumentType request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_DOCTYPE0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmbrgy", request.BrgyCode},
                {"parmdoctype",request.DocTypeNM },
                {"parmcategory",request.Category },
                {"parmdocrequirement", request.iDocRequirements },
                {"parmuserid",account.USR_ID },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save", row["DOC_TYP_ID"].Str());
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again", null);
                else if (ResultCode == "3")
                    return (Results.Failed, "Document already exist, Please try again", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, string message)> UpdateDocumentTypeAsync(DocumentType request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_DOCTYPE0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmdocid", request.DocTypeID },
                {"parmbrgy", request.BrgyCode},
                {"parmdoctype",request.DocTypeNM },
                {"parmcategory",request.Category },
                {"parmdocrequirement", request.iDocRequirements },
                {"parmuserid",account.USR_ID },
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
                    return (Results.Failed, "Document already exist, Please try again");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object doctype)> LoadMDocumentType(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_DOCTYPE0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"pmarbrgy", request.BrgyCode},
                {"parmrownum", request.num_row},
                {"parmsearch", request.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllDocTypeList(result.Read<dynamic>(), request.Userid, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, object docrqrd)> LoadMDocRequirement(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_DOCRQRD", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmdocid", request.DocTypeID},
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllDocRequirementList(result.Read<dynamic>(), request.Userid, 100));
            return (Results.Null, null);
        }
    }
}
