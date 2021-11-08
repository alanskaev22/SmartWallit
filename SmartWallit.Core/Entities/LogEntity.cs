using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWallit.Core.Entities
{
    public class LogEntity : BaseEntity
    {
         public string ServerName { get; set; }
         public string HttpStatusCode { get; set; }
         public string HttpMethod { get; set; }
         public string ApiMethodName { get; set; }
         public string ExceptionMessage { get; set; }
         public string StackTrace {  get; set; }
         public string UserId { get; set; }
    }
}
