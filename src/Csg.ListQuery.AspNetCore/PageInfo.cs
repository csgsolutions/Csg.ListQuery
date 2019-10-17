namespace Csg.ListQuery.AspNetCore
{
    public struct PageInfo
    {
        private int _offset;

        public PageInfo(int offset)
        {
            _offset = offset;
        }

        public int Offset => _offset;
    }
}
