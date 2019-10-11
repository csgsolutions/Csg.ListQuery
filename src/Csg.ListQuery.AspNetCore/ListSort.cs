using System;
using System.Collections.Generic;
using System.Text;

namespace Csg.ListQuery.AspNetCore
{
    public class ListSort<TFields>
    {
        public virtual TFields Name { get; set; }

        public virtual bool SortDescending { get; set; }
    }
}
