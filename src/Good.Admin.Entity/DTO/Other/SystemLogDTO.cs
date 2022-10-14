using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Good.Admin.Entity
{
    public class SystemLogDTO
    {
        [JsonProperty(PropertyName = "RemoteIpAddress")]
        public string RemoteIpAddress { get; set; }

        [JsonProperty(PropertyName = "Url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "@timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty(PropertyName = "Time")]
        public int Time { get; set; }

        [JsonProperty(PropertyName = "Method")]
        public string Method { get; set; }

        [JsonProperty(PropertyName = "ContentType")]
        public string ContentType { get; set; }

        [JsonProperty(PropertyName = "Body")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "StatusCode")]
        public int StatusCode { get; set; }

        [JsonProperty(PropertyName = "Response")]
        public string Response { get; set; }

        [JsonProperty(PropertyName = "SourceContext")]
        public string SourceContext { get; set; }

        [JsonProperty(PropertyName = "RequestId")]
        public string RequestId { get; set; }

        [JsonProperty(PropertyName = "RequestPath")]
        public string RequestPath { get; set; }

        [JsonProperty(PropertyName = "ConnectionId")]
        public string ConnectionId { get; set; }

        [JsonProperty(PropertyName = "MachineName")]
        public string MachineName { get; set; }

        [JsonProperty(PropertyName = "ApplicationName")]
        public string ApplicationName { get; set; }

        [JsonProperty(PropertyName = "ApplicationVersion")]
        public string ApplicationVersion { get; set; }

        [JsonProperty(PropertyName = "ThreadId")]
        public int ThreadId { get; set; }

        [JsonProperty(PropertyName = "RequestTime")]
        public DateTime RequestTime { get; set; }
        [JsonProperty(PropertyName = "Level")]
        public string Level { get; set; }
        [JsonProperty(PropertyName = "Operator")]
        public string Operator { get; set; }
    }

}
