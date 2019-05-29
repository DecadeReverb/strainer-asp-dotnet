using System;
using System.Runtime.Serialization;

namespace Sieve.Exceptions
{
    public class SieveException : Exception
    {
        public SieveException(string message) : base(message)
        {

        }

        public SieveException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public SieveException()
        {

        }

        protected SieveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
