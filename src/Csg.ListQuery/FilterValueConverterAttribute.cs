using System;

namespace Csg.ListQuery
{
    public class FilterValueConverterAttribute : System.Attribute
    {
        public FilterValueConverterAttribute(Type converterType, params object[] arguments)
        {
            this.ConverterType = converterType;
            this.Arguments = arguments;
        }

        public Type ConverterType { get; set; }

        public object[] Arguments { get; set; }

        public IFilterValueConverter CreateConverter()
        {
            if (this.Arguments.Length > 0)
            {
                return (IFilterValueConverter)Activator.CreateInstance(this.ConverterType, this.Arguments);
            }
            else
            {
                return (IFilterValueConverter)Activator.CreateInstance(this.ConverterType);
            }
        }
    }
}
