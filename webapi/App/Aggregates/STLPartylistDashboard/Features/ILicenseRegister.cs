
using webapi.Commons.AutoRegister;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using Comm.Commons.Extensions;
using webapi.App.RequestModel.AppRecruiter;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(LicenseRegisterRepository))]
    public interface ILicenseRegisterRepository
    {
        Task<(Results result, object licinfo)> LicenseRegister();
        Task<(Results result, String message)> LicenseKeyRegister(LicenseKey lic);

    }
    public class LicenseRegisterRepository: ILicenseRegisterRepository
    {
        public readonly IRepository _repo;
        public LicenseRegisterRepository(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<(Results result, string message)> LicenseKeyRegister(LicenseKey lic)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_DAB0B", new Dictionary<string, object>()
            {
                {"parmlicensekey", lic.licensekey},
                {"parmlicenseval", lic.licenseval},
                {"parmlicenseexpiry", lic.licenseexpiry},
                {"parmregistereddevice", lic.registereddevice}
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "2")
                    return (Results.Failed, "License was not valid");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object licinfo)> LicenseRegister()
        {
            var result = _repo.DQuery<dynamic>($"dbo.spfn_DAB0A");
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }
    }
}
