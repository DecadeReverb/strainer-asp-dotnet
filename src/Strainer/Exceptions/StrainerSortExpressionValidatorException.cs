using System.Runtime.Serialization;

namespace Fluorite.Strainer.Exceptions;

public class StrainerSortExpressionValidatorException : StrainerException
{
    public StrainerSortExpressionValidatorException(Type entityType)
    {
        EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
    }

    public StrainerSortExpressionValidatorException(Type entityType, string message) : base(message)
    {
        EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
    }

    public StrainerSortExpressionValidatorException(Type entityType, string message, Exception innerException) : base(message, innerException)
    {
        EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
    }

    protected StrainerSortExpressionValidatorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }

    public Type EntityType { get; }
}
