using System.Runtime.Serialization;

namespace Fluorite.Strainer.Exceptions;

public class StrainerSortExpressionValidatorException : StrainerException
{
    public StrainerSortExpressionValidatorException(Type entityType)
    {
        EntityType = Guard.Against.Null(entityType);
    }

    public StrainerSortExpressionValidatorException(Type entityType, string message) : base(message)
    {
        EntityType = Guard.Against.Null(entityType);
    }

    public StrainerSortExpressionValidatorException(Type entityType, string message, Exception innerException) : base(message, innerException)
    {
        EntityType = Guard.Against.Null(entityType);
    }

    protected StrainerSortExpressionValidatorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }

    public Type EntityType { get; }
}
