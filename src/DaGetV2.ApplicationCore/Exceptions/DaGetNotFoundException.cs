namespace DaGetV2.ApplicationCore.Exceptions
{
    using System;

    public class DaGetNotFoundException : Exception
    {
        public DaGetNotFoundException() : base() { }

        public DaGetNotFoundException(string msg) : base(msg) { }

        public DaGetNotFoundException(string msg, Exception ex) : base(msg, ex) { }
    }
}