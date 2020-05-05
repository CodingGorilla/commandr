using System;
using Jasper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Commandr.Samples.Jasper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).RunJasper(args);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseJasper()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
