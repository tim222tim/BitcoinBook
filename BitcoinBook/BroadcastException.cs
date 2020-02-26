using System;

namespace BitcoinBook
{
    public class BroadcastException : Exception
    {
        public BroadcastException()
        {
        }

        public BroadcastException(string message) : base(message)
        {
        }

        public BroadcastException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
