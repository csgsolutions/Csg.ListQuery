using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Csg.Data.ListQuery.Abstractions;

namespace Csg.Data.ListQuery
{
    public delegate void ListQueryFilterHandler(Csg.Data.IDbQueryWhereClause where, ListQueryFilter value, ListQueryFilterConfiguration config);

    public class ListQuery : IListQuery
    {
        public static IListQuery Create(IDbQueryBuilder queryBuilder, QueryDefinition queryDefinition)
        {
            return new ListQuery()
            {
                QueryBuilder = queryBuilder,
                QueryDefinition = queryDefinition
            };
        }

        protected IDbQueryBuilder QueryBuilder { get; set; }

        protected QueryDefinition QueryDefinition { get; set; }

        protected IDictionary<string, ListQueryFilterConfiguration> Validations = new Dictionary<string, ListQueryFilterConfiguration>(StringComparer.OrdinalIgnoreCase);

        protected IDictionary<string, ListQueryFilterHandler> Handlers = new Dictionary<string, ListQueryFilterHandler>(StringComparer.OrdinalIgnoreCase);

        protected bool ShouldValidate { get; set; } = true;

        IDbQueryBuilder IListQuery.QueryBuilder { get => this.QueryBuilder; }

        IDictionary<string, ListQueryFilterHandler> IListQuery.Handlers { get => this.Handlers; }

        QueryDefinition IListQuery.QueryDefinition { get => this.QueryDefinition; }

        bool IListQuery.ShouldValidate { get => this.ShouldValidate; set => this.ShouldValidate = value; }

        IDictionary<string, ListQueryFilterConfiguration> IListQuery.Validations { get => this.Validations; }
    }

}
