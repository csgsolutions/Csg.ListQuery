using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Csg.ListQuery.Tests.Mock
{
    public class ExampleConverter : IFilterValueConverter
    {
        public ExampleConverter()
        {
            this.Arg1 = "none";
        }

        public ExampleConverter(string arg1)
        {
            this.Arg1 = arg1;
        }

        public string Arg1 { get; set; }

        public DbType DataType => DbType.String;

        public object Convert(object value)
        {
            return string.Concat(this.Arg1, "_",value.ToString());
        }
    }
}
