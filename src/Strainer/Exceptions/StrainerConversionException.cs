using System.Runtime.Serialization;

namespace Fluorite.Strainer.Exceptions
{
    public class StrainerConversionException : StrainerException
    {
        public StrainerConversionException(object value, Type targetedType)
        {
            Value = value;
            TargetedType = targetedType ?? throw new ArgumentNullException(nameof(targetedType));
        }

        public StrainerConversionException(string message, object value, Type targetedType) : base(message)
        {
            Value = value;
            TargetedType = targetedType ?? throw new ArgumentNullException(nameof(targetedType));
        }

        public StrainerConversionException(string message, Exception innerException, object value, Type targetedType) : base(message, innerException)
        {
            Value = value;
            TargetedType = targetedType ?? throw new ArgumentNullException(nameof(targetedType));
        }

        protected StrainerConversionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        public Type TargetedType { get; set; }

        public object Value { get; }
    }
}
