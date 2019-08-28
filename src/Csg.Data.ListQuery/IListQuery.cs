using System.Collections.Generic;
using Csg.Data.ListQuery.Abstractions;

namespace Csg.Data.ListQuery
{
    public interface IListQuery
    {
        IDbQueryBuilder QueryBuilder { get; }

        IDictionary<string, ListQueryFilterHandler> Handlers { get; }

        IDictionary<string, ListQueryFilterConfiguration> Validations { get; }

        QueryDefinition QueryDefinition { get; }

        bool ShouldValidate { get; set; }
    }

}
