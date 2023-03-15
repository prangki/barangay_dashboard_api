
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
        Task<(Results result, object licinfo)> LicenseRegister(LicenseKey lic);
        Task<(Results result, String message)> LicenseKeyRegister(LicenseKey lic);
        Task<(Results result, String message)> GenerateLicenseKey(Generate_License_Key lic, bool isUpdate = false);
        //Task<(Results result, object lic)> LoadGeneatedLicense(string search, int row);
        Task<(Results result, object lic)> LoadGeneatedLicense(LicenseFilterRequest filter);
        Task<(Results result, object lic)> LicenseKeyAvilability(LicenseKeyAvailable lic);
        Task<(Results result, String message)> LicensekeyCertificateAsync(LicenseKeyCertificate cert);

    }
    public class LicenseRegisterRepository: ILicenseRegisterRepository
    {
        public readonly IRepository _repo;
        public LicenseRegisterRepository(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<(Results result, string message)> LicensekeyCertificateAsync(LicenseKeyCertificate cert)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIMSDAC0A", new Dictionary<string, object>()
            {
                {"parmscertificatecontent", cert.certificatecontent },
                {"parmoptrid", cert.UserAccount},
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    return (Results.Success, "Successfully saved!");
                }

                else if (ResultCode == "2")
                    return (Results.Failed, "License was not valid!");
            }
            return (Results.Null, null);
        }
        public async Task<(Results result, string message)> GenerateLicenseKey(Generate_License_Key lic, bool isUpdate=false)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_DAB0C", new Dictionary<string, object>()
            {
                {"parmid", lic.ID },
                {"parmplid", lic.PL_ID},
                {"parmpgrpid", lic.PGRP_ID},
                {"parmlicensekey", lic.LicenseKey},
                {"parmlicensetype", lic.LicenseType},
                {"parmnumber", lic.Expiry_Days},
                {"parmlicmod", lic.Lic_Mod},
                {"parmbarangaycode", lic.Location },
                {"parmproductid", lic.ProductID},
                {"parmextension", lic.Extension},
                {"parmprevextension", lic.prevExtension},
                {"@parmrenew", lic.licenserenew},
                {"parmoptrid", lic.UserAccount},
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    if (!isUpdate)
                    {
                        lic.ID = row["ID"].Str();
                        lic.Extension = row["EXTN_LOC_NO"].Str();
                        lic.Date_Generated = DateTime.Now.ToString("MMMM dd, yyyy");
                    }
                        
                    return (Results.Success, "Successfully saved!");
                }
                    
                else if (ResultCode == "2")
                    return (Results.Failed, "License was not valid!");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object lic)> LicenseKeyAvilability(LicenseKeyAvailable lic)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DAB0E", new Dictionary<string, object>()
            {
                {"parmplid", lic.plid},
                {"parmpgrpid", lic.pgrpid},
                {"parmlicensekey", lic.licensekey},
            });
            if (result != null)
                return (Results.Success, SubscriberDto.GetAvailableLicenseKeyList(result));
            return (Results.Null, null);
        }


        public async Task<(Results result, string message)> LicenseKeyRegister(LicenseKey lic)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_DAB0B", new Dictionary<string, object>()
            {
                {"parmplid",lic.plid },
                {"parmpgrpid",lic.pgrpid},
                {"parmlicensekey", lic.licensekey},
                {"parmlicenseval", lic.licenseval},
                {"parmlicenseexpiry", lic.licenseexpiry},
                {"parmregistereddevice", lic.registereddevice},
                {"parmmcaddress", lic.mcaddress },
                {"parmlicmod", lic.licmod },
                {"parmbarngaycode", lic.location },
                {"parmextension", lic.localno },
                {"parmoptrid", lic.userid},
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    //lic.localno = row["LOC_NO"].Str();
                    return (Results.Success, "Successfully saved!");
                }
                    
                else if (ResultCode == "2")
                    return (Results.Failed, "License was not valid!");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object licinfo)> LicenseRegister(LicenseKey lic)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DAB0A", new Dictionary<string, object>()
            {
                {"parmplid",lic.plid },
                {"parmpgrpid",lic.pgrpid},
                {"parmlicensekey", lic.licensekey},
                {"parmregistereddevice", lic.registereddevice},
                {"parmmcaddress", lic.mcaddress },
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object lic)> LoadGeneatedLicense(LicenseFilterRequest filter)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DAB0D", new Dictionary<string, object>()
            {
                {"parmsearch", filter.search},
                {"parmrownum", filter.num_row},
                {"parmreg", filter.reg},
                {"parmprov", filter.prov},
                {"parmmun", filter.mun},
                {"parmbrgy", filter.brgy},
            });
            if (result != null)
                return (Results.Success, SubscriberDto.GetGenerateLicenseKeyList(result));
            return (Results.Null, null);
        }
    }
}
