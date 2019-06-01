using System;
using System.Runtime.Serialization;

namespace Strainer.Exceptions
{
    public class StrainerMethodNotFoundException : StrainerException
    {
        public string MethodName { get; protected set; }

        public StrainerMethodNotFoundException(string methodName, string message) : base(message)
        {
            MethodName = methodName;
        }

        public StrainerMethodNotFoundException(string methodName, string message, Exception innerException) : base(message, innerException)
        {
            MethodName = methodName;
        }

        public StrainerMethodNotFoundException(string message) : base(message)
        {

        }

        public StrainerMethodNotFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public StrainerMethodNotFoundException()
        {

        }

        protected StrainerMethodNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
