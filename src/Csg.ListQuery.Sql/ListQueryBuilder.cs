using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Csg.ListQuery;
using Csg.Data;
using Csg.ListQuery.Internal;

namespace Csg.ListQuery.Sql
{
    /// <summary>
    /// Represents a method that handles application of a <see cref="ListFilter"/> to a given <see cref="IDbQueryWhereClause"/>.
    /// </summary>
    /// <param name="where"></param>
    /// <param name="value"></param>
    /// <param name="config"></param>
    public delegate void ListQueryFilterHandler(Csg.Data.Abstractions.IWhereClause where, ListFilter value, ListFieldMetadata config);

    public class ListQueryBuilder : IListQueryBuilder
    {
        /// <summary>
        /// Creates a <see cref="ListQueryBuilder"/> from the given query builder and query definition.
        /// </summary>
        /// <param name="queryBuilder"></param>
        /// <param name="queryDefinition"></param>
        /// <returns></returns>
        public static IListQueryBuilder Create(Csg.Data.Common.IDbQueryBuilder queryBuilder, ListQueryDefinition queryDefinition)
        {
            var dataAdapter = new DapperListDataAdapter(queryBuilder);

            return Create(queryBuilder, dataAdapter, queryDefinition);
        }

        /// <summary>
        /// Creates a <see cref="ListQueryBuilder"/> from the given query builder and query definition.
        /// </summary>
        /// <param name="queryBuilder"></param>
        /// <param name="queryDefinition"></param>
        /// <returns></returns>
        public static IListQueryBuilder Create(Csg.Data.Abstractions.ISelectQueryBuilder queryBuilder, IListQueryDataAdapter dataAdapter, ListQueryDefinition queryDefinition)
        {
            return new ListQueryBuilder()
            {
                Configuration = new ListQueryBuilderConfiguration()
                {
                    QueryBuilder = queryBuilder,
                    DataAdapter = dataAdapter,
                    QueryDefinition = queryDefinition,
                    Validations = new Dictionary<string, ListFieldMetadata>(StringComparer.OrdinalIgnoreCase),
                    Handlers = new Dictionary<string, ListQueryFilterHandler>(StringComparer.OrdinalIgnoreCase)
                }
            };
        }

        /// <summary>
        /// Gets the fully qualified data field name for the given field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        protected virtual string GetDataFieldName(string name, ListFieldMetadata metaData)
        {
            if (metaData == null)
            {
                return name;
            }

            return metaData.DataMemberName ?? metaData.Name;
        }

        string IListQueryBuilder.GetDataFieldName(string name, ListFieldMetadata metaData)
        {
            return GetDataFieldName(name, metaData);
        }

        /// <summary>
        /// Gets or sets the list builder configuration.
        /// </summary>
        protected virtual ListQueryBuilderConfiguration Configuration { get; set; }

        ListQueryBuilderConfiguration IListQueryBuilder.Configuration { get => this.Configuration; }
    }
}
