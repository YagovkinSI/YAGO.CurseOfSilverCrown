namespace YSI.CurseOfSilverCrown.Core
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public T Result { get; set; }
        public string Error { get; set; }

        public Response(T result)
        {
            Success = true;
            Result = result;
        }

        public Response(string error)
        {
            Success = false;
            Error = error;
        }
    }
}
