namespace DaGetV2.ApplicationCore.Exceptions
{
    using System;

    public class DaGetUnauthorizedException : Exception
    {
        public DaGetUnauthorizedException() : base() { }

        public DaGetUnauthorizedException(string msg) : base(msg) { }

        public DaGetUnauthorizedException(string msg, Exception ex) : base(msg, ex) { }
    }
}