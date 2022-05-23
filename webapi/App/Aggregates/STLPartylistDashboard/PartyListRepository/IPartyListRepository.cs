using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Comm.Commons.Extensions;

using webapi.Commons.AutoRegister;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.Common.Dto;
using webapi.App.STLDashboardModel;

namespace webapi.App.Aggregates.STLPartylistDashboard.PartyListRepository
{
    [Service.ITransient(typeof(PartyListRepository))]
    public interface IPartyListRepository
    {
        Task<(Results result, String message, object stl)> LoadPartyList();
    }
    public class PartyListRepository : IPartyListRepository
    {
        public readonly IRepository _repo;
        public PartyListRepository(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<(Results result, String message, object stl)> LoadPartyList()
        {
            var results = _repo.DQuery<dynamic>($"dbo.spfn_CAA02 '8888'");
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results.FirstOrDefault());
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    var stl = STLSubscriberDto.GetSTLParty(results.FirstOrDefault());
                    return (Results.Success, "Exist", results.FirstOrDefault());
                }
                else if (ResultCode == "0")
                {
                    return (Results.Failed, "Not Exist", null);
                }
            }
            return (Results.Null, "Error",null);
        }
    }
}
