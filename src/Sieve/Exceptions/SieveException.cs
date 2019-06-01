using System;
using System.Runtime.Serialization;

namespace Strainer.Exceptions
{
    public class StrainerException : Exception
    {
        public StrainerException(string message) : base(message)
        {

        }

        public StrainerException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public StrainerException()
        {

        }

        protected StrainerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
