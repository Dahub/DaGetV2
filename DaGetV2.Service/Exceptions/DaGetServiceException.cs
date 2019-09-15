using System;

namespace DaGetV2.Service
{
    public class DaGetServiceException : Exception
    {
        public DaGetServiceException() : base() { }

        public DaGetServiceException(string msg) : base(msg) { }

        public DaGetServiceException(string msg, Exception ex) : base(msg, ex) { }
    }
}