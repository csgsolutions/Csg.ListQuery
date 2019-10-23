using System;

namespace Csg.ListQuery
{
    [System.AttributeUsage(AttributeTargets.Property)]
    public class DbTypeAttribute : System.Attribute
    {
        private readonly System.Data.DbType _type;
        private readonly int? _size;

        public DbTypeAttribute(System.Data.DbType type, int size = 0)
        {
            _type = type;
            _size = size > 0 ? (int?)size : null;
        }

        public System.Data.DbType DbType
        {
            get
            {
                return _type;
            }
        }

        public int? Size
        {
            get
            {
                return _size;
            }
        }
    }
}
