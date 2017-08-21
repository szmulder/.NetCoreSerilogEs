using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using Serilog;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // entry to run app
            serviceProvider.GetService<App>().Run();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            var loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddDebug()
                .AddSerilog();            

            // add configured instance of logging
            serviceCollection.AddSingleton(loggerFactory);

            // add logging
            serviceCollection.AddLogging();

            // build system configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json", false)
                .Build();
            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));

            //Log serilog config from code
            //var serilogConfiguration = new LoggerConfiguration()
            //   .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions
            //(new Uri("http://localhost:9200"))
            //   {
            //       AutoRegisterTemplate = true,
            //   });
            //Log.Logger = serilogConfiguration.CreateLogger();

            //Log serilog config from file
            var serilogConfiguration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("serilogsettings.json")
            .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(serilogConfiguration)
                .CreateLogger();
            
            // add services
            serviceCollection.AddTransient<ITestService, TestService>();

            // add app
            serviceCollection.AddTransient<App>();
        }
    }
}