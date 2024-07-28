using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata;

/// <summary>
/// Provides <see cref="PropertyInfo"/> and full member name when supplied
/// with an <see cref="Expression{TDelegate}"/>.
/// </summary>
public class PropertyInfoProvider : IPropertyInfoProvider
{
    private readonly BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public;

    public PropertyInfo GetPropertyInfo(Type type, string name)
    {
        Guard.Against.Null(type);
        Guard.Against.NullOrWhiteSpace(name);

        return type.GetProperty(name, _bindingFlags);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="expression"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="expression"/> is invalid expression,
    /// not leading to a readable property.
    /// </exception>
    public (PropertyInfo PropertyInfo, string FullName) GetPropertyInfoAndFullName<T>(Expression<Func<T, object>> expression)
    {
        Guard.Against.Null(expression);

        if (expression.Body is not MemberExpression body)
        {
            var ubody = expression.Body as UnaryExpression;
            body = ubody?.Operand as MemberExpression;
        }

        if (body?.Member is not PropertyInfo propertyInfo)
        {
            throw new ArgumentException(
                $"Expression for '{nameof(T)}' {expression} " +
                $"is not a valid expression leading to a readable property.");
        }

        var stack = new Stack<string>();

        while (body != null)
        {
            stack.Push(body.Member.Name);
            body = body.Expression as MemberExpression;
        }

        var fullName = string.Join(".", stack.ToArray());

        return (propertyInfo, fullName);
    }

    public PropertyInfo[] GetPropertyInfos(Type type)
    {
        Guard.Against.Null(type);

        return type.GetProperties(_bindingFlags);
    }
}
