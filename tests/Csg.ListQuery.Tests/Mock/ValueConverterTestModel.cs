using System;
using System.Collections.Generic;
using System.Text;

namespace Csg.ListQuery.Tests.Mock
{
    [Filterable]
    public class ValueConverterTestModel
    {
        [FilterValueConverter(typeof(ExampleConverter))]
        public string MemberNoArgs { get; set; }

        [FilterValueConverter(typeof(ExampleConverter),"Arg1Value")]
        public string MemberWithArgs { get; set; }
    }
}
