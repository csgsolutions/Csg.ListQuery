using Microsoft.VisualStudio.TestTools.UnitTesting;
using Csg.Data.ListQuery.Abstractions;
using System.Collections.Generic;
using Csg.Data.Sql;
using System.Linq;

namespace Csg.Data.ListQuery.Tests
{
    [TestClass]
    public partial class ListQueryTests
    {
        static ListQueryTests()
        {
            Csg.Data.DbQueryBuilder.GenerateFormattedSql = false;
        }

        [TestMethod]
        public void Test_ListQuery_BuildThrowsExceptionWhenNoConfig()
        {
            IDbQueryBuilder query = new Csg.Data.DbQueryBuilder("Person", new Mock.MockConnection());
            var request = new ListQueryDefinition();

            request.Filters = new List<ListQueryFilter>(new ListQueryFilter[] {
                new ListQueryFilter() { Name = "FirstName", Operator = GenericOperator.Equal, Value = "Bob" }
            });

            Assert.ThrowsException<System.Exception>(() =>
            {
                var stmt = ListQueryBuilder.Create(query, request)
                    .Apply();
            });            
        }

        [TestMethod]
        public void Test_ListQuery_BuildWithNoValidation()
        {
            var expectedSql = "SELECT [t0].[FirstName],[t0].[LastName] FROM [dbo].[Person] AS [t0] WHERE (([t0].[FirstName]=@p0));";
            //                 SELECT [t0].[FirstName],[t0].[LastName] FROM [dbo].[Person] AS [t0] WHERE (([t0].[FirstName]=@p0));
            IDbQueryBuilder query = new Csg.Data.DbQueryBuilder("dbo.Person", new Mock.MockConnection());
            var queryDef = new ListQueryDefinition();

            queryDef.Selections = new string[] { "FirstName", "LastName" };
            queryDef.Filters = new List<ListQueryFilter>(new ListQueryFilter[] {
                new ListQueryFilter() { Name = "FirstName", Operator = GenericOperator.Equal, Value = "Bob" }
            });

            var stmt = ListQueryBuilder.Create(query, queryDef)
                .NoValidation()
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
            Assert.AreEqual(1, stmt.Parameters.Count);
        }

        [TestMethod]
        public void Test_ListQuery_DefaultHandler()
        {
            var expectedSql = "SELECT [t0].[FirstName],[t0].[LastName] FROM [dbo].[Person] AS [t0] WHERE (([t0].[FirstName]=@p0));";
            IDbQueryBuilder query = new Csg.Data.DbQueryBuilder("dbo.Person", new Mock.MockConnection());
            var queryDef = new ListQueryDefinition();

            queryDef.Selections = new string[] { "FirstName", "LastName" };
            queryDef.Filters = new List<ListQueryFilter>(new ListQueryFilter[] {
                new ListQueryFilter() { Name = "FirstName", Operator = GenericOperator.Equal, Value = "Bob" }
            });

            var conn = new Mock.MockConnection();

            var stmt = conn
                .QueryBuilder("dbo.Person")
                .ListQuery(queryDef)
                .ValidateWith<Mock.Person>()
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
            Assert.AreEqual(1, stmt.Parameters.Count);
        }

        [TestMethod]
        public void Test_ListQuery_InlineHandler()
        {
            var expectedSql = "SELECT * FROM [dbo].[Person] AS [t0] WHERE (([t0].[PersonID] IN (SELECT [t1].[PersonID] FROM [dbo].[PersonPhoneNumber] AS [t1] WHERE ([t1].[PhoneNumber] LIKE @p0) AND ([t1].[PersonID]=[t0].[PersonID]))));";
            //                 SELECT * FROM [dbo].[Person] AS [t0] WHERE (([t0].[PersonID] IN (SELECT [t1].[PersonID] FROM [dbo].[PersonPhoneNumber] AS [t1] WHERE ([t1].[PhoneNumber] LIKE @p0) AND ([t1].[PersonID]=[t0].[PersonID]))));
            IDbQueryBuilder query = new Csg.Data.DbQueryBuilder("dbo.Person", new Mock.MockConnection());            
            var queryDef = new ListQueryDefinition();

            queryDef.Filters = new List<ListQueryFilter>(new ListQueryFilter[] {
                new ListQueryFilter() { Name = "PhoneNumber", Operator = GenericOperator.Like, Value = "555" }
            });

            var stmt = ListQueryBuilder.Create(query, queryDef)
                .ValidateWith<Mock.Person>()
                .AddFilterHandler("PhoneNumber", Mock.PersonFilters.PhoneNumber)
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
            Assert.AreEqual(1, stmt.Parameters.Count);
        }

        [TestMethod]
        public void Test_ListQuery_TypeDerivedHandler()
        {
            var expectedSql = "SELECT * FROM [dbo].[Person] AS [t0] WHERE (([t0].[PersonID] IN (SELECT [t1].[PersonID] FROM [dbo].[PersonPhoneNumber] AS [t1] WHERE ([t1].[PhoneNumber] LIKE @p0) AND ([t1].[PersonID]=[t0].[PersonID]))));";
            //                 SELECT * FROM [dbo].[Person] AS [t0] WHERE (([t0].[PersonID] IN (SELECT [t1].[PersonID] FROM [dbo].[PersonPhoneNumber] AS [t1] WHERE ([t1].[PhoneNumber] LIKE @p0) AND ([t1].[PersonID]=[t0].[PersonID]))));
            IDbQueryBuilder query = new Csg.Data.DbQueryBuilder("dbo.Person", new Mock.MockConnection());
            var queryDef = new ListQueryDefinition();

            queryDef.Filters = new List<ListQueryFilter>(new ListQueryFilter[] {
                new ListQueryFilter() { Name = "PhoneNumber", Operator = GenericOperator.Like, Value = "555" }
            });

            var stmt = ListQueryBuilder.Create(query, queryDef)
                .ValidateWith<Mock.Person>()
                .AddFilterHandlers<Mock.PersonFilters>()
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
            Assert.AreEqual(1, stmt.Parameters.Count);
        }

        [TestMethod]
        public void Test_ListQuery_Paging()
        {
            var expectedSql = "SELECT COUNT(1) FROM [dbo].[Person] AS [t0] WHERE (([t0].[FirstName]=@p0));\r\nSELECT * FROM [dbo].[Person] AS [t0] WHERE (([t0].[FirstName]=@p1)) ORDER BY [FirstName] ASC OFFSET 50 ROWS FETCH NEXT 25 ROWS ONLY;";
            IDbQueryBuilder query = new Csg.Data.DbQueryBuilder("dbo.Person", new Mock.MockConnection());

            var queryDef = new ListQueryDefinition();

            queryDef.Sort = new List<ListQuerySort>()
            {
               new ListQuerySort(){ Name = "FirstName" }
            };

            queryDef.Filters = new List<ListQueryFilter>()
            {
                new ListQueryFilter(){ Name = "FirstName", Operator = GenericOperator.Equal, Value = "Bob"}
            };

            queryDef.Limit = 25;
            queryDef.Offset = 50;

            var stmt = ListQueryBuilder.Create(query, queryDef)
                .NoValidation()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText.Trim(), true);
        }
    }
}
