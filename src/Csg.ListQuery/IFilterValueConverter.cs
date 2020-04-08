namespace Csg.ListQuery
{
    public interface IFilterValueConverter
    {
        object Convert(object value);

        System.Data.DbType DataType { get; }
    }
}
