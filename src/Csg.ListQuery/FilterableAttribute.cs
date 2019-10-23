using System;

namespace Csg.ListQuery
{
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class FilterableAttribute : System.Attribute
    {
        public FilterableAttribute(bool filterable = true)
        {
            this.IsFilterable = filterable;
        }

        public bool IsFilterable { get; private set; }

        public string Description { get; set; }
    }
}
