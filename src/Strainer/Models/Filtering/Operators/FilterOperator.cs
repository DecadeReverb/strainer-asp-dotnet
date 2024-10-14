using System.Diagnostics;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Filtering.Operators;

/// <summary>
/// Provides information about filtering operator.
/// </summary>
[DebuggerDisplay("\\{{" + nameof(Symbol) + ",nq} {" + nameof(Name) + ",nq}\\}")]
public class FilterOperator : IFilterOperator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FilterOperator"/> class.
    /// </summary>
    public FilterOperator()
    {

    }

    /// <summary>
    /// Gets or sets a func providing an <see cref="System.Linq.Expressions.Expression"/>
    /// with filter operator applied when supplied a <see cref="IFilterExpressionContext"/>.
    /// </summary>
    public Func<IFilterExpressionContext, Expression> Expression { get; set;  }

    /// <summary>
    /// Gets or sets a value indicating whether current
    /// operator is case insensitive.
    /// </summary>
    public bool IsCaseInsensitive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether associated
    /// <see cref="System.Linq.Expressions.Expression"/> uses method
    /// based on a <see cref="string"/> instance like
    /// <see cref="string.Contains(string)"/> or <see cref="string.StartsWith(string)"/>.
    /// </summary>
    public bool IsStringBased { get; set; }

    /// <summary>
    /// Gets or sets the operator name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a <see cref="string"/> representation of the operator.
    /// </summary>
    public string Symbol { get; set; }

    /// <summary>
    /// Returns a <see cref="string"/> that represents the current <see cref="FilterOperator"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="string"/> that represents the current <see cref="FilterOperator"/>.
    /// </returns>
    public override string ToString()
    {
        return $"{Symbol} {Name}";
    }
}
