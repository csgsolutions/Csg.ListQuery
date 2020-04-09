using Csg.ListQuery;
using System.Runtime.Serialization;

namespace Csg.ListQuery.Tests.Mock
{
    [DataContract]
    public class DataContractTestModel
    {
        [DataMember]
        [Sortable]
        [Filterable]
        public int DataMember1 { get; set; }

        public string NonDataMember1 { get; set; }

        [Filterable]
        public string NonDataMember2 { get; set; }

        [Sortable]
        public string NonDataMember3 { get; set; }

        [DataMember(Name = "Member2")]
        [Sortable]
        [Filterable]
        public string DataMember2 { get; set; }

        [DataMember]
        public PersonDataContract Person { get;set; }
    }
}
