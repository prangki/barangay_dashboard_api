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

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(ReportingRepository))]
    public interface IReportingRepository
    {
        Task<(Results result, object acctstatic)> AcctStatistic(AcctStatistic acct);
        Task<(Results result, object donation)> DonnationReport(AcctStatistic acct);
        Task<(Results result, object donation)> EmployeeReport(AcctStatistic acct);
        Task<(Results result, object donation)> DonationReportLeaderMember(AcctStatistic acct);
        Task<(Results result, object acctm)> AcctStatisticMember(AcctStatistic acct);
        Task<(Results result, object acctl)> AcctStatisticLeader(AcctStatistic acct);
    }
    public class ReportingRepository : IReportingRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public ReportingRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object acctstatic)> AcctStatistic(AcctStatistic acct)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BDB0A", new Dictionary<string, object>() 
            {
                {"parmpgrpid",acct.PGRP_ID },
                {"parmreg",acct.Reg },
                {"parmprov",acct.Prov },
                {"parmmun",acct.Mun },
                {"parmbrgy",acct.Brgy },
                {"parmsitio",acct.Sitio }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object donation)> DonnationReport(AcctStatistic acct)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_DAABEABDB0B", new Dictionary<string, object>()
            {
                {"parmpgrpid",acct.PGRP_ID },
                {"parmreg",acct.Reg },
                {"parmprov",acct.Prov },
                {"parmmun",acct.Mun },
                {"parmbrgy",acct.Brgy },
                {"parmsitio",acct.Sitio }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object acctm)> AcctStatisticMember(AcctStatistic acct)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BEABDB0A", new Dictionary<string, object>()
            {
                {"parmpgrpid",acct.PGRP_ID },
                {"parmreg",acct.Reg },
                {"parmprov",acct.Prov },
                {"parmmun",acct.Mun },
                {"parmbrgy",acct.Brgy },
                {"parmsitio",acct.Sitio }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object acctl)> AcctStatisticLeader(AcctStatistic acct)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BEABDB0B", new Dictionary<string, object>()
            {
                {"parmpgrpid",acct.PGRP_ID },
                {"parmreg",acct.Reg },
                {"parmprov",acct.Prov },
                {"parmmun",acct.Mun },
                {"parmbrgy",acct.Brgy },
                {"parmsitio",acct.Sitio }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object donation)> DonationReportLeaderMember(AcctStatistic acct)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_DAABDB0A", new Dictionary<string, object>()
            {
                {"parmpgrpid",acct.PGRP_ID },
                {"parmreg",acct.Reg },
                {"parmprov",acct.Prov },
                {"parmmun",acct.Mun },
                {"parmbrgy",acct.Brgy },
                {"parmsitio",acct.Sitio }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object donation)> EmployeeReport(AcctStatistic acct)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_DAABDB0A1", new Dictionary<string, object>()
            {
                {"parmpgrpid",acct.PGRP_ID },
                {"parmreg",acct.Reg },
                {"parmprov",acct.Prov },
                {"parmmun",acct.Mun },
                {"parmbrgy",acct.Brgy },
                {"parmsitio",acct.Sitio }
            });
            if (results != null)
                return (Results.Success, results);
            return (Results.Null, null);
        }
    }
}
