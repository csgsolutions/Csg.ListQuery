using System;

namespace Csg.ListQuery.AspNetCore
{
    public class ListItemPropertyInfo
    {
        public virtual string PropertyName { get; set; }

        public virtual string JsonName { get; set; }

        public virtual bool IsFilterable { get; set; }

        public virtual bool IsSortable { get; set; }

        public virtual Type PropertyType { get => this.Property.PropertyType; }

        public virtual System.Reflection.PropertyInfo Property { get; set; }
    }
}
