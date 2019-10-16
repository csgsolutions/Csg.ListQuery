using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Csg.ListQuery.AspNetCore;

namespace Csg.ListQuery.AspNetCore.Client
{
    public class ListRequestClient 
    {
        private HttpClient _httpClient;


        public ListRequestClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        
        protected IDictionary<string, string> CreateQueryFromRequest(ListRequestMessage request)
        {
            throw new NotImplementedException();
        }

        protected async Task<PagedListResponse<T>> GetAsync<T>(string path, IDictionary<string, string> queryString)
        {
            throw new NotImplementedException();
            var response = await _httpClient.GetAsync($"{path}/filter{queryString}");
            var responseText = await response.Content.ReadAsStringAsync();
        }


        public async Task<ListResponse<T>> GetAsync<T>(string url, ListRequestMessage request)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedListResponse<T>> GetAllAsync<T>(string path, ListRequestMessage request)
        {
            int totalCount = 0;
            var requestQuery = CreateQueryFromRequest(request);
            var response = await GetAsync<T>(path, requestQuery);
            var result = new PagedListResponse<T>();

            result.Data = response.Data;
            result.Fields = response.Fields;

            while(response.Meta.Next?.Offset > 0)
            {
                // set the next offset
                requestQuery["offset"] = response.Meta.Next?.Offset.ToString();
                
                // get the next page
                response = await GetAsync<T>(path, requestQuery);
                
                // concat with existing page(s)
                result.Data = result.Data.Concat(response.Data);
                totalCount += response.Meta.Count;
            }

            result.Meta.Add("total_count", totalCount.ToString());

            return response;
        }
               
        public async Task<ListResponse<T>> PostAsync<T>(string url, ListRequestMessage request)
        {
            throw new NotImplementedException();
        }

        public async Task<ListResponse<T>> PostAllAsync<T>(string path, ListRequestMessage request)
        {
            throw new NotImplementedException();
        }
    }
}
