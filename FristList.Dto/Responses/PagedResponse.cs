using System.Collections.Generic;

namespace FristList.Dto.Responses
{
    public class PagedResponse<T> : Response<IEnumerable<T>>
    {
        public int Page { get; init; }
        public int PageSize { get; init; }
        
        public int TotalPages { get; init; }
        public int TotalRecords { get; init; }

        public PagedResponse(IEnumerable<T> data, int page, int pageSize, int totalPages, int totalRecords) 
            : base(data)
        {
            Page = page;
            PageSize = pageSize;

            TotalPages = totalPages;
            TotalRecords = totalRecords;
        }

        public static PagedResponse<T> Create(IEnumerable<T> data, int page, int pageSize, int totalCount)
        {
            var totalPages = totalCount / pageSize + (totalCount % pageSize > 0 ? 1 : 0);
            return new PagedResponse<T>(data, page, pageSize, totalPages, totalCount);
        }
    }
}