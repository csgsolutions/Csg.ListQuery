namespace Csg.ListQuery.AspNetCore.Abstractions
{
    public class PagedListLinks
    {
        public virtual string Next { get; set; }
        public virtual string Self { get; set; }
        public virtual string Prev { get; set; }
    }
}
