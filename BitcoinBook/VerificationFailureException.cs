using System;

namespace BitcoinBook
{
    public class VerificationFailureException : Exception
    {
        public VerificationFailureException()
        {
        }

        public VerificationFailureException(string message) : base(message)
        {
        }

        public VerificationFailureException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
