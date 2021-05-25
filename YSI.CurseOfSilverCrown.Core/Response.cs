using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSI.CurseOfSilverCrown.Core
{
    public class Response<T>
    {
        public bool Success { get; }

        public T Result { get; }

        public string Error { get; }

        public Exception Exception { get; }

        public Response(T result)
        {
            Success = true;
            Result = result;
        }

        public Response(string error, Exception exception)
        {
            Success = false;
            Error = error;
            Exception = exception;
        }
    }
}
