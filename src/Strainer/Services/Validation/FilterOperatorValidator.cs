using Fluorite.Strainer.Models.Filtering.Operators;

namespace Fluorite.Strainer.Services.Validation;

public class FilterOperatorValidator : IFilterOperatorValidator
{
    public FilterOperatorValidator()
    {

    }

    public void Validate(IFilterOperator filterOperator)
    {
        Guard.Against.Null(filterOperator);

        if (string.IsNullOrWhiteSpace(filterOperator.Symbol))
        {
            throw new InvalidOperationException(
                $"{nameof(IFilterOperator.Symbol)} for filter operator " +
                $"\"{filterOperator}\" cannot be null, empty or contain " +
                $"only whitespace characters.");
        }

        if (filterOperator.Expression == null)
        {
            throw new InvalidOperationException(
                $"{nameof(IFilterOperator.Expression)} for filter operator " +
                $"\"{filterOperator}\" cannot be null.");
        }
    }

    public void Validate(IEnumerable<IFilterOperator> filterOperators)
    {
        Guard.Against.Null(filterOperators);

        if (!filterOperators.Any())
        {
            return;
        }

        foreach (var @operator in filterOperators)
        {
            Validate(@operator);
        }

        var duplicatedSymbols = filterOperators
            .GroupBy(f => f.Symbol)
            .FirstOrDefault(g => g.Count() > 1);
        if (duplicatedSymbols != null)
        {
            throw new InvalidOperationException(
                $"More then one filter operator found with the same" +
                $"symbol: \"{duplicatedSymbols.Key}\". " +
                $"Symbol representing filter operator must be unique. " +
                $"Please remove or change symbol for either of operators. " +
                $"Conflicting filter operators:\n" +
                $"{string.Join(Environment.NewLine, duplicatedSymbols.Select(f => f.ToString()))}");
        }
    }
}
