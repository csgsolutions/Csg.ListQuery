using System;
using System.Collections.Generic;
using Csg.ListQuery.AspNetCore.Abstractions;
using Csg.ListQuery.Abstractions;

namespace Csg.ListQuery.AspNetCore
{
    /// <summary>
    /// Represents a query for a list of objects that optionally specifies a set of fields to return, filters, and sort order.
    /// </summary>
    /// <typeparam name="TValidationType"></typeparam>
    public class ListRequest<TValidationType> : IListRequest where TValidationType: class
    {
        /// <summary>
        /// Gets or sets a collection of field names to return.
        /// </summary>
        public IEnumerable<string> Fields { get; set; }

        /// <summary>
        /// Gets or sets a collection of filters to apply.
        /// </summary>
        public IEnumerable<Csg.ListQuery.Abstractions.ListQueryFilter> Filters { get; set; }

        /// <summary>
        /// Gets or sets a list of sort actions to apply.
        /// </summary>
        public IEnumerable<Csg.ListQuery.Abstractions.ListQuerySort> Sort { get; set; }

        #region IListRequest

        IEnumerable<string> IListRequest.Fields { get => this.Fields; set => this.Fields = value; }

        IEnumerable<ListQueryFilter> IListRequest.Filters { get => this.Filters; set => this.Filters = value; }

        IEnumerable<ListQuerySort> IListRequest.Sort { get => this.Sort; set => this.Sort = value; }

        Type IListRequest.GetValidationType() => typeof(TValidationType);

        #endregion
    }

    public class PagedListRequest<TValidationType> : ListRequest<TValidationType>, IPagedListRequest where TValidationType : class
    {
        /// <summary>
        /// Gets or sets the zero-based index of the first record in the result set that will be returned.
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// Gets or sets the maximum number of records that will be returned in the result set.
        /// </summary>
        public int Limit { get; set; } = 100;
    }
}
