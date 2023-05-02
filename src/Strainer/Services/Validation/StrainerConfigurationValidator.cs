using Fluorite.Strainer.Models.Configuration;

namespace Fluorite.Strainer.Services.Validation
{
    /// <inheritdoc/>
    public class StrainerConfigurationValidator : IStrainerConfigurationValidator
    {
        private readonly IFilterOperatorValidator _filterOperatorValidator;
        private readonly ISortExpressionValidator _sortExpressionValidator;

        public StrainerConfigurationValidator(
            IFilterOperatorValidator filterOperatorValidator,
            ISortExpressionValidator sortExpressionValidator)
        {
            _filterOperatorValidator = filterOperatorValidator ?? throw new ArgumentNullException(nameof(filterOperatorValidator));
            _sortExpressionValidator = sortExpressionValidator ?? throw new ArgumentNullException(nameof(sortExpressionValidator));
        }

        /// <summary>
        /// Validates an instance of Strainer configuration.
        /// </summary>
        /// <param name="strainerConfiguration">
        /// The instance of <see cref="IStrainerConfiguration"/> to validate.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="strainerConfiguration"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="strainerConfiguration"/> is not valid.
        /// </exception>
        public void Validate(IStrainerConfiguration strainerConfiguration)
        {
            if (strainerConfiguration is null)
            {
                throw new ArgumentNullException(nameof(strainerConfiguration));
            }

            try
            {
                _filterOperatorValidator.Validate(strainerConfiguration.FilterOperators.Values);
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
}
