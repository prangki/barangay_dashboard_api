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
using Newtonsoft.Json;
using Dapper;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(PBXRepository))]
    public interface IPBXRepository
    {
        Task<(Results result, string message)> SaveCallLogs(string id, string name, string callType);
        Task<(Results result, object list)> GetListCallLogs(string id);
        Task<(Results result, object extension)> GetExtensionId(string mcAddress, string fullname);
    }

    public class PBXRepository : IPBXRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }

        public PBXRepository(ISubscriber identity, IRepository repo, IConfiguration config)
        {
            _identity = identity;
            _repo = repo;
            Database.getConnection(config);
        }

        public async Task<(Results result, string message)> SaveCallLogs(string id, string name, string callType)
        {

            var result = Database.ExecuteQuery("dbo.spfn_CALLLOGS", new Dictionary<string, object>
            {
                {"paramusrid", id},
                {"paramcllr", name},
                {"paramclltyp", callType}
            });

            if(result != 1)
                return (Results.Failed, "FAILED");
            return (Results.Success, "SUCCESS");
        }

        public async Task<(Results result, object list)> GetListCallLogs(string id)
        {
            var result = Database.DSpQuery<dynamic>($"dbo.spfn_CALLLOGS", new Dictionary<string, object>()
            {
                {"paramusrid", id}
            });

            if (result != null)
                return (Results.Success, result);
            return (Results.Failed, null);
        }

        public async Task<(Results result, object extension)> GetExtensionId(string mcAddress, string fullname)
        {
            var result = Database.getSingleQuery($"select TOP(1) USER_ID from dbo.USERS with(nolock) where MACHINE_NAME='{mcAddress}' or FULLNAME = '{fullname}' and IS_ACTIVE='1'", "USER_ID");

            if (result != null)
                return (Results.Success, result);
            return (Results.Failed, null);
        }
    };

    public class Database
    {
        private static IConfiguration _config;

        public static SqlConnection getConnection(IConfiguration config){ _config = config; return Connection; }

        public static SqlConnection Connection
        {
            get
            {
                string constring = _config["PBXDatabase:ConnectionString"].Str();
                SqlConnection con = null;
                try
                {
                    con = new SqlConnection(constring);
                }
                catch (SqlException sEx)
                {
                    sEx.StackTrace.ToString();
                }
                return con;
            }
        }

        public static IList<T> DSpQuery<T>(string SpName, object parameters = null) where T : class
        {
            try
            {
                if (parameters != null)
                    return Database.Connection.Query<T>(SpName,
                        param: new DynamicParameters(parameters),
                        commandType: CommandType.StoredProcedure).ToList();
                return Database.Connection.Query<T>(SpName,
                    commandType: CommandType.StoredProcedure).ToList();
            }
            catch { return null; } // 'error to connecting database'/'query error'
        }

        public static IList<T> DQuery<T>(string query, object parameters = null) where T : class
        {
            try
            {
                return Database.Connection.Query<T>(query,
                    commandType: CommandType.Text).ToList();
            }
            catch { return null; } // 'error to connecting database'/'query error'
        }

        public static int ExecuteQuery(string query, object parameters = null)
        {
            int x = 0;
            try
            {
                if (parameters != null)
                    return Convert.ToInt16(Database.Connection.QuerySingle(query,
                        param: new DynamicParameters(parameters),
                        commandType: CommandType.StoredProcedure).RESULT);
                else
                    return Convert.ToInt16(Database.Connection.QuerySingle(query,
                       param: null,
                       commandType: CommandType.Text).RESULT);
            }
            catch (SqlException ex)
            {
                return x;
            }
        }

        public static String getSingleQuery(string query, string returnval)
        {
            string str = "";
            SqlConnection con = Connection;
            con.Open();
            try
            {
                SqlCommand com = new SqlCommand(query, con);
                SqlDataReader reader = com.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        str = reader[returnval].ToString();
                    }
                    reader.Close();
                }
            }
            catch (SqlException ex)
            {

            }
            finally
            {
                con.Close();
            }

            return str;
        }
    }
}
