using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Microsoft.Extensions.Options;

namespace Fluorite.Strainer.AspNetCore.Services;

/// <summary>
/// Provides access to <see cref="StrainerOptions"/> when using Strainer
/// within ASP.NET Core applications registered as singleton services.
/// </summary>
public class AspNetCoreSingletonStrainerOptionsProvider : IStrainerOptionsProvider
{
    private readonly IOptionsMonitor<StrainerOptions> _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="AspNetCoreSingletonStrainerOptionsProvider"/>
    /// class.
    /// </summary>
    /// <param name="options">
    /// The Strainer options wrapper in <see cref="IOptions{TOptions}"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    public AspNetCoreSingletonStrainerOptionsProvider(IOptionsMonitor<StrainerOptions> options)
    {
        _options = Guard.Against.Null(options);
    }

    /// <inheritdoc/>
    public StrainerOptions GetStrainerOptions() => _options.CurrentValue;
}
