namespace Csg.Data.ListQuery.Abstractions
{
    public class FilterableAttribute : System.Attribute
    {
        public FilterableAttribute(bool filterable = true)
        {
            this.IsFilterable = filterable;
        }

        public bool IsFilterable { get; private set; }

    }
}
