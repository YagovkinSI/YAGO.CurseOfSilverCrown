namespace YAGO.World.Host.Controllers.Common
{
    public record MyDataResponse<T>(
        bool IsAuthorized,
        T? Data)
        where T : class 
    {
        public static MyDataResponse<T> NotAuthorized => new(IsAuthorized: false, Data: null);
    }
}
