using Csg.Data.ListQuery.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.Data.ListQuery.Internal
{
    public static class Extensions
    {
        public static Csg.Data.Sql.SqlOperator ToSqlOperator(this GenericOperator @operator)
        {
            Csg.Data.Sql.SqlOperator nativeOperator;

            switch (@operator)
            {
                case GenericOperator.Equal: nativeOperator = Csg.Data.Sql.SqlOperator.Equal; break;
                case GenericOperator.GreaterThan: nativeOperator = Csg.Data.Sql.SqlOperator.GreaterThan; break;
                case GenericOperator.GreaterThanOrEqual: nativeOperator = Csg.Data.Sql.SqlOperator.GreaterThanOrEqual; break;
                case GenericOperator.LessThan: nativeOperator = Csg.Data.Sql.SqlOperator.LessThan; break;
                case GenericOperator.LessThanOrEqual: nativeOperator = Csg.Data.Sql.SqlOperator.LessThanOrEqual; break;
                case GenericOperator.NotEqual: nativeOperator = Csg.Data.Sql.SqlOperator.NotEqual; break;
                default: throw new NotSupportedException($"An unsupported filter operator '{@operator}' was encountered");
            }

            return nativeOperator;
        }

    }
}
