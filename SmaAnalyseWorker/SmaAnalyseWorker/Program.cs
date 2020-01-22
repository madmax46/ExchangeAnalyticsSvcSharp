using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using DbWrapperCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySqlWrapper;
using SmaAnalyseWorker.Workers;
using System;
using System.Collections.Generic;
using System.IO;

namespace SmaAnalyseWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
                logging.AddConsole();
            });

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(serviceCollection);

            RegisterTypes(containerBuilder);

            var container = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);

            var service = serviceProvider.GetService<AutoInstrumentsAnalyser>();
            service.RunAsync();



            Console.ReadLine();
        }

        static void RegisterTypes(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(serviceProvider =>
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                var config = builder.Build();
                return config;
            }).As<IConfigurationRoot>().SingleInstance();

            var param = new ResolvedParameter(
            (pi, ctx) => pi.ParameterType == typeof(IDBProvider) && pi.Name == "smanAlyticsDb",
            (pi, ctx) =>
            {
                var config = ctx.Resolve<IConfigurationRoot>();
                var mySqlConfig = config.GetSection("MySqlSmanAlytics")?.Get<MySqlConfig>();
                return new MySqlWrap(mySqlConfig);
            });

            var param2 = new ResolvedParameter(
          (pi, ctx) => pi.ParameterType == typeof(IDBProvider) && pi.Name == "stockQuotesDb",
          (pi, ctx) =>
          {
              var config = ctx.Resolve<IConfigurationRoot>();
              var mySqlConfig = config.GetSection("MySqlStockQuotes")?.Get<MySqlConfig>();
              return new MySqlWrap(mySqlConfig);
          });

            var listParams = new List<ResolvedParameter>() { param, param2 };

            containerBuilder.RegisterType<AutoInstrumentsAnalyser>().AsSelf()
                .WithParameters(listParams).SingleInstance();
        }
    }
}
