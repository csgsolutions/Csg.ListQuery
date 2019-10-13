using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.ListQuery.Abstractions
{
    public class ListQueryResult<T>
    {
        public ListQueryResult(IEnumerable<T> data, int? dataCount = null, int ? totalCount = null, bool isBuffered = true)
        {
            this.Data = data;
            this.DataCount = dataCount;
            this.IsBuffered = isBuffered;
            this.TotalCount = totalCount;
        }

        /// <summary>
        /// Gets an enumerator for the data returned from the query.
        /// </summary>
        public virtual IEnumerable<T> Data { get; private set; }

        /// <summary>
        /// Gets the total rows available at the source, regardless of any Limit or Offset applied to the query.
        /// </summary>
        public int? TotalCount { get; set; }

        /// <summary>
        /// Gets a value that indicates if the data in <see cref="Data"/> is buffered or streamed. If
        /// </summary>
        public bool IsBuffered { get; private set; }

        /// <summary>
        /// Gets the length of the data in <see cref="Data"/> if the result is buffered.
        /// </summary>
        public int? DataCount { get; private set; }
    }
}
