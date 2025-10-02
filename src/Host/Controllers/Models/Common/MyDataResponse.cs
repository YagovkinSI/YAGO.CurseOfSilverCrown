namespace YAGO.World.Application.Common.Models
{
    public record MyDataResponse<T>(
        bool IsAuthorized,
        T? Data)
        where T : class 
    {
        public static MyDataResponse<T> NotAuthorized => new(IsAuthorized: false, Data: null);
    }
}
