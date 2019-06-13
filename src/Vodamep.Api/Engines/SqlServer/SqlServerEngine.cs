using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.Security.Principal;
using Vodamep.Api.CmdQry;
using Vodamep.Hkpv.Model;

namespace Vodamep.Api.Engines.SqlServer
{
    public class SqlServerEngine : IEngine
    {
        private readonly string _connectionString;
        private readonly ILogger<SqlServerEngine> _logger;
        private readonly DbUpdater _dbUpdater;
        private bool _dbUpdateDone = false;
        private AuthContext _authContext;

        public SqlServerEngine(SqlServerEngineConfiguration configuration, DbUpdater dbUpdater,
            ILogger<SqlServerEngine> logger)
        {
            _connectionString = configuration.ConnectionString;
            _dbUpdater = dbUpdater;
            _logger = logger;
        }

        public void Execute(ICommand cmd)
        {
            if (!_dbUpdateDone)
            {
                _dbUpdater.Upgrade(_connectionString);
                _dbUpdateDone = true;
            }

            switch (cmd)
            {
                case HkpvReportSaveCommand save:
                    this.Save(save.Report);
                    return;
                case TestCommand test:
                    this.Test();
                    return;
            }

            throw new System.NotImplementedException();
        }

        public void Login(IPrincipal principal)
        {
            this._authContext = principal != null ? new AuthContext(principal) : null;
        }


        private void Test()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var cmd = new SqlCommand("select count(*) from Message", connection);

                var c = cmd.ExecuteScalar();
            }
        }

        private void Save(HkpvReport report)
        {
            if (_authContext?.Principal == null)
            {
                _logger.LogError("Not logged in");
                throw new Exception("Not logged in");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var institutionId = this.GetRowId("Institution", report.Institution.Id, connection);

                var lastInfo = GetLast(report.Institution.Id, institutionId, connection);

                var info = HkpvReportInfo.Create(report, lastInfo?.Id ?? -1, lastInfo?.Created ?? DateTime.Now);

                string state = null;

                if (lastInfo != null && info.Equals(lastInfo))
                {
                    state = "DUPLICATE";
                }

                this.SaveReport(connection, report, info, institutionId, state);
            }
        }

        private void SaveReport(SqlConnection connection, HkpvReport report, HkpvReportInfo info, int institutionId,
            string state)
        {
            using (var ms = report.WriteToStream(asJson: false, compressed: true))
            {
                var userId = this.GetRowId("User", _authContext.Principal.Identity.Name, connection);

                SqlCommand insert =
                    new SqlCommand("insert into [Message]([UserId], [InstitutionId], [Hash_SHA256], [Month], [Year], [Date], [Data], [State]) values(@userId, @institutionId, @hash, @month, @year, @date, @data, @state)",
                        connection);
                insert.Parameters.AddWithValue("@userId", userId);
                insert.Parameters.AddWithValue("@institutionId", institutionId);
                insert.Parameters.AddWithValue("@hash", info.HashSHA256);
                insert.Parameters.AddWithValue("@month", info.Month);
                insert.Parameters.AddWithValue("@year", info.Year);
                insert.Parameters.AddWithValue("@date", DateTime.Now);
                if (!string.IsNullOrWhiteSpace(state))
                    insert.Parameters.AddWithValue("@state", state);
                insert.Parameters.AddWithValue("@data", ms.ToArray());
                try
                {
                    insert.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw ex;
                }

                _logger.LogInformation("Report saved");
            }
        }

        private int GetRowId(string tablename, string name, SqlConnection connection)
        {
            if (string.IsNullOrEmpty(name))
            {
                var msg = $"A name is required ({tablename})";
                _logger.LogError(msg);
                throw new Exception(msg);
            }

            var command = new SqlCommand($"SELECT Id from [{tablename}] where Name =  @name", connection);
            command.Parameters.AddWithValue("@name", name);

            var result = command.ExecuteScalar();

            if (result is int l)
                return l;


            var insert = new SqlCommand($"INSERT into [{tablename}] (Name) values (@name)", connection);
            insert.Parameters.AddWithValue("@name", name);
            insert.ExecuteNonQuery();

            result = command.ExecuteScalar();

            if (result is int l2)
                return l2;

            throw new Exception($"Can not resolve Id '{name}' from '{tablename}'");
        }


        private HkpvReportInfo GetLast(string institution, int institudionId, SqlConnection connection)
        {
            var command =
                new SqlCommand(
                    $"SELECT top 1 [Id],[Month],[Year],[Hash_SHA256],[Date] from [Message] where [InstitutionId] =  @institudionId ORDER BY [Date] desc",
                    connection);
            command.Parameters.AddWithValue("@institudionId", institudionId);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var info = new HkpvReportInfo()
                    {
                        //Id = reader.GetInt32(0),
                        Month = Convert.ToInt32(reader.GetDecimal(1)),
                        Year = Convert.ToInt32(reader.GetDecimal(2)),
                        HashSHA256 = reader.GetString(3),
                        Created = reader.GetDateTime(4),
                        Institution = institution
                    };

                    return info;
                }
            }


            return null;
        }
    }
}