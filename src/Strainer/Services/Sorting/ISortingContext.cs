using Fluorite.Strainer.Services.Validation;

namespace Fluorite.Strainer.Services.Sorting;

public interface ISortingContext
{
    ISortExpressionProvider ExpressionProvider { get; }

    ISortExpressionValidator ExpressionValidator { get; }

    ISortingWayFormatter Formatter { get; }

    ISortTermParser TermParser { get; }

    ISortTermValueParser TermValueParser { get; }
}
