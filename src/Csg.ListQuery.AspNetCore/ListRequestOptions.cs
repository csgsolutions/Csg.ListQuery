using System;
using System.Collections.Generic;
using System.Text;

namespace Csg.ListQuery.AspNetCore
{
    public class ListRequestOptions
    {
        public int? MaxLimit { get; set; } = 100;

        public int? DefaultLimit { get; set; } = 25;

    }
}
