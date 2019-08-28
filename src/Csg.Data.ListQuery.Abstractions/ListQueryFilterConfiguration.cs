namespace Csg.Data.ListQuery.Abstractions
{
    public class ListQueryFilterConfiguration
    {
        /// <summary>
        /// Gets or sets the column name as defined in the provider (database).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the provider specific type name (e.g. varchar)
        /// </summary>
        public System.Data.DbType DataType { get; set; }

        /// <summary>
        /// Gets or sets the data type size of the column, if applicable.
        /// </summary>
        public int? DataTypeSize { get; set; }

        public bool? IsFilterable { get; set; }

        public bool? IsSortable { get; set; }
    }
}
