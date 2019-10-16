using System;
using System.Collections.Generic;
using System.Text;

namespace Csg.ListQuery.AspNetCore
{
    public class ListRequestValidationError
    {
        public string Field { get; set; }

        public string Error { get; set; }
    }
}
