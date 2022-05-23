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
using webapi.Services.Firebase;
using Comm.Commons.Advance;

using webapi.App.RequestModel.SubscriberApp.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.Common;
using webapi.App.Features.UserFeature;

namespace webapi.App.Aggregates.SubscriberAppAggregate.Features
{
    [Service.ITransient(typeof(SalesReportRepository))] 
    public interface ISalesReportRepository
    {
        Task<(Results result, object report)> MaxSalesReportAsync(FilterRequest filter);
    }

    public class SalesReportRepository : ISalesReportRepository
    {
        private readonly ISubscriber _identity;
        private readonly IRepository _repo;
        public STLAccount account { get{ return _identity.AccountIdentity(); } } 
        public SalesReportRepository(ISubscriber identity, IRepository repo){
            _identity = identity; 
            _repo = repo; 
        }

        public async Task<(Results result, object report)> MaxSalesReportAsync(FilterRequest filter){
            var results = _repo.DSpQueryMultiple("dbo.spfn_ACC0A", new Dictionary<string, object>(){
                { "parmcompid", account.PL_ID },
                { "parmbrcd", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmdtfrom", filter.From },
                { "parmdtto", filter.To },
                { "parmovrall", (filter.IsOverall?"1":"0") },
                { "parmftrns", filter.BaseFilter },
            });
            if(results != null)
                return (Results.Success, SalesReportDto.FilterMaxSalesReports(results.Read<dynamic>(), 50)); 
            return (Results.Null, null); 
        }
    }
}