using Csg.Data;

namespace Csg.ListQuery.Sql.Internal
{
    public delegate void ApplyEventHandler(object sender, ApplyEventArgs args);

    public class ApplyEventArgs : System.EventArgs
    {
        public ApplyEventArgs(ListQueryBuilderConfiguration config)
        {
            this.Configuration = config;
            this.QueryBuilder = config.QueryBuilder;
        }

        public ApplyEventArgs(ListQueryBuilderConfiguration config, IDbQueryBuilder queryBuilder)
        {
            this.Configuration = config;
            this.QueryBuilder = queryBuilder;
        }

        public ListQueryBuilderConfiguration Configuration { get; set; }

        public IDbQueryBuilder QueryBuilder { get; set; }
    }
}
