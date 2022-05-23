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
using webapi.App.Aggregates.Common.Dto;
using webapi.App.Features.UserFeature;
using webapi.App.STLDashboardModel;
using System.Xml;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(LiveStreamingRepository))]
    public interface ILiveStreamingRepository
    {
        Task<(Results result, object url)> LiveStreamingURL();
        Task<(Results result, String message)> LiveStreamingAsync(string url, string description);
        Task<(Results result, String message)> LiveStreamingAsyncSA(LiveStreaming request);
    }
    public class LiveStreamingRepository : ILiveStreamingRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public LiveStreamingRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        
        public async Task<(Results result, object url)> LiveStreamingURL()
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_CBA02", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
            }).FirstOrDefault();
            if (result != null)
            {
                return (Results.Success, result);
            }
            return (Results.Null, null);
        }
        public IEnumerable<XElement> StreamRootChildDoc(StringReader stringReader)
        {
            using (XmlReader reader = XmlReader.Create(stringReader))
            {
                reader.MoveToContent();
                // Parse the file and display each of the nodes.
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "Child")
                            {
                                XElement el = XElement.ReadFrom(reader) as XElement;
                                if (el != null)
                                    yield return el;
                            }
                            break;
                    }
                }
            }
        }
        public async Task<(Results result, string message)> LiveStreamingAsyncSA(LiveStreaming request)
        {
            //var result = _repo.DSpQuery<dynamic>($"dbo.spfn_CBA04", new Dictionary<string, object>()
            //{
            //    {"parmoptrid",account.USR_ID },
            //    {"parmstrmurl",request.LiveStreamUrl },
            //    {"parmstrmdesc",request.LiveStreamDescription },
            //    {"parmgroup",request.Group }
            //}).FirstOrDefault();
            //if (result != null)
            //{
            //    var row = ((IDictionary<string, object>)result);
            //    string ResultCode = row["RESULT"].Str();
            //    if (ResultCode == "1")
            //    {
            //        return (Results.Success, "Successfully save!");
            //    }
            //    return (Results.Failed, "Something wrong in your data. Please try again");
            //}
            //return (Results.Null, null);

            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_CBA03", new Dictionary<string, object>()
            {
                {"parmplid",request.PL_ID },
                {"parmpgrpid", request.PGRP_ID },
                {"parmoptrid",account.USR_ID },
                {"parmstrmurl",request.LiveStreamUrl },
                {"parmstrmdesc",request.LiveStreamDescription }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    await SASettings(row, new { LiveStreamUrl = request.LiveStreamUrl, LiveStreamName = request.LiveStreamDescription }, request.PL_ID, request.PGRP_ID);
                    return (Results.Success, "Successfully save!");
                }
                return (Results.Failed, "Something wrong in your data. Please try again");
            }
            return (Results.Null, null);
        }
        public async Task<(Results result, string message)> LiveStreamingAsync(string url, string description)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_CBA03", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid", account.PGRP_ID },
                {"parmoptrid",account.USR_ID },
                {"parmstrmurl",url },
                {"parmstrmdesc",description }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    await CompanySettings(row, new { LiveStreamUrl = url, LiveStreamName = description });
                    return (Results.Success, "Successfully save!");
                }
                return (Results.Failed, "Something wrong in your data. Please try again");
            }
            return (Results.Null, null);
        }
        public async Task<bool> CompanySettings(IDictionary<string, object> row, object content)
        {
            //var settings = STLSubscriberDto.GetGroup(row);
            var notifications = SubscriberDto.EventNofitication(row);
            await Pusher.PushAsync($"/{account.PL_ID}/{account.PGRP_ID}/notify"
                , new { type = "post-notification", content = notifications });
            await Pusher.PushAsync($"/{account.PL_ID}/{account.PGRP_ID}/notify"
                , new { type = "streaming-update", content = content });
            return false;
        }

        public async Task<bool> SASettings(IDictionary<string, object> row, object content, string plid, string pgrpid)
        {
            //var settings = STLSubscriberDto.GetGroup(row);
            var notifications = SubscriberDto.EventNofitication(row);
            await Pusher.PushAsync($"/{plid}/{pgrpid}/notify"
                , new { type = "streaming-update", content = content });
            return false;
        }

    }
}
