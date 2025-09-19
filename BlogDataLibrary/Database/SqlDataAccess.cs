using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BlogDataLibrary.Database
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<T>> LoadData<T, U>(string sqlStatement,
                                                  U parameters,
                                                  string connectionStringName,
                                                  bool isStoredProcedure)
        {
            CommandType commandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                var rows = await connection.QueryAsync<T>(sqlStatement, parameters, commandType: commandType);
                return rows.ToList();
            }
        }

        public async Task SaveData<T>(string sqlStatement,
                                      T parameters,
                                      string connectionStringName,
                                      bool isStoredProcedure)
        {
            CommandType commandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sqlStatement, parameters, commandType: commandType);
            }
        }
    }
}
