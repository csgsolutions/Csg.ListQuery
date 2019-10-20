namespace Csg.ListQuery.AspNetCore.Abstractions
{
    public class ListResponseLinks
    {
        public virtual string Next { get; set; }
        public virtual string Self { get; set; }
        public virtual string Prev { get; set; }
    }
}
