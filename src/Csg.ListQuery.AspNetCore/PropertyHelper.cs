using Csg.ListQuery.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csg.ListQuery.AspNetCore
{
    /// <summary>
    /// Helper methods for property reflection
    /// </summary>
    public static class PropertyHelper
    {
        /// <summary>
        /// Gets a list of the properties for a give type and optionally matching the given predicate.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static Dictionary<string, ListItemPropertyInfo> GetProperties(Type type, Func<ListItemPropertyInfo, bool> predicate = null)
        {
            var listConfigs = Csg.ListQuery.Internal.ReflectionHelper.GetListPropertyInfo(type, fromCache: true);
           
            return listConfigs.Values
                .Select(prop =>
                {
                    var propInfo = new ListItemPropertyInfo();
                    var jsonPropertyAttribute = prop.PropertyInfo.GetCustomAttributes(typeof(Newtonsoft.Json.JsonPropertyAttribute), false).FirstOrDefault();

                    propInfo.Property = prop.PropertyInfo;
                    propInfo.PropertyName = prop.Name;                    
                    propInfo.JsonName = ((Newtonsoft.Json.JsonPropertyAttribute)jsonPropertyAttribute)?.PropertyName ?? prop.Name;
                    propInfo.IsFilterable = prop.IsFilterable == true;
                    propInfo.IsSortable = prop.IsSortable == true;
                    propInfo.Description = prop.Description;

                    return propInfo;
                })
                .Where(x => predicate == null || predicate(x))
                .ToDictionary(k => k.PropertyName, StringComparer.OrdinalIgnoreCase);
        }
    }
}
