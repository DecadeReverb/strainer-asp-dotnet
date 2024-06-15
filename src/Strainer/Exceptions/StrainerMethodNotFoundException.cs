using System.Runtime.Serialization;

namespace Fluorite.Strainer.Exceptions;

public class StrainerMethodNotFoundException : StrainerException
{
    public StrainerMethodNotFoundException()
    {

    }

    public StrainerMethodNotFoundException(string message) : base(message)
    {

    }

    public StrainerMethodNotFoundException(string message, Exception innerException) : base(message, innerException)
    {

    }

    public StrainerMethodNotFoundException(string methodName, string message) : base(message)
    {
        MethodName = Guard.Against.Null(methodName);
    }

    public StrainerMethodNotFoundException(string methodName, string message, Exception innerException) : base(message, innerException)
    {
        MethodName = Guard.Against.Null(methodName);
    }

    protected StrainerMethodNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }

    public string MethodName { get; }
}
