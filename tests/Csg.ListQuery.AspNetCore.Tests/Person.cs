using System;
using System.Collections.Generic;
using System.Text;

namespace Csg.ListQuery.AspNetCore.Tests
{
    public class Person
    {
        public int PersonID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTimeOffset BirthDate { get; set; }
    }
}
