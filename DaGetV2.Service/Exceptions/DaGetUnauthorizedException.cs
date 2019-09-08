using System;

namespace DaGetV2.Service
{
    public class DaGetUnauthorizedException : Exception
    {
        public DaGetUnauthorizedException() : base() { }

        public DaGetUnauthorizedException(string msg) : base(msg) { }

        public DaGetUnauthorizedException(string msg, Exception ex) : base(msg, ex) { }
    }
}