
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using Comm.Commons.Extensions;
using webapi.App.Features.UserFeature;
using webapi.App.STLDashboardModel;
using webapi.Commons.AutoRegister;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(AnnouncementRepository))]
    public interface IAnnouncementRepository
    {
        Task<(Results result, String message)> AnnouncementAsync(string title, string description, bool isUpdate=false);
        Task<(Results result, string message, object request)> EventsAsync(GROUPEVENT request, bool isUpdate = false);
        Task<(Results result, object stlevents)> LoadEvents();
        //Task<(Results result, object announcement)> LoadAnnouncement();
    }
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public AnnouncementRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message)> AnnouncementAsync(string title, string description, bool isUpdate = false)
        {
            var result = _repo.DSpQuery<dynamic>("dbo.spfn_1AAN0PA", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmoptrid",account.USR_ID },
                {"parmttl",title },
                {"parmdesc",description }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    await Pusher.PushAsync($"/{account.PL_ID}/{account.PGRP_ID}/notify", new { type = "post-notification", content = result });
                    return (Results.Success, "Successfully published");
                }
                return (Results.Failed, "Something wrong in your data. Please try again");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object announcement)> LoadAnnouncement()
        {
            throw new NotImplementedException();
        }

        public async Task<(Results result, string message, object request)> EventsAsync(GROUPEVENT request, bool isUpdate = false)
        {
            var result = _repo.DSpQuery<dynamic>("dbo.spfn_0CC01", new Dictionary<string, object>()
            {
                {"parmplid", (isUpdate?request.PL_ID:request.PL_ID)},
                {"parmpgrpid", (isUpdate?request.PGRP_ID:request.PGRP_ID)},
                {"parmoptrid", account.USR_ID},
                {"parmtitle", request.Title},
                {"parmdescription", request.Description},
                {"parmdate", request.Date},
                {"parmtime", request.Time},
                {"parmlocation", request.Location},
                {"parmprocessid", (isUpdate?request.Process:"")},
                {"parmeventid", (isUpdate?request.Event:"")},
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    //await Pusher.PushAsync($"/{account.PL_ID}/{(isUpdate ? request.PGRP_ID : request.PGRP_ID)}/notify", new { type = "post-notication", content = SubscriberDto.EventNofitication(result) });
                    await PostAnnouncement(result);
                    return (Results.Success, "Successfully save",result);
                }
                else if (ResultCode == "0")
                    return (Results.Failed, "Something wrong in your data. Please try again",null);
                else if (ResultCode == "2")
                    return (Results.Failed, "Events already exist",null);
            }
            return (Results.Null, null,null);
        }
        public async Task<bool> PostAnnouncement(IDictionary<string, object> data)
        {
            await Pusher.PushAsync($"/{account.PL_ID}/{account.PGRP_ID}/notify"
                , new { type = "post-notification",content = SubscriberDto.EventNofitication(data) });
            return true;
        }
        public async Task<(Results result, object stlevents)> LoadEvents()
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_0CCCBACBB01", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }
    }
}
