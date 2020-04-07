using Csg.ListQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebAPI
{
    public class Person
    {
        [Filterable]
        public int PersonID { get; set; }

        [Sortable]
        public string FirstName { get; set; }

        [Sortable]
        public string LastName { get; set; }
    }
}
