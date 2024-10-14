using FDMS_API.Models.ResponseModel;
using System.ComponentModel;

namespace FDMS_API.Extentions
{
    public static class PaginationExtention
    {
        public static PagedResult<T> Pagination<T>(IEnumerable<T> items, int pageSize, int currentPage)
        {
            int totalCount = items.Count();
            int totalPage = (int)Math.Ceiling((double)totalCount / pageSize);
            int startIndex = (currentPage - 1) * pageSize;

            return new PagedResult<T>
            {
                TotalCount = totalCount,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPage = totalPage,
                Items = items.Skip(startIndex).Take(pageSize)
            };
        }
    }
}
