using Csg.ListQuery;
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
    /// <summary>
    /// Extension methods for the <see cref="IDbQueryBuilder"/>.
    /// </summary>
    public static class DbQueryBuilderExtensions
    {
        public static IListQueryBuilder ListQuery(this Csg.Data.Common.IDbQueryBuilder queryBuilder, ListQueryDefinition queryDef)
        {
            return Csg.ListQuery.Sql.ListQueryBuilder.Create(queryBuilder, queryDef);
        }
        
        /// <summary>
        /// Sets the columns that will be selected on the given query builder and replaces any existing selections.
        /// </summary>
        /// <param name="queryBuilder"></param>
        /// <param name="columns"></param>
        /// <remarks>This method replaces any existing selected columns on the query builder.</remarks>
        /// <returns></returns>
        public static Csg.Data.Abstractions.ISelectQueryBuilder SelectOnly(this Csg.Data.Abstractions.ISelectQueryBuilder queryBuilder, params ISqlColumn[] columns)
        {
            var query = queryBuilder.Fork();

            query.Configuration.SelectColumns.Clear();

            foreach (var col in columns)
            {
                query.Configuration.SelectColumns.Add(col);
            }

            return query;
        }

        /// <summary>
        /// Sets the columns that will be selected on the given query builder and replaces any existing selections.
        /// </summary>
        /// <param name="queryBuilder"></param>
        /// <param name="columns"></param>
        /// <remarks>This method replaces any existing selected columns on the query builder.</remarks>
        /// <returns></returns>
        public static Csg.Data.Abstractions.ISelectQueryBuilder SelectOnly(this Csg.Data.Abstractions.ISelectQueryBuilder queryBuilder, params string[] columns)
        {
            return SelectOnly(queryBuilder, queryBuilder.Root.Columns(columns).ToArray());
        }

        /// <summary>
        /// Applies filters to the given where clause and joins them together in OR logic.
        /// </summary>
        /// <param name="where"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public static Csg.Data.Abstractions.IWhereClause Any(this Csg.Data.Abstractions.IWhereClause where, Action<Csg.Data.Abstractions.IWhereClause> whereClause)
        {
            var orWhere = new Csg.Data.WhereClause(where.Root, SqlLogic.Or);

            whereClause(orWhere);
            where.AddFilter(orWhere.Filters);

            return where;
        }

        /// <summary>
        /// Adds the given filter to a query builder where clause.
        /// </summary>
        /// <param name="where"></param>
        /// <param name="filter"></param>
        /// <param name="valueType"></param>
        /// <param name="valueTypeSize"></param>
        /// <param name="stringMatchType"></param>
        /// <param name="performDataTypeConversion"></param>
        /// <param name="valueConverter"></param>
        /// <returns></returns>
        public static Csg.Data.Abstractions.IWhereClause AddFilter(this Csg.Data.Abstractions.IWhereClause where, ListFilter filter, System.Data.DbType valueType, int? valueTypeSize = null, SqlWildcardDecoration stringMatchType = SqlWildcardDecoration.BeginsWith, bool performDataTypeConversion = true, Func<object, object> valueConverter = null)
        {
            Csg.ListQuery.Sql.Internal.Extensions.AddFilter(where, filter.Name, filter.Operator ?? ListFilterOperator.Equal, filter.Value, valueType: valueType, valueTypeSize: valueTypeSize, stringMatchType: stringMatchType, performDataTypeConversion: performDataTypeConversion, valueConverter: valueConverter);
            return where;
        }
    }
}
