namespace Fluorite.Strainer.Exceptions;

public class StrainerMethodNotFoundException : StrainerException
{
    public StrainerMethodNotFoundException(string methodName)
    {
        MethodName = Guard.Against.Null(methodName);
    }

    public StrainerMethodNotFoundException(string methodName, string message) : base(message)
    {
        MethodName = Guard.Against.Null(methodName);
    }

    public StrainerMethodNotFoundException(string methodName, string message, Exception innerException) : base(message, innerException)
    {
        MethodName = Guard.Against.Null(methodName);
    }

    public string MethodName { get; }
}
