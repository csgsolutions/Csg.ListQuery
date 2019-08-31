using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Csg.Data.ListQuery.Abstractions;

namespace Csg.Data.ListQuery
{
    public delegate void ListQueryFilterHandler(Csg.Data.IDbQueryWhereClause where, ListQueryFilter value, ListPropertyInfo config);

    public class ListQuery : IListQuery
    {
        public static IListQuery Create(IDbQueryBuilder queryBuilder, ListQueryDefinition queryDefinition)
        {
            return new ListQuery()
            {
                QueryBuilder = queryBuilder,
                QueryDefinition = queryDefinition
            };
        }

        protected IDbQueryBuilder QueryBuilder { get; set; }

        protected ListQueryDefinition QueryDefinition { get; set; }

        protected IDictionary<string, ListPropertyInfo> Validations = new Dictionary<string, ListPropertyInfo>(StringComparer.OrdinalIgnoreCase);

        protected IDictionary<string, ListQueryFilterHandler> Handlers = new Dictionary<string, ListQueryFilterHandler>(StringComparer.OrdinalIgnoreCase);

        protected bool ShouldValidate { get; set; } = true;

        IDbQueryBuilder IListQuery.QueryBuilder { get => this.QueryBuilder; }

        IDictionary<string, ListQueryFilterHandler> IListQuery.Handlers { get => this.Handlers; }

        ListQueryDefinition IListQuery.QueryDefinition { get => this.QueryDefinition; }

        bool IListQuery.ShouldValidate { get => this.ShouldValidate; set => this.ShouldValidate = value; }

        IDictionary<string, ListPropertyInfo> IListQuery.Validations { get => this.Validations; }
    }

}
