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

        
    }
}
