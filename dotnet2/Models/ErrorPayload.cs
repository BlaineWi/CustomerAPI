using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace customerAPI.Models
{
    /// <summary>
    /// Error Payload
    /// </summary>
    public class ErrorPayload
    {
        /// <summary>
        /// HTTP Status Code
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Stack Trace
        /// </summary>
        public string StackTrace { get; set; }
        /// <summary>
        /// Additional Data
        /// </summary>
        public Dictionary<string,string> Data { get; set; }
    }
}
