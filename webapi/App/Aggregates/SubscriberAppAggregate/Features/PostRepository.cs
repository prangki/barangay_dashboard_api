using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Comm.Commons.Extensions;
using Infrastructure.Repositories;
using webapi.Commons.AutoRegister;
using webapi.App.Aggregates.Common;
using webapi.App.Model.User;
using webapi.App.Globalize.Company;

using Newtonsoft.Json;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;

namespace webapi.App.Aggregates.SubscriberAppAggregate.Features
{
    [Service.ITransient(typeof(PostRepository))] 
    public interface IPostRepository
    {
        Task<(Results result, object items)> AnnouncementAsync(FilterRequest filter);
        Task<(Results result, object items)> HelpCenterAsync(FilterRequest filter);
    }

    public class PostRepository : IPostRepository
    {
        private readonly ISubscriber _identity;
        private readonly IRepository _repo;
        public STLAccount account { get{ return _identity.AccountIdentity(); } } 
        public PostRepository(ISubscriber identity, IRepository repo){
            _identity = identity; 
            _repo = repo; 
        }

        public async Task<(Results result, object items)> HelpCenterAsync(FilterRequest filter){
             var results = _repo.DSpQueryMultiple("dbo.spfn_ADA0A", new Dictionary<string, object>(){
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmfsqno", filter.BaseFilter },
            });
            if(results != null)
                return (Results.Success, PostDto.FilterHelpCenters(results.Read(), 50));
            return (Results.Null, null); 
        }

        public async Task<(Results result, object items)> AnnouncementAsync(FilterRequest filter){
            var results = _repo.DSpQueryMultiple($@"dbo.spfn_ADB0A", new Dictionary<string, object>(){
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmftrns", filter.BaseFilter },
            });
            if(results != null)
                return (Results.Success, PostDto.FilterAnnouncements(results.Read(), 25));
            return (Results.Null, null); 
        }
    }
}