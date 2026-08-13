namespace YAGO.World.Domain.Common.Exceptions
{
    public class YagoNotValidException : YagoException
    {
        public YagoNotValidException(string message)
            : base(message, 400)
        { }
    }
}
