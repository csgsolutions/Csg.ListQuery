using System;
using System.Collections.Generic;
using Csg.ListQuery.AspNetCore.Abstractions;
using Csg.ListQuery.Abstractions;
using System.Linq;

namespace Csg.ListQuery.AspNetCore
{
    /// <summary>
    /// Represents a query for a list of objects that optionally specifies a set of fields to return, filters, and sort order.
    /// </summary>
    public class ListRequest : IListRequest
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

        /// <summary>
        /// Transforms a list request into a list query
        /// </summary>
        /// <param name="selectableProperties">A set of properties used to validate the given selections</param>
        /// <param name="filerableProperties">A set of properties used to validate the given filters</param>
        /// <param name="sortableProperties">A set of properties used to validate the given sorts</param>
        /// <exception cref="MissingFieldException">When a field is not valid</exception>
        /// <returns></returns>
        public virtual ListRequestValidationResult Validate(
            IDictionary<string, DomainPropertyInfo> selectableProperties,
            IDictionary<string, DomainPropertyInfo> filerableProperties,
            IDictionary<string, DomainPropertyInfo> sortableProperties
        )
        {
            var queryDef = new Csg.ListQuery.Abstractions.ListQueryDefinition();
            var errors = new List<ListRequestValidationError>();

            if (this.Fields != null)
            {
                queryDef.Selections = this.Fields.Select(s => new
                {
                    Raw = s,
                    Exists = selectableProperties.TryGetValue(s, out DomainPropertyInfo domainProp),
                    Domain = domainProp
                }).Where(field =>
                {
                    if (field.Exists)
                    {
                        return true;
                    }
                    else
                    {
                        errors.Add(field.Raw, "Field is not valid for selection.");
                        return false;
                    }
                })
                .Select(field => field.Domain.PropertyName)
                .ToList();
            }

            if (this.Filters != null)
            {
                queryDef.Filters = this.Filters.Select(s => new
                {
                    Raw = s,
                    Exists = filerableProperties.TryGetValue(s.Name, out DomainPropertyInfo domainProp),
                    Domain = domainProp
                }).Where(filter =>
                {
                    if (filter.Exists && filter.Domain.IsFilterable)
                    {
                        return true;
                    }
                    else
                    {
                        errors.Add(filter.Raw.Name, "Field is not valid for filtering.");
                        return false;
                    }
                }).Select(filter =>
                {
                    return new ListQueryFilter()
                    {
                        Name = filter.Domain.PropertyName,
                        Operator = filter.Raw.Operator,
                        Value = filter.Raw.Value
                    };
                })
                .ToList();
            }

            if (this.Sort != null)
            {
                queryDef.Sort = this.Sort.Select(s => new
                {
                    Raw = s,
                    Exists = sortableProperties.TryGetValue(s.Name, out DomainPropertyInfo domainProp),
                    Domain = domainProp
                }).Where(s =>
                {
                    if (s.Exists && s.Domain.IsSortable)
                    {
                        return true;
                    }
                    else
                    {
                        errors.Add(s.Raw.Name, "Field is not valid for sorting.");
                        return false;
                    }
                }).Select(s =>
                {
                    return new ListQuerySort()
                    {
                        Name = s.Domain.PropertyName,
                        SortDescending = s.Raw.SortDescending
                    };
                })
                .ToList();
            }

            return new ListRequestValidationResult(errors, queryDef);
        }

        #region IListRequest

        IEnumerable<string> IListRequest.Fields { get => this.Fields; set => this.Fields = value; }

        IEnumerable<ListQueryFilter> IListRequest.Filters { get => this.Filters; set => this.Filters = value; }

        IEnumerable<ListQuerySort> IListRequest.Sort { get => this.Sort; set => this.Sort = value; }

        #endregion
    }

    public class PagedListRequest : ListRequest, IPagedListRequest 
    {
        /// <summary>
        /// Gets or sets the zero-based index of the first record in the result set that will be returned.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of records that will be returned in the result set.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Transforms a list request into a list query
        /// </summary>
        /// <param name="selectableProperties"></param>
        /// <param name="filerableProperties"></param>
        /// <param name="sortableProperties"></param>
        /// <returns></returns>
        public override ListRequestValidationResult Validate(IDictionary<string, DomainPropertyInfo> selectableProperties, IDictionary<string, DomainPropertyInfo> filerableProperties, IDictionary<string, DomainPropertyInfo> sortableProperties)
        {
            var result = base.Validate(selectableProperties, filerableProperties, sortableProperties);

            if (this.Limit > 0)
            {
                result.ListQuery.Offset = this.Offset;
                result.ListQuery.Limit = this.Limit;
            }

            return result;
        }
    }
}
