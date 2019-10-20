using System;
using System.Collections;
using System.Collections.Generic;
using Csg.ListQuery.AspNetCore.Abstractions;

namespace Csg.ListQuery.AspNetCore.Abstractions
{
    public class ListResponse<T> : IListResponse<T>
    {
        public ListResponse()
        {
        }

        public ListResponse(IListRequest request, IEnumerable<T> data)
        {
            this.Data = data;
            this.Meta = this.Meta ?? new ListResponseMeta();
            this.Meta.Fields = request.Fields;
        }

        public virtual IEnumerable<T> Data { get; set; }

        public virtual ListResponseMeta Meta { get; set; }

        public virtual ListResponseLinks Links { get; set; }
    }
}
