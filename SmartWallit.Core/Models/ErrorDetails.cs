using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWallit.Core.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; set; }
        [JsonIgnore]
        public string StackTrace { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
