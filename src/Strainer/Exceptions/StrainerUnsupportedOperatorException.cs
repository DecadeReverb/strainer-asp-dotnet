using Fluorite.Strainer.Models.Filtering.Operators;
using System.Reflection;
using System.Runtime.Serialization;

namespace Fluorite.Strainer.Exceptions;

public class StrainerUnsupportedOperatorException : StrainerException
{
    public StrainerUnsupportedOperatorException(
        IFilterOperator filterOperator,
        PropertyInfo propertyInfo,
        object value)
    {
        FilterOperator = filterOperator ?? throw new ArgumentNullException(nameof(filterOperator));
        PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        Value = value;
    }

    public StrainerUnsupportedOperatorException(
        string message,
        IFilterOperator filterOperator,
        PropertyInfo propertyInfo,
        object value)
        : base(message)
    {
        FilterOperator = filterOperator ?? throw new ArgumentNullException(nameof(filterOperator));
        PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        Value = value;
    }

    public StrainerUnsupportedOperatorException(
        string message,
        Exception innerException,
        IFilterOperator filterOperator,
        PropertyInfo propertyInfo,
        object value)
        : base(message, innerException)
    {
        FilterOperator = filterOperator ?? throw new ArgumentNullException(nameof(filterOperator));
        PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        Value = value;
    }

    protected StrainerUnsupportedOperatorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }

    public IFilterOperator FilterOperator { get; }

    public PropertyInfo PropertyInfo { get; }

    public object Value { get; }
}
