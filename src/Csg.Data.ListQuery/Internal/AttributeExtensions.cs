using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Csg.Data.ListQuery.Internal
{
    public static class AttributeExtensions
    {
        public static bool TryGetAttribute<T>(this PropertyInfo property, out T attr) where T : Attribute
        {
            attr = property.GetCustomAttribute<T>();

            if (attr == null)
            {
                return false;
            }

            return true;
        }

        public static bool TryGetAttribute<T>(this Type type, out T attr) where T : Attribute
        {
            attr = type.GetCustomAttribute<T>();

            if (attr == null)
            {
                return false;
            }

            return true;
        }
    }
}
