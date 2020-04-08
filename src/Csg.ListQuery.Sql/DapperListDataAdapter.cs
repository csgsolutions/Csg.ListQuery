using Csg.Data;
using Csg.Data.Abstractions;
using Csg.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.ListQuery.Sql
{
    public class DapperListDataAdapter : IListQueryDataAdapter
    {
        private Csg.Data.Common.IDbQueryBuilder _originalQueryBuilder;

        public DapperListDataAdapter(Csg.Data.Common.IDbQueryBuilder queryBuilder)
        {
            _originalQueryBuilder = queryBuilder;
        }

        public async Task<IEnumerable<T>> GetResultsAsync<T>(ISelectQueryBuilder query, bool stream, int? commandTimeout)
        {
            var connection = _originalQueryBuilder.Features.Get<IDbConnection>(_originalQueryBuilder);
            var transaction = _originalQueryBuilder.Features.Get<IDbTransaction>(_originalQueryBuilder);
            var cmd = query.Render().ToDapperCommand(transaction, commandTimeout: commandTimeout);

            return await Dapper.SqlMapper.QueryAsync<T>(connection, cmd);
        }

        public async Task<BatchResult<T>> GetTotalCountAndResultsAsync<T>(ISelectQueryBuilder query, bool stream, int? commandTimeout)
        {
            var countQuery = query.SelectOnly(new SqlRawColumn("COUNT(1)"));

            countQuery.Configuration.PagingOptions = null;
            countQuery.Configuration.OrderBy.Clear();

            var connection = _originalQueryBuilder.Features.Get<IDbConnection>(_originalQueryBuilder);
            var transaction = _originalQueryBuilder.Features.Get<IDbTransaction>(_originalQueryBuilder);

            var batchCmd = new ISqlStatementElement[] { countQuery, query }.RenderBatch(_originalQueryBuilder.Provider)
                .ToDapperCommand(transaction, commandTimeout: commandTimeout);
            
            var result = new BatchResult<T>();

            using (var batchReader = await Dapper.SqlMapper.QueryMultipleAsync(connection, batchCmd))
            {
                result.TotalCount = await batchReader.ReadFirstAsync<int>();
                result.Items = await batchReader.ReadAsync<T>();
            }

            return result;
        }
    }
}
