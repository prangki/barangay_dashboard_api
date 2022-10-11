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

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(TemplateRepository))]
    public interface ITemplateRepository
    {
        Task<(Results result, object tpl)> Load_TemplateType();
        Task<(Results result, String message, String tplid)> TemplateTypeAsync(TemplateType req);
        Task<(Results result, object tpl)> Load_TemplateDoc(string templateid);
        Task<(Results result, String message, string tplid)> TemplateDocAsync(TemplateDocument req);
        Task<(Results result, String message, String descid)> ItemTabAsyn(ItemDescription req);
        Task<(Results result, object item)> Load_ItemTab(ItemDescription req);
        Task<(Results result, object tagline)> Load_Tagline(string templateid);
        Task<(Results result, object restype)> Load_ResType();
        Task<(Results result, String message, String resid)> ResidentTypeAsync(ResidentType req);
        Task<(Results result, String message)> RemoveResidentType(ResidentType req);
        Task<(Results result, String message)> RemoveTemplateType(TemplateType req);
        Task<(Results result, String message)> RemoveTemplateDoc(TemplateDocument req);

    }
    public class TemplateRepository:ITemplateRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public TemplateRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object tpl)> Load_TemplateType()
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_TPLTYP0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message, string tplid)> TemplateTypeAsync(TemplateType req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_TPLTYP0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmtemplatetypeid",req.TemplateTypeID },
                {"parmtemplatetype",req.Description },
                {"parmoptrid",account.USR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    if (req.TemplateTypeID.IsEmpty())
                        req.TemplateTypeID = row["TPLTYP_ID"].Str();
                    return (Results.Success, "Successfully save", req.TemplateTypeID);
                }
                    
                else if (ResultCode == "2")
                    return (Results.Failed, "Description already Exist, Please try again", null);
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Description, Please try again", null);
                return (Results.Failed, "Something wrong in your data, Please try again", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, object tpl)> Load_TemplateDoc(string templateid)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_TPLDOC0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmtemplatetypeid", templateid }
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message, string tplid)> TemplateDocAsync(TemplateDocument req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_TPLDOC0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmtemplateid",req.TemplateDocID },
                {"parmtemplatename",req.Description },
                {"parmtemplatetypeid",req.TemplateTypeID },
                {"parmtemplatecontent",req.TemplateDocContent },
                {"parmtagline",req.iTagline },
                {"parmoptrid",account.USR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    if (req.TemplateDocID.IsEmpty())
                        req.TemplateDocID = row["TPL_ID"].Str();
                    return (Results.Success, "Successfully save", req.TemplateDocID);
                }

                else if (ResultCode == "2")
                    return (Results.Failed, "Description already Exist, Please try again", null);
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Description, Please try again", null);
                return (Results.Failed, "Something wrong in your data, Please try again", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, string message, string descid)> ItemTabAsyn(ItemDescription req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_TPLTAG0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmdescriptionid",req.DescriptionID },
                {"parmtemplateid",req.TPLID },
                {"parmtemplateitemname",req.TP_ItemDescription },
                {"parmtaglinedescription",req.Item_Description },
                {"parmoptrid",account.USR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    if (req.DescriptionID.IsEmpty())
                        req.DescriptionID = row["TPL_ID"].Str();
                    return (Results.Success, "Successfully save", req.DescriptionID);
                }

                else if (ResultCode == "2")
                    return (Results.Failed, "Description already Exist, Please try again", null);
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Description, Please try again", null);
                return (Results.Failed, "Something wrong in your data, Please try again", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, object item)> Load_ItemTab(ItemDescription req)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_TPLTYP0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmtemplateid",req.TPLID },
                {"parmtemplateitemname",req.TP_ItemDescription }
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object tagline)> Load_Tagline(string templateid)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_TPLDOCTAG", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmtemplateid",templateid }
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object restype)> Load_ResType()
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BIMSRESTYP0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message, string resid)> ResidentTypeAsync(ResidentType req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIMSRESTYP0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmrestypid",req.RestTypId },
                {"parmdescription",req.Description },
                {"parmoptrid",account.USR_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    if (req.RestTypId.IsEmpty())
                        req.RestTypId = row["RESTYP_ID"].Str();
                    return (Results.Success, "Successfully save", req.RestTypId);
                }

                else if (ResultCode == "2")
                    return (Results.Failed, "Description already Exist, Please try again", null);
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Description, Please try again", null);
                return (Results.Failed, "Something wrong in your data, Please try again", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, string message)> RemoveResidentType(ResidentType req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIMSRESTYP0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmrestypid", req.RestTypId }
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

        public async Task<(Results result, string message)> RemoveTemplateType(TemplateType req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_TPLTYP0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmtemplatetypeid", req.TemplateTypeID }
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

        public async Task<(Results result, string message)> RemoveTemplateDoc(TemplateDocument req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_TPLDOC0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmtemplateid", req.TemplateDocID },
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
