using Csg.ListQuery.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.ListQuery.Client
{
    public abstract class ListRequestBuilder
    {
        public ListRequestBuilder()
        {
            Request = new ListRequest()
            {
                Filters = new List<ListQuery.ListQueryFilter>(),
                Fields = new List<string>(),
                Sort = new List<ListQuery.ListQuerySort>()
            };
        }

        public Csg.ListQuery.Server.ListRequest Request { get; set; }

        public abstract System.Threading.Tasks.Task<Csg.ListQuery.Server.IListResponse<T>> GetResponseAsync<T>(Csg.ListQuery.Server.ListRequest request);
    }
}
