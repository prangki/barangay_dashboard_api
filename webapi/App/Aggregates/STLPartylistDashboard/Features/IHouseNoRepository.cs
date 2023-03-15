using Infrastructure.Repositories;
using System.IO;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using webapi.App.STLDashboardModel;
using webapi.Commons.AutoRegister;
using Dapper;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System;
using Comm.Commons.Extensions;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(HouseNoRepository))]
    public interface IHouseNoRepository
    {
        Task<(Results result, object hsspersit)> GetNumOfHousesPerSitio(string code);
        Task<(Results result, object residents)> LoadResidents();
        Task<(Results result, string message)> SaveHouseNo(HouseDetails details);
        Task<(Results result, object houses)> LoadHouses(int currentRow);
        Task<(Results result, string message)> SaveHousehold(HouseDetails details);
        Task<(Results result, object households)> LoadHouseholds(int currentRow);
        Task<(Results result, object households)> LoadSpecificHouseholds(string houseid);
        Task<(Results result, string message)> SaveFamilyMember(HouseDetails details);
        Task<(Results result, object family)> LoadFamilies(int currentRow);
        Task<(Results result, object family)> LoadSpecificFamilyMembers(string householdid);
        Task<(Results result, object houseinfo)> LoadHouseInfo();
        Task<(Results result, string message)> RemoveHouseNo(string houseno);
        Task<(Results result, string message)> RemoveHousehold(string householdid, string userid);
        Task<(Results result, string message)> RemoveFamilyMember(string familyId, string familyMemId);
        Task<(Results result, string message)> UpdateHouseNo(HouseDetails details);
        Task<(Results result, string message)> UpdateHousehold(HouseDetails details);
        Task<(Results result, string message)> UpdateFamily(HouseDetails details);
        Task<(Results result, string message)> UpdateFamilyMember(string userId, string familyId);
        Task<(Results result, object houseinfo)> LoadFamilyMember(string familyId);
        Task<(Results result, string message)> RemoveFamilyMember(string userId);
        Task<(Results result, string message)> AddHouseClassification(string classification);
        Task<(Results result, object classifications)> LoadHouseClassifications();
        Task<(Results result, string message)> RemoveHouseClassifications(string classification);
        Task<(Results result, object report)> LoadHouseReport(string brgyloc, string from, string to);
        Task<(Results result, object numbers)> LoadHouseNumbers();
        Task<(Results result, object family)> LoadFamilyMembers(string householdId);


    }

    public class HouseNoRepository : IHouseNoRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }

        public HouseNoRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object hsspersit)> GetNumOfHousesPerSitio(string code)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGY_HOUSESPERSITIO", new Dictionary<string, object>()
                {
                    {"parmcode", code}
                });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        //public async Task<(Results result, object households)> LoadHouseholds()
        //{
        //    var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHOUSEHOLDS", new Dictionary<string, object>()
        //        {
        //            {"parmplid", account.PL_ID},
        //            {"parmpgrpid", account.PGRP_ID}
        //        });
        //    if (result != null)
        //        return (Results.Success, result);
        //    return (Results.Null, null);
        //}

        public async Task<(Results result, string message)> SaveHouseNo(HouseDetails details)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHOUSENO", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmhseid", details.HouseId},
                    {"parmhseownr", details.HouseOwner},
                    {"parmhseclsfctn", details.HouseClassification},
                    {"parmsitloc", details.SitioId},
                    {"parmhmaddr", details.HomeAddress},
                    {"parmrgsby", details.CreatedBy},
                    {"parmrgsdt", details.DateCreated}
                }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, "Already Exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object houses)> LoadHouses(int currentRow)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHOUSENO", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmcurrow", currentRow}
                });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object residents)> LoadResidents()
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_GETALLRESIDENTS", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID}
                });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> SaveHousehold(HouseDetails details)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHOUSEHOLD", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmhseid", details.HouseId},
                    {"parmhhld", details.Householder},
                    {"parmcsftn", details.HouseholdClassification},
                    {"parmrgsby", details.CreatedBy},
                    {"parmrgsdt", details.DateCreated}
                }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, "Already Exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object households)> LoadHouseholds(int currentRow)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHOUSEHOLD", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmcurrow", currentRow}
                });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object households)> LoadSpecificHouseholds(string houseid)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHOUSEHOLD", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmhseid", houseid}
                });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> SaveFamilyMember(HouseDetails details)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYFMLYMMBR", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmhseid", details.HouseId},
                    {"parmhhldid", details.HouseholdId},
                    {"parmhhldr", details.Householder},
                    {"parmusrid", details.FamilyMember},
                    {"parmrltnsp", details.Relationship},
                    {"parmoptr", details.CreatedBy},
                    {"parmdt", details.DateCreated}
                }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, "Already Exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object family)> LoadFamilies(int currentRow)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYFMLYMMBR", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmcurrow", currentRow}
                });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object family)> LoadSpecificFamilyMembers(string householdid)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYFMLYMMBR", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmhhld", householdid}
                });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object houseinfo)> LoadHouseInfo()
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHSENOHHLDFMLY00", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID}
                });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> RemoveHouseNo(string houseno)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHSENOHHLDFMLY02", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmhseid", houseno}
                }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, $"House# {houseno} doesn't exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> RemoveHousehold(string householdid, string userid)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHSENOHHLDFMLY02", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmhhldid", householdid},
                    {"parmusrid", userid},
                }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, $"Household ({householdid}) doesn't exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> RemoveFamilyMember(string familyId,string familyMemId)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHSENOHHLDFMLY02", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmfamid", familyId},
                    {"parmusrid", familyMemId} 
                }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, $"Family member doesn't exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> UpdateHouseNo(HouseDetails details)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHSENOHHLDFMLY01", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmhseid", details.HouseId},
                    {"parmhseownr", details.HouseOwner},
                    {"parmhseclsfctn", details.HouseClassification},
                    {"parmsitloc", details.SitioId},
                    {"parmhmaddr", details.HomeAddress},
                    {"parmmodby", details.ModifiedBy},
                    {"parmmoddt", details.ModifiedDate},
                }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, $"House# {details.HouseId} doesn't exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> UpdateHousehold(HouseDetails details)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHHLDU", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmhseid", details.HouseId},
                    {"parmhhldid", details.HouseholdId},
                    {"parmhhld", details.Householder},
                    {"parmcsftn", details.HouseholdClassification},
                    {"parmmodby", details.ModifiedBy},
                    {"parmmoddt", details.ModifiedDate}
                }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, $"Household doesn't exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> UpdateFamily(HouseDetails details)
        
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHSENOHHLDFMLY01", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmhseid", details.HouseId},
                    {"parmhhldid", details.HouseholdId},
                    {"parmhhlder", details.Householder},
                    {"parmfamid", details.FamilyId},
                    {"parmnewfamid", details.NewFamilyHead},
                    {"parmmodby", details.ModifiedBy},
                    {"parmmoddt", details.ModifiedDate}
                }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, $"House# {details.HouseId} doesn't exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> UpdateFamilyMember(string userId, string familyId)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYFMLYADMBR", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmusrid", userId},
                    {"parmfamid", familyId}
                }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, $"Already exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object houseinfo)> LoadFamilyMember(string familyId)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYFMLYADMBR", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmfamid", familyId}
                });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> RemoveFamilyMember(string userId)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYFMLYADMBR", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmusrid", userId},
                }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, $"Already exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> AddHouseClassification(string classification)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHSECLSFTN", new Dictionary<string, object>()
            {
                {"parmclsftn", classification}
            }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, $"Already exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object classifications)> LoadHouseClassifications()
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHSECLSFTN", new Dictionary<string, object>()
                {});
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> RemoveHouseClassifications(string classification)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHSECLSFTN", new Dictionary<string, object>()
            {
                {"parmid", classification}
            }).FirstOrDefault();

            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save");
                else if (ResultCode == "0")
                    return (Results.Failed, $"Already exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object report)> LoadHouseReport(string brgyloc, string from, string to)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHSRPT", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid", account.PGRP_ID},
                {"parmbrgyloc", brgyloc},
                {"parmfrom", from},
                {"parmto", to}
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object numbers)> LoadHouseNumbers()
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYHSENOLIST", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid", account.PGRP_ID},
                {"parmbrgyid", account.LOC_BRGY}
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, object family)> LoadFamilyMembers(string householdId)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYVWFMLYMBR", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmhhldid", householdId}
                });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

    }
}
