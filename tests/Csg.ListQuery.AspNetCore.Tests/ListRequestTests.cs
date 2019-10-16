using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Csg.ListQuery.AspNetCore.Tests
{
    [TestClass]
    public class ListRequestTests
    {
        [TestMethod]
        public void Test_PropertyHelper_GetDomainProperties()
        {
            var selectProps = PropertyHelper.GetDomainProperties(typeof(Person));
            var filterProps = PropertyHelper.GetDomainProperties(typeof(PersonFilters));
            var sortProps = PropertyHelper.GetDomainProperties(typeof(PersonSorts));

            Assert.AreEqual(4, selectProps.Count);
            Assert.AreEqual(4, selectProps.Count(x => x.Value.IsSortable));
            Assert.AreEqual(4, selectProps.Count(x => x.Value.IsFilterable));

            Assert.AreEqual(4, filterProps.Count);
            Assert.AreEqual(2, filterProps.Count(x => x.Value.IsFilterable));

            Assert.AreEqual(4, sortProps.Count);
            Assert.AreEqual(2, sortProps.Count(x => x.Value.IsSortable));
        }


        [TestMethod]
        public void Test_Validate_AllArgs()
        {
            var selectProps = PropertyHelper.GetDomainProperties(typeof(Person));
            var filterProps = PropertyHelper.GetDomainProperties(typeof(PersonFilters), x => x.IsFilterable);
            var sortProps = PropertyHelper.GetDomainProperties(typeof(PersonSorts), x => x.IsSortable);
           
            var request = new ListRequest();
            request.Fields = new string[] { "PersonID", "LastName", "FirstName", "BirthDate" };
            request.Filters = new List<ListQuery.Abstractions.ListQueryFilter>
            {
                new ListQuery.Abstractions.ListQueryFilter(){ Name  = "PersonID", Operator = ListQuery.Abstractions.ListFilterOperator.Equal, Value = "test" },
                new ListQuery.Abstractions.ListQueryFilter(){ Name  = "LastName", Operator = ListQuery.Abstractions.ListFilterOperator.Equal, Value = "test" },
                new ListQuery.Abstractions.ListQueryFilter(){ Name  = "FirstName", Operator = ListQuery.Abstractions.ListFilterOperator.Equal, Value = "test" },
                new ListQuery.Abstractions.ListQueryFilter(){ Name  = "BirthDate", Operator = ListQuery.Abstractions.ListFilterOperator.Equal, Value = "test" },
            };
            request.Sort = new List<ListQuery.Abstractions.ListQuerySort>
            {
                new ListQuery.Abstractions.ListQuerySort(){ Name  = "PersonID" },
                new ListQuery.Abstractions.ListQuerySort(){ Name  = "LastName" },
                new ListQuery.Abstractions.ListQuerySort(){ Name  = "FirstName" },
                new ListQuery.Abstractions.ListQuerySort(){ Name  = "BirthDate" },
            };

            var result = request.Validate(selectProps, filterProps, sortProps);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Errors.Count, 4);
        }

        [TestMethod]
        public void Test_Validate_OneGenericArg()
        {
            var request = new ListRequest();
            request.Fields = new string[] { "PersonID","LastName","FirstName","BirthDate" };
            request.Filters = new List<ListQuery.Abstractions.ListQueryFilter>
            {
                new ListQuery.Abstractions.ListQueryFilter(){ Name  = "PersonID", Operator = ListQuery.Abstractions.ListFilterOperator.Equal, Value = "test" },
                new ListQuery.Abstractions.ListQueryFilter(){ Name  = "LastName", Operator = ListQuery.Abstractions.ListFilterOperator.Equal, Value = "test" },
                new ListQuery.Abstractions.ListQueryFilter(){ Name  = "FirstName", Operator = ListQuery.Abstractions.ListFilterOperator.Equal, Value = "test" },
                new ListQuery.Abstractions.ListQueryFilter(){ Name  = "BirthDate", Operator = ListQuery.Abstractions.ListFilterOperator.Equal, Value = "test" },
            };
            request.Sort = new List<ListQuery.Abstractions.ListQuerySort>
            {
                new ListQuery.Abstractions.ListQuerySort(){ Name  = "PersonID" },
                new ListQuery.Abstractions.ListQuerySort(){ Name  = "LastName" },
                new ListQuery.Abstractions.ListQuerySort(){ Name  = "FirstName" },
                new ListQuery.Abstractions.ListQuerySort(){ Name  = "BirthDate" },
            };

            var result = request.Validate<Person>();

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(4, result.ListQuery.Selections.Count());
            Assert.AreEqual(4, result.ListQuery.Filters.Count());
            Assert.AreEqual(4, result.ListQuery.Sort.Count());
        }

        [TestMethod]
        public void Test_ToListQuery_TwoGenericArgs()
        {
            var request = new ListRequest();
            request.Fields = new string[] { "PersonID", "LastName", "FirstName", "BirthDate" };
            request.Filters = new List<ListQuery.Abstractions.ListQueryFilter>
            {
                new ListQuery.Abstractions.ListQueryFilter(){ Name  = "PersonID", Operator = ListQuery.Abstractions.ListFilterOperator.Equal, Value = "test" },
                new ListQuery.Abstractions.ListQueryFilter(){ Name  = "LastName", Operator = ListQuery.Abstractions.ListFilterOperator.Equal, Value = "test" },
                new ListQuery.Abstractions.ListQueryFilter(){ Name  = "FirstName", Operator = ListQuery.Abstractions.ListFilterOperator.Equal, Value = "test" },
                new ListQuery.Abstractions.ListQueryFilter(){ Name  = "BirthDate", Operator = ListQuery.Abstractions.ListFilterOperator.Equal, Value = "test" },
            };
            request.Sort = new List<ListQuery.Abstractions.ListQuerySort>
            {
                new ListQuery.Abstractions.ListQuerySort(){ Name  = "PersonID" },
                new ListQuery.Abstractions.ListQuerySort(){ Name  = "LastName" },
                new ListQuery.Abstractions.ListQuerySort(){ Name  = "FirstName" },
                new ListQuery.Abstractions.ListQuerySort(){ Name  = "BirthDate" },
            };

            var result = request.Validate<Person, PersonFilters>();

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Errors.Count, 2);
            Assert.AreEqual(4, result.ListQuery.Selections.Count());
            Assert.AreEqual(2, result.ListQuery.Filters.Count());
            Assert.AreEqual(4, result.ListQuery.Sort.Count());
        }


        [TestMethod]
        public void Test_ToListQuery_ThreeGenericArgs()
        {
            var request = new ListRequest();
            request.Fields = new string[] { "PersonID", "LastName", "FirstName", "BirthDate" };
            request.Filters = new List<ListQuery.Abstractions.ListQueryFilter>
            {
                new ListQuery.Abstractions.ListQueryFilter(){ Name  = "PersonID", Operator = ListQuery.Abstractions.ListFilterOperator.Equal, Value = "test" },
                new ListQuery.Abstractions.ListQueryFilter(){ Name  = "LastName", Operator = ListQuery.Abstractions.ListFilterOperator.Equal, Value = "test" },
                new ListQuery.Abstractions.ListQueryFilter(){ Name  = "FirstName", Operator = ListQuery.Abstractions.ListFilterOperator.Equal, Value = "test" },
                new ListQuery.Abstractions.ListQueryFilter(){ Name  = "BirthDate", Operator = ListQuery.Abstractions.ListFilterOperator.Equal, Value = "test" },
            };
            request.Sort = new List<ListQuery.Abstractions.ListQuerySort>
            {
                new ListQuery.Abstractions.ListQuerySort(){ Name  = "PersonID" },
                new ListQuery.Abstractions.ListQuerySort(){ Name  = "LastName" },
                new ListQuery.Abstractions.ListQuerySort(){ Name  = "FirstName" },
                new ListQuery.Abstractions.ListQuerySort(){ Name  = "BirthDate" },
            };

            var result = request.Validate<Person, PersonFilters, PersonSorts>();

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.Errors.Count, 4);
            Assert.AreEqual(4, result.ListQuery.Selections.Count());
            Assert.AreEqual(2, result.ListQuery.Filters.Count());
            Assert.AreEqual(2, result.ListQuery.Sort.Count());
        }
    }
}
