using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Fluorite.Sieve.Example.Services.Middleware
{
    /// <summary>
    /// Provides means of time measurement for incoming requests.
    /// </summary>
    public class TimeMeasurementMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<long> _times;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeMeasurementMiddleware"/>
        /// class.
        /// </summary>
        /// <param name="next">
        /// The <see cref="RequestDelegate"/>.
        /// </param>
        public TimeMeasurementMiddleware(RequestDelegate next)
        {
            _next = next;
            _times = new List<long>();
        }

        /// <summary>
        /// Asynchronously measures .
        /// </summary>
        /// <param name="context">
        /// The <see cref="HttpContext"/>.
        /// </param>
        public async Task Invoke(HttpContext context)
        {
            var sw = new Stopwatch();
            sw.Start();

            await _next.Invoke(context);

            sw.Stop();
            _times.Add(sw.ElapsedMilliseconds);
            var text = $"AVG: {(int)_times.Average()}ms; AT {sw.ElapsedMilliseconds}; COUNT: {_times.Count()}";
            Console.WriteLine(text);

            await context.Response.WriteAsync($"\n<!-- {text} -->");
        }
    }
}
