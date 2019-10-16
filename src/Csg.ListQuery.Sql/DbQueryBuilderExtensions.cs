using Csg.ListQuery.Abstractions;
using Csg.ListQuery.Internal;
using Csg.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csg.Data;
using Csg.ListQuery.Sql.Internal;
using Csg.ListQuery.Sql;

namespace Csg.Data
{
    public static class DbQueryBuilderExtensions
    {
        public static IListQueryBuilder ListQuery(this IDbQueryBuilder queryBuilder, ListQueryDefinition queryDef)
        {
            return Csg.ListQuery.Sql.ListQueryBuilder.Create(queryBuilder, queryDef);
        }
        
        /// <summary>
        /// Sets the columns that will be selected on the given query builder.
        /// </summary>
        /// <param name="queryBuilder"></param>
        /// <param name="columns"></param>
        /// <remarks>This method replaces any existing selected columns on the query builder.</remarks>
        /// <returns></returns>
        public static IDbQueryBuilder Select(this IDbQueryBuilder queryBuilder, params ISqlColumn[] columns)
        {
            var query = queryBuilder.Fork();

            query.SelectColumns.Clear();

            foreach (var col in columns)
            {
                query.SelectColumns.Add(col);
            }

            return query;
        }

        public static ISqlColumn Column(this ISqlTable table, string columnName, string alias)
        {
            return new SqlColumn(table, columnName, alias);
        }

        public static IEnumerable<ISqlColumn> Columns(this ISqlTable table, params string[] columnExpressions)
        {
            foreach (var columnExpr in columnExpressions)
            {
                yield return SqlColumn.Parse(table, columnExpr);
            }
        }
    }
}
