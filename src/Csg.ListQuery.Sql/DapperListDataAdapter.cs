using Csg.Data;
using Csg.Data.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.ListQuery.Sql
{
    public class DapperListDataAdapter : IListQueryDataAdapter
    {
        private System.Data.IDbConnection _connection;
        private System.Data.IDbTransaction _transaction;

        public DapperListDataAdapter(System.Data.IDbConnection connection, System.Data.IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public async Task<IEnumerable<T>> GetResultsAsync<T>(SqlStatementBatch batch, bool stream, int commandTimeout)
        {
            var cmdFlags = stream ? Dapper.CommandFlags.Pipelined : Dapper.CommandFlags.Buffered;
            var cmd = batch.ToDapperCommand(_transaction, commandTimeout, commandFlags: cmdFlags);

            return await Dapper.SqlMapper.QueryAsync<T>(_connection, cmd);
        }

        public async Task<BatchResult<T>> GetTotalCountAndResultsAsync<T>(SqlStatementBatch batch, bool stream, int commandTimeout)
        {
            var cmdFlags = stream ? Dapper.CommandFlags.Pipelined : Dapper.CommandFlags.Buffered;
            var cmd = batch.ToDapperCommand(_transaction, commandTimeout, commandFlags: cmdFlags);

            var result = new BatchResult<T>();

            using (var batchReader = await Dapper.SqlMapper.QueryMultipleAsync(_connection, cmd))
            {
                result.TotalCount = await batchReader.ReadFirstAsync<int>();
                result.Items = await batchReader.ReadAsync<T>();
            }

            return result;
        }
    }
}
