namespace UniStore.Models
{
    public class Pagination
    {
        public int Page { get; set; }

        public int PageCount { get; set; }

        public string Search { get; set; }

        public string Order { get; set; }

        public string OrderBy { get; set; }
    }
}