﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Fluorite.Strainer.Extensions.DependencyInjection;
using Fluorite.Strainer.Sample.Entities;
using Fluorite.Strainer.Sample.Services;

namespace Fluorite.Strainer.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("TestSqlServer")));

            services.AddStrainer<ApplicationStrainerProcessor>()
                .AddCustomFilterMethods<StrainerCustomFilterMethods>()
                .AddCustomSortMethods<StrainerCustomSortMethods>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // TIME MEASUREMENT
            var times = new List<long>();
            app.Use(async (context, next) =>
            {
                var sw = new Stopwatch();
                sw.Start();
                await next.Invoke();
                sw.Stop();
                times.Add(sw.ElapsedMilliseconds);
                var text = $"AVG: {(int)times.Average()}ms; AT {sw.ElapsedMilliseconds}; COUNT: {times.Count()}";
                Console.WriteLine(text);
                await context.Response.WriteAsync($"<!-- {text} -->");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}