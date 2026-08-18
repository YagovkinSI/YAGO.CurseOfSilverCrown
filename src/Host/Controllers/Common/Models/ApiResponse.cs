namespace YAGO.World.Host.Controllers.Common.Models
{
    public record ApiResponse<T>(T? Data)
        where T : class
    {
        public static ApiResponse<T> CreateSuccess(T? data) => new(Data: data);

        public static ApiResponse<T> Empty => new(Data: null);
    }
}
