using Csg.Data.ListQuery.Abstractions;

namespace Csg.Data.ListQuery.Tests.Mock
{
    public class PersonFilters
    {
        public static void PhoneNumber(IDbQueryWhereClause where, ListQueryFilter filter, ListPropertyInfo config)
        {
            var sqf = new Sql.SqlSubQueryFilter(where.Root, "dbo.PersonPhoneNumber");
            sqf.ColumnName = "PersonID";
            sqf.SubQueryColumn = "PersonID";
            sqf.SubQueryFilters.Add(new Sql.SqlStringMatchFilter(sqf.SubQueryTable, "PhoneNumber", Sql.SqlWildcardDecoration.Contains, filter.Value.ToString()));
            sqf.SubQueryFilters.Add(new Sql.SqlColumnCompareFilter(sqf.SubQueryTable, "PersonID", Sql.SqlOperator.Equal, sqf.Table));
            where.AddFilter(sqf);
        }
    }
}
