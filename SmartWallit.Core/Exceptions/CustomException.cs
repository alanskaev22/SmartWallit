using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SmartWallit.Core.Exceptions
{
    public class CustomException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ContentType { get; set; } = @"application/json";
        public string Field { get; set; }

        public CustomException(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
        }

        public CustomException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public CustomException(HttpStatusCode statusCode, string message, string field)
            : base(message)
        {
            StatusCode = statusCode;
            Field = field;
        }

        public CustomException(HttpStatusCode statusCode, Exception inner)
            : this(statusCode, inner.ToString()) { }

        public CustomException(HttpStatusCode statusCode, JObject errorObject)
            : this(statusCode, errorObject.ToString())
        {
            ContentType = @"application/json";
        }
    }
}
