using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Comm.Commons.Extensions;
using webapi.Commons.AutoRegister;
using webapi.App.Aggregates.Common;
using webapi.App.STLDashboardModel;
using Infrastructure.Repositories;

namespace webapi.App.Aggregates.STLPartylistDashboard.HeadOfficeRepository
{
    [Service.ITransient(typeof(HeadOfficeRepository))]
    public interface IHeadOfficeRepository
    {
        Task<(Results result, String message)> SaveHeadOffice(AgentHeadOffice ho);
    }
    public class HeadOfficeRepository:IHeadOfficeRepository
    {
        public readonly IRepository _repo;
        public HeadOfficeRepository(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<(Results result, string message)> SaveHeadOffice(AgentHeadOffice ho)
        {
            try
            {
                var result = _repo.DSpQuery<dynamic>($"dbo.spfn_CAABDABDB", new Dictionary<string, object>()
                {
                    {"@parmplid",ho.parmplid },
                    {"@parmplnm",ho.parmplnm },
                    {"@parmpladd",ho.parmpladd },
                    {"@parmtelno",ho.parmtelno },
                    {"@parmplemladd",ho.parmplemladd },

                    {"@parmpgrpid",ho.parmpgrpid },
                    //{"@parmpsncd",ho.parmpsncd },
                    {"@parmpltclid",ho.parmpltclid},

                    {"@parmusrfnm",ho.parmusrfnm },
                    {"@parmusrlnm",ho.parmusrlnm },
                    {"@parmusrmnm",ho.parmusrmnm },
                    {"@parmusrnm",ho.parmusrnm },
                    {"@parmusrpsswrd",ho.parmusrpsswrd },
                    {"@parmaddrss",ho.parmaddrss },
                    {"@parmusrmobno",ho.parmusrmobno },
                    {"@parmemladd",ho.parmemladd },
                    {"@parmuserid",ho.parmuserid },
                }).FirstOrDefault();
                if (result != null)
                {
                    var row = ((IDictionary<string, object>)result);
                    string ResultCode = row["RESULT"].Str();
                    if (ResultCode == "1")
                    {
                        return (Results.Success, "Successfully Save");
                    }
                    return (Results.Failed, "Something wrong in your data. Please try again");
                }
                return (Results.Null, null);
            }
            catch (Exception) { throw; }
        }
    }

}
