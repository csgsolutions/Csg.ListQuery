using System;
using System.Collections.Generic;
using System.Text;

namespace Csg.ListQuery.AspNetCore
{
    public class ListRequestValidationError
    {
        public virtual string Field { get; set; }

        public virtual string Error { get; set; }
    }
}
