using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Csg.ListQuery.Server;

namespace Csg.ListQuery.AspNetCore.Tests.Mock
{
    public class MockRequestBuilder : Csg.ListQuery.Client.ListRequestBuilder
    {
        public override Task<IListResponse<T>> GetResponseAsync<T>(ListRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
