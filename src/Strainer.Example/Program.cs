﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Fluorite.Strainer.Example
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:80/", "https://*:443/")
                .UseStartup<Startup>();
    }
}
