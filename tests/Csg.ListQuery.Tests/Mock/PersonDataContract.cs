using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Csg.ListQuery.Tests.Mock
{
    [DataContract]
    public class PersonDataContract
    {
        [DataMember(Name ="ID")]
        public string PersonID { get; set; }
    }
}
