using Fluorite.Strainer.Collections;
using Fluorite.Strainer.Models.Filtering.Operators;

namespace Fluorite.Strainer.Services.Validation;

public interface IFilterOperatorValidator
{
    void Validate(IFilterOperator filterOperator);

    void Validate(IEnumerable<IFilterOperator> filterOperators, IReadOnlySet<string> excludedBuiltInFilterOperators);
}
