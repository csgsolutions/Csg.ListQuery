using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csg.ListQuery.AspNetCore.Abstractions;
using Csg.ListQuery.AspNetCore.ModelBinding;

namespace Csg.ListQuery.AspNetCore.Tests
{
    public class MockClient : Csg.ListQuery.AspNetCore.Client.IPagedListSupport
    {
        public MockClient()
        {
        }

        public IList<Person> People { get; set; } = new List<Person>();
                
        public Task<IPagedListHateaosResponse<TData>> GetAsync<TData>(string url)
        {
            var factory = new ListRequestFactory();
            var uri = new System.Uri(url);
            var request = factory.CreateRequest<PagedListRequest>(uri.Query);
            
            IEnumerable<TData> dataSource = null;
            if (typeof(TData) == typeof(Person))
            {
                dataSource = this.People.Cast<TData>();
            }
            else
            {
                throw new NotSupportedException();
            }

            var properties = PropertyHelper.GetProperties(typeof(TData));

            var resultData = dataSource.Skip(request.Offset).Take(request.Limit);
            var queryResult = new Csg.ListQuery.Abstractions.ListQueryResult<TData>(
                resultData,
                resultData.Count(),
                dataSource.Count(),
                true,
                dataSource.Count() > request.Offset + request.Limit
            );

            var currentUri = new System.Uri(url);

            return Task.FromResult<IPagedListHateaosResponse<TData>>(
                Csg.ListQuery.AspNetCore.ListResponseExtensions.ToListResponse<TData, TData>(queryResult, request, properties, x=>x, currentUri)
            );
        }

        public Task<IPagedListResponse<TData>> PostAsync<TData>(IPagedListRequest request)
        {
            IEnumerable<TData> dataSource = null;
            if (typeof(TData) == typeof(Person))
            {
                dataSource = this.People.Cast<TData>();
            }
            else
            {
                throw new NotSupportedException();
            }

            var properties = PropertyHelper.GetProperties(typeof(TData));

            var resultData = dataSource.Skip(request.Offset).Take(request.Limit);

            var queryResult = new Csg.ListQuery.Abstractions.ListQueryResult<TData>(
                resultData,
                resultData.Count(),
                dataSource.Count(),
                true,
                dataSource.Count() > request.Offset + request.Limit
            );

            return Task.FromResult<IPagedListResponse<TData>>(
                Csg.ListQuery.AspNetCore.ListResponseExtensions.ToListResponse<TData, TData>(queryResult, request, properties, x => x)
            );
        }
    }
}
