namespace FDMS_API.Models.ResponseModel
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
