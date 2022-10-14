using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Good.Admin.Util
{
    public class ElkLogDTO
    {
        public Fields fields { get; set; }
        public string level { get; set; }
        public DateTime @timestamp { get; set; }
    }
    public class Fields
    {
        /// <summary>
        /// 
        /// </summary>
        public string RemoteIpAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Response { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SourceContext { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RequestPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ConnectionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MachineName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ApplicationVersion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ThreadId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime RequestTime { get; set; }

    }
}
