using Microsoft.VisualStudio.TestTools.UnitTesting;
using Csg.ListQuery.Abstractions;
using System.Collections.Generic;
using Csg.ListQuery.AspNetCore.Client;
using System.Linq;
using System.Data;

namespace Csg.ListQuery.AspNetCore.Tests
{
    [TestClass]
    public partial class RequestBuilderTests
    {
        [TestMethod]
        public void Test_BuildRequest()
        {
            var builder = new Csg.ListQuery.AspNetCore.Client.ListRequestBuilder();

            var request = builder.Select("FirstName", "LastName")
                .Where("FirstName", "Bob")
                .Where("Age", ListFilterOperator.GreaterThan, 1)
                .Order("LastName", "FirstName")
                .Offset(0)
                .Limit(25)
                .ToQueryString();

        }

    }
}
