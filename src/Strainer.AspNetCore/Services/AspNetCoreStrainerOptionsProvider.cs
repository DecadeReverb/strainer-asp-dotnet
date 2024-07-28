using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Microsoft.Extensions.Options;

namespace Fluorite.Strainer.AspNetCore.Services;

/// <summary>
/// Provides access to <see cref="StrainerOptions"/> when using Strainer
/// within ASP.NET Core applications.
/// </summary>
public class AspNetCoreStrainerOptionsProvider : IStrainerOptionsProvider
{
    private readonly IOptionsSnapshot<StrainerOptions> _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="AspNetCoreStrainerOptionsProvider"/>
    /// class.
    /// </summary>
    /// <param name="options">
    /// The Strainer options wrapper in <see cref="IOptionsSnapshot{TOptions}"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    public AspNetCoreStrainerOptionsProvider(IOptionsSnapshot<StrainerOptions> options)
    {
        _options = Guard.Against.Null(options);
    }

    /// <inheritdoc/>
    public StrainerOptions GetStrainerOptions() => _options.Value;
}
