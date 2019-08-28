﻿using Csg.Data.ListQuery.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.Data.ListQuery.Internal
{
    public static class ReflectionHelper
    {
        private static System.Collections.Concurrent.ConcurrentDictionary<Type, Dictionary<string, ListQueryFilterConfiguration>> s_typeCache 
            = new System.Collections.Concurrent.ConcurrentDictionary<Type, Dictionary<string, ListQueryFilterConfiguration>>();

        public static IDictionary<string, ListQueryFilterConfiguration> GetConfigurationFromTypeProperties(Type type)
        {
            return s_typeCache.GetOrAdd(type, (t) =>
            {
                var schema = new Dictionary<string, ListQueryFilterConfiguration>(StringComparer.OrdinalIgnoreCase);
                bool defaultFilterable = false;
                bool defaultSortable = false;

                if (type.TryGetAttribute(out FilterableAttribute filterableAttr))
                {
                    defaultFilterable = filterableAttr.IsFilterable;
                }

                if (type.TryGetAttribute(out SortableAttribute sortableAttr))
                {
                    defaultSortable = sortableAttr.IsSortable;
                }

                foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    var ci = new ListQueryFilterConfiguration();

                    ci.Name = property.Name;

                    // first see if the property has the DbType attribute, otherwise infer DbType from the property type
                    if (property.TryGetAttribute(out DbTypeAttribute dbTypeAttr))
                    {
                        ci.DataType = dbTypeAttr.DbType;
                        ci.DataTypeSize = dbTypeAttr.Size;
                    }
                    else
                    {
                        ci.DataType = DbConvert.TypeToDbType(property.PropertyType);
                    }

                    // if the property is decorated with StringLength or MaxLength, use that as the size.
                    if (!ci.DataTypeSize.HasValue && property.TryGetAttribute(out System.ComponentModel.DataAnnotations.StringLengthAttribute stringLengthAttr))
                    {
                        ci.DataTypeSize = stringLengthAttr.MaximumLength;
                    }
                    else if (!ci.DataTypeSize.HasValue && property.TryGetAttribute(out System.ComponentModel.DataAnnotations.MaxLengthAttribute maxLengthAttr))
                    {
                        ci.DataTypeSize = maxLengthAttr.Length;
                    }

                    ci.IsFilterable = property.TryGetAttribute(out filterableAttr) ? filterableAttr.IsFilterable : defaultFilterable;
                    ci.IsSortable = property.TryGetAttribute(out sortableAttr) ? sortableAttr.IsSortable : defaultSortable;

                    schema.Add(property.Name, ci);
                }

                return schema;
            });
        }
    }
}