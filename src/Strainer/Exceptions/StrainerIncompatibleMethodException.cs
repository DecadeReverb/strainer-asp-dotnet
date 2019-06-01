using System;
using System.Runtime.Serialization;

namespace Fluorite.Strainer.Exceptions
{
    public class StrainerIncompatibleMethodException : StrainerException
    {
        public string MethodName { get; protected set; }
        public Type ExpectedType { get; protected set; }
        public Type ActualType { get; protected set; }

        public StrainerIncompatibleMethodException(
            string methodName,
            Type expectedType,
            Type actualType,
            string message)
            : base(message)
        {
            MethodName = methodName;
            ExpectedType = expectedType;
            ActualType = actualType;
        }

        public StrainerIncompatibleMethodException(
            string methodName,
            Type expectedType,
            Type actualType,
            string message,
            Exception innerException)
            : base(message, innerException)
        {
            MethodName = methodName;
            ExpectedType = expectedType;
            ActualType = actualType;
        }

        public StrainerIncompatibleMethodException(string message) : base(message)
        {

        }

        public StrainerIncompatibleMethodException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public StrainerIncompatibleMethodException()
        {

        }

        protected StrainerIncompatibleMethodException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
