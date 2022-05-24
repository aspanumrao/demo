using CacheManager.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateV01
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    
        public Startup(IHostingEnvironment env)
        {
            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            builder.SetBasePath(env.ContentRootPath)
                .AddJsonFile("Configuration.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            Action<ConfigurationBuilderCachePart> settings = (x) =>
            {
                x.WithMicrosoftLogging(yy =>
                {
                    yy.AddConsole(LogLevel.Debug);

                }).WithDictionaryHandle();
            };
            services.AddOcelot((IConfigurationRoot)Configuration, settings);
            services.AddMvc();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public  async void Configure(IApplicationBuilder app, IHostingEnvironment env)
     {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvcWithDefaultRoute();
            await app.UseOcelot();
         
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
            
        }
    }
}
