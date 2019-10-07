using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Csg.Data.ListQuery.Abstractions;

namespace Csg.Data.ListQuery
{
    /// <summary>
    /// Represents a method that handles application of a <see cref="ListQueryFilter"/> to a given <see cref="IDbQueryWhereClause"/>.
    /// </summary>
    /// <param name="where"></param>
    /// <param name="value"></param>
    /// <param name="config"></param>
    public delegate void ListQueryFilterHandler(Csg.Data.IDbQueryWhereClause where, ListQueryFilter value, ListPropertyInfo config);

    public class ListQueryBuilder : IListQueryBuilder
    {
        public static IListQueryBuilder Create(IDbQueryBuilder queryBuilder, ListQueryDefinition queryDefinition)
        {
            return new ListQueryBuilder()
            {
                Configuration = new ListQueryBuilderConfiguration()
                {
                    QueryBuilder = queryBuilder,
                    QueryDefinition = queryDefinition,
                    Validations = new Dictionary<string, ListPropertyInfo>(StringComparer.OrdinalIgnoreCase),
                    Handlers = new Dictionary<string, ListQueryFilterHandler>(StringComparer.OrdinalIgnoreCase)
                }
            };
        }

        protected virtual ListQueryBuilderConfiguration Configuration { get; set; }

        ListQueryBuilderConfiguration IListQueryBuilder.Configuration { get => this.Configuration; }
    }
}
