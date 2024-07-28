using Fluorite.Strainer.Models.Configuration;

namespace Fluorite.Strainer.Services.Configuration;

/// <summary>
/// Provides read-only <see cref="IStrainerConfiguration"/>.
/// </summary>
public class StrainerConfigurationProvider : IStrainerConfigurationProvider
{
    private readonly IStrainerConfiguration _strainerConfiguration;

    /// <summary>
    /// Initializes a new instance of the <see cref="StrainerConfigurationProvider"/> class.
    /// </summary>
    /// <param name="strainerConfiguration">
    /// The Strainer configuration to use.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="strainerConfiguration"/> is <see langword="null"/>.
    /// </exception>
    public StrainerConfigurationProvider(IStrainerConfiguration strainerConfiguration)
    {
        _strainerConfiguration = Guard.Against.Null(strainerConfiguration);
    }

    /// <inheritdoc/>
    public IStrainerConfiguration GetStrainerConfiguration() => _strainerConfiguration;
}
