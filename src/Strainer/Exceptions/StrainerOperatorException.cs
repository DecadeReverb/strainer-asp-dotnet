using Fluorite.Strainer.Models.Filtering.Operators;
using System.Reflection;
using System.Runtime.Serialization;

namespace Fluorite.Strainer.Exceptions;

public class StrainerOperatorException : StrainerException
{
    public StrainerOperatorException(
        IFilterOperator filterOperator,
        PropertyInfo propertyInfo,
        object value)
    {
        FilterOperator = Guard.Against.Null(filterOperator);
        PropertyInfo = Guard.Against.Null(propertyInfo);
        Value = value;
    }

    public StrainerOperatorException(
        string message,
        IFilterOperator filterOperator,
        PropertyInfo propertyInfo,
        object value)
        : base(message)
    {
        FilterOperator = Guard.Against.Null(filterOperator);
        PropertyInfo = Guard.Against.Null(propertyInfo);
        Value = value;
    }

    public StrainerOperatorException(
        string message,
        Exception innerException,
        IFilterOperator filterOperator,
        PropertyInfo propertyInfo,
        object value)
        : base(message, innerException)
    {
        FilterOperator = Guard.Against.Null(filterOperator);
        PropertyInfo = Guard.Against.Null(propertyInfo);
        Value = value;
    }

    protected StrainerOperatorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }

    public IFilterOperator FilterOperator { get; }

    public PropertyInfo PropertyInfo { get; }

    public object Value { get; }
}
