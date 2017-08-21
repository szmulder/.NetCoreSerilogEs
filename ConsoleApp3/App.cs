using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace ConsoleApp3
{
    public class App
    {
        private readonly ITestService _testService;
        private readonly ILogger<App> _logger;
        private readonly AppSettings _config;

        public App(ITestService testService,
            IOptions<AppSettings> config,
            ILogger<App> logger)
        {
            _testService = testService;
            _logger = logger;
            _config = config.Value;
        }

        public void Run()
        {
            _logger.LogWarning($"This is a console application for {_config.Title}");
            _testService.Run();
            Console.ReadKey();
        }
    }
}
