using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.ListQuery
{
    /// <summary>
    /// Represents the result of a list query against a data source.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListQueryResult<T>
    {
        /// <summary>
        /// Initializes a new instance with the given option.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataCount"></param>
        /// <param name="totalCount"></param>
        /// <param name="isBuffered"></param>
        /// <param name="hasMoreData"></param>
        public ListQueryResult(IEnumerable<T> data, int? dataCount = null, int ? totalCount = null, bool isBuffered = true, bool hasMoreData = false)
        {
            this.Data = data;
            this.DataCount = dataCount;
            this.IsBuffered = isBuffered;
            this.TotalCount = totalCount;
            this.HasMoreData = hasMoreData;
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

        /// <summary>
        /// Gets a value that indicates if there is more data at the source that can be fetched by incrementing the requested offset.
        /// </summary>
        public bool HasMoreData { get; private set; }
    }
}
