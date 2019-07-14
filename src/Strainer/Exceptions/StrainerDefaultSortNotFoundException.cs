using System;
using System.Runtime.Serialization;

namespace Fluorite.Strainer.Exceptions
{
    public class StrainerDefaultSortNotFoundException : StrainerException
    {
        public StrainerDefaultSortNotFoundException(Type entityType)
        {
            EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
        }

        public StrainerDefaultSortNotFoundException(Type entityType, string message) : base(message)
        {
            EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
        }

        public StrainerDefaultSortNotFoundException(Type entityType, string message, Exception innerException) : base(message, innerException)
        {
            EntityType = entityType ?? throw new ArgumentNullException(nameof(entityType));
        }

        protected StrainerDefaultSortNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        public Type EntityType { get; }
    }
}
