﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fluorite.Extensions.DependencyInjection;
using Fluorite.Sieve.Example.Data;
using Fluorite.Sieve.Example.Services.Middleware;
using Fluorite.Strainer.Example.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Fluorite.Strainer.Example
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
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDatabase");
            });

            services.AddStrainer<ApplicationStrainerProcessor>()
                .AddCustomFilterMethods<StrainerCustomFilterMethods>()
                .AddCustomSortMethods<StrainerCustomSortMethods>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<TimeMeasurementMiddleware>();
            app.UseMvc();
        }
    }
}
