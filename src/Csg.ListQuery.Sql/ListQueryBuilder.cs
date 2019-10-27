using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Csg.ListQuery;
using Csg.Data;

namespace Csg.ListQuery.Sql
{
    /// <summary>
    /// Represents a method that handles application of a <see cref="ListFilter"/> to a given <see cref="IDbQueryWhereClause"/>.
    /// </summary>
    /// <param name="where"></param>
    /// <param name="value"></param>
    /// <param name="config"></param>
    public delegate void ListQueryFilterHandler(Csg.Data.IDbQueryWhereClause where, ListFilter value, ListFieldMetadata config);

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
                    Validations = new Dictionary<string, ListFieldMetadata>(StringComparer.OrdinalIgnoreCase),
                    Handlers = new Dictionary<string, ListQueryFilterHandler>(StringComparer.OrdinalIgnoreCase)
                }
            };
        }

        protected virtual ListQueryBuilderConfiguration Configuration { get; set; }

        ListQueryBuilderConfiguration IListQueryBuilder.Configuration { get => this.Configuration; }
    }
}
