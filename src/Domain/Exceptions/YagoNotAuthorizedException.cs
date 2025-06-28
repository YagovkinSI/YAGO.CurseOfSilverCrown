namespace YAGO.World.Domain.Exceptions
{
    public class YagoNotAuthorizedException : YagoException
    {
        public YagoNotAuthorizedException()
            : base("Необходимо авторизоваться.")
        {
        }
    }
}
