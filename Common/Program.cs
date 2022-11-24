using System;

namespace Common
{
    public class Request
    {
        public RequestType Mode { get; set; }
        public byte[] Bytes { get; set; }
    }
    public enum RequestType
    {
        Transmit,
        Receive,
    }
}
