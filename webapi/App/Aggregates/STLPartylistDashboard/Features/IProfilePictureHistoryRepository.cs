using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using webapi.App.STLDashboardModel;
using webapi.Commons.AutoRegister;
using Comm.Commons.Extensions;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.Common.Dto;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(ProfilePictureHistoryRepository))]
    public interface IProfilePictureHistoryRepository
    {
        Task<(Results result, string message)> UploadProfilePictureAsync(Profile_Picture req);
        Task<(Results result, string message)> ChangeProfilePicture(Profile_Picture req);
    }
    public class ProfilePictureHistoryRepository:IProfilePictureHistoryRepository
    {
        private readonly ISubscriber _identity;
        private readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public ProfilePictureHistoryRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message)> UploadProfilePictureAsync(Profile_Picture req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_PRFPIC0B", new Dictionary<string, object>()
            {
                {"parmplid",req.PL_ID },
                {"parmpgrpid",req.PGRP_ID },
                {"parmoptrid",account.USR_ID },
                {"parmuserid", req.Userid },
                {"parmImgURL", req.ImageURL},
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully saved!");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> ChangeProfilePicture(Profile_Picture req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_PRFPIC0C", new Dictionary<string, object>()
            {
                {"parmplid",req.PL_ID },
                {"parmpgrpid",req.PGRP_ID },
                {"parmoptrid",account.USR_ID },
                {"parmuserid", req.Userid },
                {"parmImgURL", req.ImageURL},
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully saved!");
            }
            return (Results.Null, null);
        }
    }
}
