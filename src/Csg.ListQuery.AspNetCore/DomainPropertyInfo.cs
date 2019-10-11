using System;

namespace Csg.ListQuery.AspNetCore
{
    public class DomainPropertyInfo
    {
        public string PropertyName { get; set; }

        public string JsonName { get; set; }

        public bool IsFilterable { get; set; }

        public bool IsSortable { get; set; }

        public Type PropertyType { get => this.Property.PropertyType; }

        public System.Reflection.PropertyInfo Property { get; set; }
    }
}
