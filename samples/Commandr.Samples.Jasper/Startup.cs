using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Commandr.Samples.Jasper
{
    public class Startup
    {
        public void ConfigureContainer(ServiceRegistry services)
        {
            services.AddLogging();

            services.AddCommandr();
            services.For<ICommandBus>().Use<JasperCommandBus>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
                             {
                                 endpoints.MapCommands(_ => _.AddExecutingAssembly());
                             });
        }
    }
}
