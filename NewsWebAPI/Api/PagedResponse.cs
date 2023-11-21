namespace NewsWebAPI.Api
{
    public class PagedResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public string FirstPageUrl => PageUrl(1);
        public string LastPageUrl => PageUrl(TotalPages);

        // Các URL của trang trước và trang tiếp theo
        public string PrevPageUrl => PageUrl(PageNumber - 1);
        public string NextPageUrl => PageUrl(PageNumber + 1);
        public PagedResponse(bool status, string message, List<T> data, int pageNumber, int pageSize, int totalItems)
        {
            Status = status;
            Message = message;
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
        }

        private string PageUrl(int pageNumber)
        {
            if (pageNumber < 1 || pageNumber > TotalPages)
            {
                return null;
            }

            return $"https://localhost:7020/api/articles?page={pageNumber}";
        }
    }

}
