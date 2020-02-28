using System;

namespace BitcoinBook
{
    public class PrivateKeyNotFoundException : Exception
    {
        public PrivateKeyNotFoundException()
        {
        }

        public PrivateKeyNotFoundException(string message) : base(message)
        {
        }

        public PrivateKeyNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
