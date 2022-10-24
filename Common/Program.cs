using System;

namespace Common
{
    public class Request
    {
        public RequestType Mode { get; set; }
        public string[] Files { get; set; }
    }
    public enum RequestType
    {
        Transmit,
        Receive,
    }
} 
