using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeAnalyticsService.IRepositories;
using ExchangeAnalyticsService.Repositories;
using ExchangeAnalyticsService.Services;
using ExchangeAnalyticsService.Services.Interfaces;
using ExchangeAnalyticsService.Utils;
using ExchCommonLib;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using DbWrapperCore;
using ExchangeAnalyticsService.Auth;
using System.IO;
using ExchangeAnalyticsService.Analytic;
using ExchangeAnalyticsService.Classes;

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

            services.AddLogging(logging =>
            {
                //logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            });

            services.AddSingleton<IDBProvider>(DbUtils.MariaDbWrapper);
            services.AddSingleton<IInstrumentsRepository, InstrumentsRepository>();
            services.AddSingleton<IInstrumentsService, InstrumentsService>();
            services.AddSingleton<IRatesRepository, RatesRepository>();
            services.AddSingleton<IAnalyticsService, AnalyticsService>();
            services.AddSingleton<ICandlesService, CandlesService>();
            services.AddSingleton<IParsersService, ParsersService>();
            services.AddSingleton<IParsersRepository, ParsersRepository>();
            services.AddSingleton<InstrumentsTechAnalyser, InstrumentsTechAnalyser>();
            services.AddSingleton<IPortfolioService, PortfolioService>();
            services.AddSingleton<IPortfolioRepository>(r =>
            {
                var log = r.GetService<ILogger<PortfolioRepository>>();
                return new PortfolioRepository(log, DbUtils.UserPortfolioDb);
            });
            services.AddSingleton<IOperationsRepository>(r =>
            {
                var log = r.GetService<ILogger<OperationsRepository>>();
                return new OperationsRepository(log, DbUtils.UserPortfolioDb);
            });
            services.AddSingleton<ITechMethodsRepository>(r =>
            {
                var log = r.GetService<ILogger<TechMethodsRepository>>();
                return new TechMethodsRepository(DbUtils.SmAnalyticsDb, log);
            });
            //services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IAccountRepository, AccountRepository>();


            services.AddSingleton<IConfigurationRoot>(serviceProvider =>
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                var config = builder.Build();
                return config;
            });


            AddAuthenticationServices(services);
            AddAdditionalServices(services);



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddApiVersioning();
            services.AddMvcCore().AddApiExplorer();

            AddSwaggerGen(services);

        }

        private static void AddSwaggerGen(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "AnalyticsService API",
                    Version = "v1",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Glushakov Max",
                        Email = string.Empty,
                        Url = ""
                    }
                });
                c.AddSecurityDefinition("Bearer",
                    new ApiKeyScheme
                    {
                        In = "header",
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                        Name = "Authorization",
                        Type = "apiKey"
                    });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                });
            });
        }


        private static void AddAdditionalServices(IServiceCollection services)
        {

        }

        private static void AddAuthenticationServices(IServiceCollection services)
        {
            services.AddAuthentication("BearerRemote")
                    .AddScheme<RemoteJWTAuthenticationOptions, RemoteJWTAuthenticationHandler>("BearerRemote", null);

            services.AddSingleton<IRemoteJwtAuthenticationService>(serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<IConfigurationRoot>();
                var serviceAuthenticationParams = config.GetSection("ServiceAuthentication")?.Get<ServiceAuthenticationParams>();

                return new RemoteJwtAuthenticationService(serviceAuthenticationParams.TokenCheckServiceUrl);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AnalyticsService V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();

        }
    }
}
