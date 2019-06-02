using System;
using Fluorite.Sieve.Example.Services.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Fluorite.Extensions.Builder
{
    public static class TimeMeasurementApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseTimeMeasurement(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.UseMiddleware<TimeMeasurementMiddleware>();
        }
    }
}
