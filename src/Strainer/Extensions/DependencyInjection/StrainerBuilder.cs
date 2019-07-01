using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fluorite.Extensions.DependencyInjection
{
    public class StrainerBuilder : IStrainerBuilder
    {
        public StrainerBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get; }
    }
}
