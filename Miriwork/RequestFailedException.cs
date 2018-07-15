using System;
using System.Collections.Generic;
using System.Text;

namespace Miriwork
{
    internal class RequestFailedException : Exception
    {
        public object ErrorResponse { get; }

        public int? StatusCode { get; }

        public RequestFailedException(object errorResponse, int? statusCode)
        {
            this.ErrorResponse = errorResponse;
            this.StatusCode = statusCode;
        }
    }
}
