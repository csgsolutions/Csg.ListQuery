using System;

namespace Csg.ListQuery.Abstractions
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
        public virtual ListFilterOperator? Operator { get; set; }

        /// <summary>
        /// Gets or sets the value(s) to be applied as part of the filter. This can be a single scalar value, or an IEnumerable.
        /// </summary>
        public virtual object Value { get; set; }

        /// <summary>
        /// Gets the ListQuery standard sstring representation of the given operator.
        /// </summary>
        /// <param name="oper"></param>
        /// <returns></returns>
        public static string OperatorToString(ListQuery.Abstractions.ListFilterOperator? oper)
        {
            if (!oper.HasValue)
            {
                return string.Empty;
            }

            switch (oper.Value)
            {
                case ListQuery.Abstractions.ListFilterOperator.Equal: return UrlElements.op_eq;
                case ListQuery.Abstractions.ListFilterOperator.NotEqual: return UrlElements.op_ne;
                case ListQuery.Abstractions.ListFilterOperator.GreaterThan: return UrlElements.op_gt;
                case ListQuery.Abstractions.ListFilterOperator.GreaterThanOrEqual: return UrlElements.op_ge;
                case ListQuery.Abstractions.ListFilterOperator.LessThan: return UrlElements.op_lt;
                case ListQuery.Abstractions.ListFilterOperator.LessThanOrEqual: return UrlElements.op_le;
                case ListQuery.Abstractions.ListFilterOperator.Like: return UrlElements.op_like;
                case ListQuery.Abstractions.ListFilterOperator.IsNull: return UrlElements.op_isnull;
            }

            throw new NotSupportedException($"Unsupported filter operator: {oper}");
        }
    }
}
