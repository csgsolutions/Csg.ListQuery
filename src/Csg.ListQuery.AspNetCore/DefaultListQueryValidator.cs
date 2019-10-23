﻿using Csg.ListQuery;
using Csg.ListQuery.Server;
using Csg.ListQuery.Server.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csg.ListQuery.AspNetCore
{
    public class DefaultListQueryValidator : IListQueryValidator
    {

        protected virtual Csg.ListQuery.ListQueryDefinition CreateQueryDefinition()
        {
            return new Csg.ListQuery.ListQueryDefinition();
        }

        /// <summary>
        /// Transforms a list request into a list query
        /// </summary>
        /// <param name="selectableProperties">A set of properties used to validate the given selections</param>
        /// <param name="filerableProperties">A set of properties used to validate the given filters</param>
        /// <param name="sortableProperties">A set of properties used to validate the given sorts</param>
        /// <exception cref="MissingFieldException">When a field is not valid</exception>
        /// <returns></returns>
        public virtual ListRequestValidationResult Validate(
            IListRequest request,
            IDictionary<string, ListItemPropertyInfo> selectableProperties,
            IDictionary<string, ListItemPropertyInfo> filerableProperties,
            IDictionary<string, ListItemPropertyInfo> sortableProperties
        )
        {
            var queryDef = CreateQueryDefinition();
            var errors = new List<ListRequestValidationError>();

            if (request.Fields != null)
            {
                queryDef.Selections = request.Fields.Select(s => new
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

            if (request.Filters != null)
            {
                queryDef.Filters = request.Filters.Select(s => new
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

            if (request.Sort != null)
            {
                queryDef.Sort = request.Sort.Select(s => new
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

            if (request.Limit > 0)
            {
                queryDef.Offset = request.Offset;
                queryDef.Limit = request.Limit;
            }

            return new ListRequestValidationResult(errors, queryDef);
        }
    }
}