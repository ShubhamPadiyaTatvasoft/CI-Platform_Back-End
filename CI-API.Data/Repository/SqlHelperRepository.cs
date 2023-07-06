using CI_API.Data.Interface;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_API.Data.Repository
{
    public class SqlHelperRepository : ISqlHelperRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        public SqlHelperRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("connection"));
        }

        public async Task<int> ChangesOnData<T>(string spName, T parameters)
        {
            return await _connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> GetData<T, P>(string spName, P parameters)
        {
            return await _connection.QueryAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
