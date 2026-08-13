namespace YAGO.World.Domain.Common.Exceptions
{
    public class YagoNotAuthorizedException : YagoException
    {
        public YagoNotAuthorizedException()
            : base("Необходимо авторизоваться.")
        { }
    }
}
