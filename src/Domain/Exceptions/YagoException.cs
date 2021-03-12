using System;

namespace YAGO.World.Domain.Exceptions
{
    public class YagoException : Exception
    {
        public int? ErrorCode { get; }

        public YagoException(string message, int? errorCode = null)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
