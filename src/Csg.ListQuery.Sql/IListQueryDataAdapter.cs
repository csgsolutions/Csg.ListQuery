using Csg.Data;
using Csg.Data.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Csg.ListQuery.Sql
{
    public interface IListQueryDataAdapter
    {
        Task<IEnumerable<T>> GetResultsAsync<T>(SqlStatementBatch batch, bool stream, int commandTimeout, CancellationToken cancellationToken = default);

        Task<BatchResult<T>> GetTotalCountAndResultsAsync<T>(SqlStatementBatch batch, bool stream, int commandTimeout, CancellationToken cancellationToken = default);
    }

    public class BatchResult<T>
    {
        public int TotalCount { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
