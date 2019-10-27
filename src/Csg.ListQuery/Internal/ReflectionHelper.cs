using Csg.ListQuery;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.ListQuery.Internal
{
    /// <summary>
    /// Reflection helpers
    /// </summary>
    public static class ReflectionHelper
    {
        private static System.Collections.Concurrent.ConcurrentDictionary<Type, IDictionary<string, ReflectedFieldMetadata>> s_typeCache 
            = new System.Collections.Concurrent.ConcurrentDictionary<Type, IDictionary<string, ReflectedFieldMetadata>>();

        private static IDictionary<string, ReflectedFieldMetadata> GetListPropertyInfoInternal(Type type)
        {
            var schema = new Dictionary<string, ReflectedFieldMetadata>(StringComparer.OrdinalIgnoreCase);
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
                var ci = new ReflectedFieldMetadata();

                ci.Name = property.Name;
                ci.Description = property.TryGetAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>(out System.ComponentModel.DataAnnotations.DisplayAttribute displayAttr)
                    ? displayAttr.GetDescription()
                    : (string)null;

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

                if (property.TryGetAttribute(out filterableAttr)) 
                {
                    ci.IsFilterable = filterableAttr.IsFilterable;
                    ci.Description = filterableAttr.Description ?? ci.Description;
                }
                else
                {
                    ci.IsFilterable = defaultFilterable;
                }
                
                ci.IsSortable = property.TryGetAttribute(out sortableAttr) ? sortableAttr.IsSortable : defaultSortable;

                ci.PropertyInfo = property;

                schema.Add(property.Name, ci);
            }

            return schema;
        }

        /// <summary>
        /// Gets a dictionary of field metadata created from the properties of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fromCache"></param>
        /// <returns></returns>
        public static IDictionary<string, ReflectedFieldMetadata> GetFieldsFromType(Type type, bool fromCache = true)
        {
            if (!fromCache)
            {
                return GetListPropertyInfoInternal(type);
            }

            return s_typeCache.GetOrAdd(type, GetListPropertyInfoInternal);
        }

        /// <summary>
        /// Removes all cached metadata for the given type.
        /// </summary>
        /// <param name="type"></param>
        public static void RemoveCachedType(Type type)
        {
            s_typeCache.TryRemove(type, out _);
        }

        /// <summary>
        /// Clears all cached metadata.
        /// </summary>
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
