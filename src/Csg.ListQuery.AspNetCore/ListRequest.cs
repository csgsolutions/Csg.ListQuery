using System;
using System.Collections.Generic;
using Csg.ListQuery.AspNetCore.Abstractions;
using Csg.ListQuery.Abstractions;
using System.Linq;
using System.Text;

namespace Csg.ListQuery.AspNetCore
{
    /// <summary>
    /// Represents a query for a list of objects that optionally specifies a set of fields to return, filters, and sort order.
    /// </summary>
    public class ListRequest : IListRequestModel
    {
        /// <summary>
        /// Gets or sets a collection of field names to return.
        /// </summary>
        public virtual IEnumerable<string> Fields { get; set; }

        /// <summary>
        /// Gets or sets a collection of filters to apply.
        /// </summary>
        public virtual IEnumerable<Csg.ListQuery.Abstractions.ListQueryFilter> Filters { get; set; }

        /// <summary>
        /// Gets or sets a list of sort actions to apply.
        /// </summary>
        public virtual IEnumerable<Csg.ListQuery.Abstractions.ListQuerySort> Sort { get; set; }
        
        /// <summary>
        /// Gets or sets the zero-based index of the first record in the result set that will be returned.
        /// </summary>
        public virtual int Offset { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of records that will be returned in the result set.
        /// </summary>
        public virtual int Limit { get; set; }

        /// <summary>
        /// Transforms a list request into a list query
        /// </summary>
        /// <param name="selectableProperties">A set of properties used to validate the given selections</param>
        /// <param name="filerableProperties">A set of properties used to validate the given filters</param>
        /// <param name="sortableProperties">A set of properties used to validate the given sorts</param>
        /// <exception cref="MissingFieldException">When a field is not valid</exception>
        /// <returns></returns>
        public virtual ListRequestValidationResult Validate(
            IDictionary<string, ListItemPropertyInfo> selectableProperties,
            IDictionary<string, ListItemPropertyInfo> filerableProperties,
            IDictionary<string, ListItemPropertyInfo> sortableProperties
        )
        {
            var queryDef = new Csg.ListQuery.Abstractions.ListQueryDefinition();
            var errors = new List<ListRequestValidationError>();

            if (this.Fields != null)
            {
                queryDef.Selections = this.Fields.Select(s => new
                {
                    Raw = s,
                    Exists = selectableProperties.TryGetValue(s, out ListItemPropertyInfo domainProp),
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
                    Exists = filerableProperties.TryGetValue(s.Name, out ListItemPropertyInfo domainProp),
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
                    Exists = sortableProperties.TryGetValue(s.Name, out ListItemPropertyInfo domainProp),
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

            if (this.Limit > 0)
            {
                queryDef.Offset = this.Offset;
                queryDef.Limit = this.Limit;
            }

            return new ListRequestValidationResult(errors, queryDef);
        }
        
        public string ToQueryString()
        {
            var query = new StringBuilder("?");

            if (this.Fields != null)
            {
                query.Append(UrlElements.c_fields).Append("=").Append(string.Join(",", this.Fields.Select(System.Uri.EscapeDataString))).Append("&");
            }

            if (this.Sort != null)
            {
                query.Append(UrlElements.c_order).Append("=").Append(string.Join(",", this.Sort.Select(s => string.Concat(s.SortDescending ? "-" : "", System.Uri.EscapeDataString(s.Name))))).Append("&");
            }

            if (this.Offset > 0)
            {
                query.Append($"offset={this.Offset}").Append("&");
            }

            if (this.Limit > 0)
            {
                query.Append($"limit={this.Limit}").Append("&");
            }

            if (this.Filters != null)
            {
                foreach (var filter in this.Filters)
                {

                    // special case needs to make two filters and Value is an enumerable with two values
                    if (filter.Operator == ListQuery.Abstractions.ListFilterOperator.Between)
                    {
                        var filterValues = Csg.ListQuery.Internal.ValueHelpers.GetFilterValues(filter.Value, System.Data.DbType.String, true)
                            .Select(s => s.ToString());

                        query.Append($"where[{System.Uri.EscapeDataString(filter.Name)}]=ge:")
                            .Append(System.Uri.EscapeDataString(filterValues.First()))
                            .Append("&")
                            .Append($"where[{System.Uri.EscapeDataString(filter.Name)}]=le:")
                            .Append(System.Uri.EscapeDataString(filterValues.Skip(1).First()))
                            .Append("&");
                    }
                    else
                    {
                        query.Append($"where[{System.Uri.EscapeDataString(filter.Name)}]=")
                            .Append(ListQueryFilter.OperatorToString(filter.Operator))
                            .Append(":")
                            .Append(System.Uri.EscapeDataString(filter.Value.ToString()))
                            .Append("&");
                    }
                }
            }

            // remove the last ampersand
            query.Length--;

            return query.ToString();
        }

        #region IListRequest

        IEnumerable<string> IListRequest.Fields { get => this.Fields; set => this.Fields = value; }

        IEnumerable<ListQueryFilter> IListRequest.Filters { get => this.Filters; set => this.Filters = value; }

        IEnumerable<ListQuerySort> IListRequest.Sort { get => this.Sort; set => this.Sort = value; }

        #endregion
    }
}
