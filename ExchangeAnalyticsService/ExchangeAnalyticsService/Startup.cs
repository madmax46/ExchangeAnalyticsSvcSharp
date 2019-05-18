using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Repositories;
using ExchangeAnalyticsService.Utils;
using ExchCommonLib;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace ExchangeAnalyticsService
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

            services.AddSingleton<IDBProvider>(DbUtils.MariaDbWrapper);
            services.AddSingleton<IInstrumentsRepository, InstrumentsRepository>();
            services.AddSingleton<ITestReturnRepository, TestReturnRepository>();
            services.AddSingleton<IRatesRepository, RatesRepository>();


            services.AddMemoryCache();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddApiVersioning();
            services.AddMvcCore().AddApiExplorer();

            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("1.0", new Info { Title = "My API", Version = "1.0" 
                c.SwaggerDoc("1.0", new Info
                {
                    Version = "1.0",
                    Title = "My API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Glushakov Max",
                        Email = string.Empty,
                        Url = ""
                    }
                });
                c.SwaggerDoc("2.0", new Info
                {
                    Title = "My API",
                    Version = "2.0"
                });


            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/1.0/swagger.json", "1.0");
                c.SwaggerEndpoint("/swagger/2.0/swagger.json", "2.0");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();

        }
    }
}
