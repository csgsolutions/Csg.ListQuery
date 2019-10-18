using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.ListQuery.AspNetCore.Tests
{
    [TestClass]
    public class AspNetCoreClientTests
    {
        [TestMethod]
        public async Task Test_GetAllPagesAsync()
        {
            var client = new MockClient();
            for (int i = 1; i <= 105; i++)
            {
                client.People.Add(new Person());
            }

            string uri = "https://localhost/api/People/filter?offset=10&limit=10";

            var response = new Abstractions.PagedListHateoasResponse<Person>();
            response.Data = client.People.Take(10);
            response.Links = new Abstractions.PagedListLinks()
            {
                Next = uri
            };
            response.Meta = new Abstractions.PagedListResponseMeta()
            {
                CurrentCount = 10
                //Next = new Abstractions.PageInfo(10)
            };

            var result = await Csg.ListQuery.AspNetCore.Client.ListResponseExtensions.GetAllPagesAsync<Person>(client, response).ConfigureAwait(false);

            Assert.AreEqual(105, result.DataCount);
            Assert.AreEqual(105, result.Data.Count());
            Assert.AreEqual(11, result.RequestCount);
        }

        [TestMethod]
        public async Task Test_PostAllPagesAsync()
        {
            var client = new MockClient();
            
            for (int i = 1; i <= 105; i++)
            {
                client.People.Add(new Person());
            }

            var response = new PagedListRequest()
            {
                Offset = 0,
                Limit = 10
            };

            var result = await Csg.ListQuery.AspNetCore.Client.ListResponseExtensions.PostAllPagesAsync<Person>(client, response).ConfigureAwait(false);

            Assert.AreEqual(105, result.DataCount);
            Assert.AreEqual(105, result.Data.Count());
            Assert.AreEqual(11, result.RequestCount);
        }
    }
}
