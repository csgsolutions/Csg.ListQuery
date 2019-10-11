using Csg.ListQuery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Csg.ListQuery.AspNetCore
{
    public class ListFilter<TField>
    {
        public virtual TField Name { get; set; }
        public virtual ListFilterOperator? Operator { get; set; }
        public virtual object Value { get; set; }
    }
}
