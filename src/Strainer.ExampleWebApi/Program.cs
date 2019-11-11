using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fluorite.Sieve.Example.Data;
using Fluorite.Strainer.Attributes;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fluorite.Strainer.ExampleWebApi
{
    public class Program
    {
        //public static IEnumerable<Assembly> GetAssemblies()
        //{
        //    var list = new List<string>();
        //    var stack = new Stack<Assembly>();

        //    stack.Push(Assembly.GetEntryAssembly());

        //    do
        //    {
        //        var asm = stack.Pop();

        //        yield return asm;

        //        foreach (var reference in asm.GetReferencedAssemblies())
        //            if (!list.Contains(reference.FullName))
        //            {
        //                stack.Push(Assembly.Load(reference));
        //                list.Add(reference.FullName);
        //            }

        //    }
        //    while (stack.Count > 0);

        //}

        public static void Main(string[] args)
        {
            //var objectAttributeType = typeof(StrainerObjectAttribute);
            //var propertyAttributeType = typeof(StrainerPropertyAttribute);
            //var assemblies = GetAssemblies();
            //var types = assemblies.SelectMany(a => a.GetTypes()).ToList();
            //var typesFiltered = types.Where(t => (t.IsClass || t.IsValueType)
            //    && (t.CustomAttributes.Any(c => c.AttributeType == objectAttributeType)
                        //|| t.GetProperties().Any(p => p.CustomAttributes.Any(c => c.AttributeType == propertyAttributeType)))).ToList();

            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                try
                {
                    var context = provider.GetRequiredService<ApplicationDbContext>();

                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    DatabaseInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = provider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
