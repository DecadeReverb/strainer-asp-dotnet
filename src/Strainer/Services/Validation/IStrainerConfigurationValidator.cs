using Fluorite.Strainer.Models.Configuration;

namespace Fluorite.Strainer.Services.Validation;

/// <summary>
/// Defines means of <see cref="IStrainerConfiguration"/> validation.
/// </summary>
public interface IStrainerConfigurationValidator
{
    /// <summary>
    /// Validates an instance of Strainer configuration.
    /// </summary>
    /// <param name="strainerConfiguration">
    /// The instance of <see cref="IStrainerConfiguration"/> to validate.
    /// </param>
    void Validate(IStrainerConfiguration strainerConfiguration);
}
