namespace Csg.Data.ListQuery.Abstractions
{
    public class ListQueryFilter
    {
        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        public virtual GenericOperator? Operator { get; set; }

        /// <summary>
        /// Gets or sets the value(s) to be applied as part of the filter. This can be a single scalar value, or an IEnumerable.
        /// </summary>
        public virtual object Value { get; set; }
    }
}
