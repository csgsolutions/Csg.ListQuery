using System;
using System.Linq;
using System.Text;
using Csg.ListQuery.Abstractions;
using Csg.ListQuery.AspNetCore.Abstractions;
using System.Collections.Generic;

namespace Csg.ListQuery.AspNetCore
{
    public static class ListRequestExtensions
    {

        /// <summary>
        /// Transforms a list request into a repository query, mapping column names from domain to infrastructure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Csg.ListQuery.Abstractions.ListQueryDefinition ToListQuery(this IListRequest request, 
            IDictionary<string, DomainPropertyInfo> domainProperties,
            IDictionary<string, DomainPropertyInfo> filterProperties
        )
        {
            var queryDef = new Csg.ListQuery.Abstractions.ListQueryDefinition();

            if (request.Fields != null)
            {
                queryDef.Selections = request.Fields.Select(field =>
                {
                    if (domainProperties.TryGetValue(field, out DomainPropertyInfo domainProp))
                    {
                        return domainProp.PropertyName;
                    }
                    else
                    {
                        throw new MissingFieldException($"An invalid field '{field}' was requested.");
                    }
                });
            }

            if (request.Filters != null)
            {            
                queryDef.Filters = request.Filters.Select(filter =>
                {
                    string name = filter.Name;
                
                    if (filterProperties.TryGetValue(filter.Name, out DomainPropertyInfo domainProp) && domainProp.IsFilterable)
                    {
                        name = domainProp.PropertyName;
                    }
                    else
                    {
                        throw new MissingFieldException($"An invalid filter '{filter.Name}' was requested.");
                    }

                    return new ListQueryFilter()
                    {
                        Name = name,
                        Operator = filter.Operator,
                        Value = filter.Value
                    };
                });
            }

            if (request.Sort != null)
            {
                queryDef.Sort = request.Sort.Select(s =>
                {
                    if (domainProperties.TryGetValue(s.Name, out DomainPropertyInfo domainProp) && domainProp.IsSortable)
                    {
                        return new ListQuerySort()
                        {
                            Name = domainProp.PropertyName,
                            SortDescending = s.SortDescending
                        };
                    }
                    else
                    {
                        throw new MissingFieldException($"An invalid sort field '{s.Name}' was requested.");
                    }
                });
            }

            if (request is IPagedListRequest pagedRequest)
            {
                if (pagedRequest.Limit > 0)
                {
                    queryDef.Offset = pagedRequest.Offset;
                    queryDef.Limit = pagedRequest.Limit;
                }
            }          

            return queryDef;           
        }
    }
}
