using Fluorite.Strainer.Models.Filtering.Operators;
using System.Reflection;

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

    public IFilterOperator FilterOperator { get; }

    public PropertyInfo PropertyInfo { get; }

    public object Value { get; }
}
