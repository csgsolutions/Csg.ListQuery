using Csg.ListQuery.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.ListQuery.Internal
{
    public static class ReflectionHelper
    {
        private static System.Collections.Concurrent.ConcurrentDictionary<Type, IDictionary<string, ReflectedListPropertyInfo>> s_typeCache 
            = new System.Collections.Concurrent.ConcurrentDictionary<Type, IDictionary<string, ReflectedListPropertyInfo>>();

        private static IDictionary<string, ReflectedListPropertyInfo> GetListPropertyInfoInternal(Type type)
        {
            var schema = new Dictionary<string, ReflectedListPropertyInfo>(StringComparer.OrdinalIgnoreCase);
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
                var ci = new ReflectedListPropertyInfo();

                ci.Name = property.Name;

                // first see if the property has the DbType attribute, otherwise infer DbType from the property type
                if (property.TryGetAttribute(out DbTypeAttribute dbTypeAttr))
                {
                    ci.DataType = dbTypeAttr.DbType;
                    ci.DataTypeSize = dbTypeAttr.Size;
                }
                else
                {
                    ci.DataType = TypeToDbType(property.PropertyType);
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
                ci.PropertyInfo = property;

                schema.Add(property.Name, ci);
            }

            return schema;
        }

        public static IDictionary<string, ReflectedListPropertyInfo> GetListPropertyInfo(Type type, bool fromCache = true)
        {
            if (!fromCache)
            {
                return GetListPropertyInfoInternal(type);
            }

            return s_typeCache.GetOrAdd(type, GetListPropertyInfoInternal);
        }

        public static void RemoveCachedType(Type type)
        {
            s_typeCache.TryRemove(type, out _);
        }

        public static void ClearCachedTypes()
        {
            s_typeCache.Clear();
        }

        /// <summary>
        /// Gets the <see cref="DbType"/> for a System type.
        /// </summary>
        /// <remarks>From https://github.com/csgsolutions/Csg.Data/blob/1a8b0c54047cc645a5b7b42119a64f4c70c31ba5/Csg.Data/DbConvert.cs#L145</remarks>
        /// <param name="type"></param>
        /// <returns></returns>
        public static System.Data.DbType TypeToDbType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type == typeof(byte)) return DbType.Byte;
            if (type == typeof(short)) return DbType.Int16;
            if (type == typeof(int)) return DbType.Int32;
            if (type == typeof(long)) return DbType.Int64;
            if (type == typeof(bool)) return DbType.Boolean;
            if (type == typeof(Guid)) return DbType.Guid;
            if (type == typeof(DateTime)) return DbType.DateTime2;
            if (type == typeof(DateTimeOffset)) return DbType.DateTimeOffset;
            if (type == typeof(TimeSpan)) return DbType.Time;
            if (type == typeof(string)) return DbType.String;
            if (type == typeof(char)) return DbType.StringFixedLength;
            if (type == typeof(byte[])) return DbType.Binary;
            if (type == typeof(decimal)) return DbType.Decimal;
            if (type == typeof(float)) return DbType.Single;
            if (type == typeof(double)) return DbType.Double;

            return DbType.Object;
        }
    }
}
