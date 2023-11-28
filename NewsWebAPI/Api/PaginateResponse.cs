namespace NewsWebAPI.Api
{
    public class PaginateResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItem { get; set; }

        public PaginateResponse(bool status, string message, T data, int pageNumber, int pageSize,int totalItem = 0)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
            Message = message;
            Status = status;
            TotalItem = totalItem;
        }
    }
}