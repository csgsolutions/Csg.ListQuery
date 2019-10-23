using System.Collections.Generic;
using Csg.ListQuery;
using Csg.Data;
using System;
using Csg.ListQuery.Sql.Internal;

namespace Csg.ListQuery.Sql
{


    /// <summary>
    /// Configuration properties for the list builder fluent API.
    /// </summary>
    public class ListQueryBuilderConfiguration
    {
        public virtual IDbQueryBuilder QueryBuilder { get; set; }

        public virtual IDictionary<string, ListQueryFilterHandler> Handlers { get; set; }

        public virtual IDictionary<string, ListPropertyInfo> Validations { get; set; }

        public virtual ListQueryDefinition QueryDefinition { get; set; }

        public virtual bool UseValidation { get; set; } = true;

        public virtual bool UseStreamingResult { get; set; } = false;

        /// <summary>
        /// Gets or sets a value that indicates if an additional row beyond <see cref="ListQueryDefinition.Limit"/> will be requested from the data source in order to determine if additional rows can be fetched.
        /// </summary>
        /// <remarks>If <see cref="UseStreamingResult"/> is true, this value is ignored and defaults to false.</remarks>
        public virtual bool UseLimitOracle { get; set; } = true;

        /// <summary>
        /// An event that fires before <see cref="ListQueryExtensions.Apply(IListQueryBuilder)"/> 
        /// </summary>
        public event ApplyEventHandler BeforeApply;

        /// <summary>
        /// An event that fires after <see cref="ListQueryExtensions.Apply(IListQueryBuilder)"/> 
        /// </summary>
        public event ApplyEventHandler AfterApply;

        /// <summary>
        /// Invokes callbacks registered for the <see cref="BeforeApply"/> event
        /// </summary>
        internal protected void OnBeforeApply()
        {
            if (this.BeforeApply != null)
            {
                this.BeforeApply(this, new ApplyEventArgs(this));
            }
        }

        /// <summary>
        /// Invokes callbacks registered for the <see cref="AfterApply"/> event
        /// </summary>
        internal protected void OnAfterApply(IDbQueryBuilder queryBuilder)
        {
            if (this.AfterApply != null)
            {
                this.AfterApply(this, new ApplyEventArgs(this, queryBuilder));
            }
        }

    }


}
