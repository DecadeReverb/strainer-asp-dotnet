using Fluorite.Strainer.Models.Configuration;

namespace Fluorite.Strainer.Services.Validation;

/// <inheritdoc/>
public class StrainerConfigurationValidator : IStrainerConfigurationValidator
{
    private readonly IFilterOperatorValidator _filterOperatorValidator;
    private readonly ISortExpressionValidator _sortExpressionValidator;

    public StrainerConfigurationValidator(
        IFilterOperatorValidator filterOperatorValidator,
        ISortExpressionValidator sortExpressionValidator)
    {
        _filterOperatorValidator = Guard.Against.Null(filterOperatorValidator);
        _sortExpressionValidator = Guard.Against.Null(sortExpressionValidator);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="strainerConfiguration"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="strainerConfiguration"/> is not valid.
    /// </exception>
    public void Validate(IStrainerConfiguration strainerConfiguration)
    {
        Guard.Against.Null(strainerConfiguration);

        try
        {
            _filterOperatorValidator.Validate(strainerConfiguration.FilterOperators.Values, strainerConfiguration.ExcludedBuiltInFilterOperators);
            _sortExpressionValidator.Validate(strainerConfiguration.PropertyMetadata);
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(
                "Invalid Strainer configuration. See inner exception for details.",
                exception);
        }
    }
}
