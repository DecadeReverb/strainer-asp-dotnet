using Fluorite.Extensions.DependencyInjection;
using Fluorite.Strainer.Services.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fluorite.Strainer.AspNetCore.Extensions.DependencyInjection
{
    public static class StrainerBuilderExtensions
    {
        public static IStrainerBuilder AddFluentApiMetadataProvider(this IStrainerBuilder strainerBuilder)
        {
            if (strainerBuilder is null)
            {
                throw new ArgumentNullException(nameof(strainerBuilder));
            }

            strainerBuilder.Services.Add(new ServiceDescriptor(
                typeof(IMetadataProvider),
                typeof(MetadataMapper),
                strainerBuilder.ServiceLifetime));

            return strainerBuilder;
        }

        public static IStrainerBuilder AddAttributesMetadataProvider(this IStrainerBuilder strainerBuilder)
        {
            if (strainerBuilder is null)
            {
                throw new ArgumentNullException(nameof(strainerBuilder));
            }

            strainerBuilder.Services.Add(new ServiceDescriptor(
                typeof(IMetadataProvider),
                typeof(AttributeMetadataProvider),
                strainerBuilder.ServiceLifetime));

            return strainerBuilder;
        }
    }
}
