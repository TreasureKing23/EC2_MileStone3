namespace _1903966_Milestone2.Utilities
{
    public class PagedResult<T> where T : class
    {
        public PagedResult()
        {

        }

        public List<T> Data { get; set; }

        public int TotalItems { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

    }
}