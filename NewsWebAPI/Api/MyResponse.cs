namespace NewsWebAPI.Api
{
    public class MyResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public MyResponse(bool status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }

}
