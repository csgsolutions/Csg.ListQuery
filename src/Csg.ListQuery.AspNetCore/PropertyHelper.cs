using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csg.ListQuery.AspNetCore
{
    public static class PropertyHelper
    {
        public static Dictionary<string, DomainPropertyInfo> GetDomainProperties(Type type)
        {
            var listConfigs = Csg.ListQuery.Internal.ReflectionHelper.GetListPropertyInfo(type, fromCache: true);
           
            return listConfigs.Values
                .Select(prop =>
                {
                    var propInfo = new DomainPropertyInfo();
                    var jsonPropertyAttribute = prop.PropertyInfo.GetCustomAttributes(typeof(Newtonsoft.Json.JsonPropertyAttribute), false).FirstOrDefault();

                    propInfo.Property = prop.PropertyInfo;
                    propInfo.PropertyName = prop.Name;                    
                    propInfo.JsonName = ((Newtonsoft.Json.JsonPropertyAttribute)jsonPropertyAttribute)?.PropertyName ?? prop.Name;
                    propInfo.IsFilterable = prop.IsFilterable == true;
                    propInfo.IsSortable = prop.IsSortable == true;

                    return propInfo;
                })
                .ToDictionary(k => k.PropertyName, StringComparer.OrdinalIgnoreCase);
        }
    }
}
