using System;

namespace YAGO.World.Domain.Exceptions
{
    public class YagoException : Exception
    {
        public YagoException(string message)
            : base(message)
        { }
    }
}
