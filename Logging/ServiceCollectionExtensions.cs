using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.Runtime;
using GlobalPay.Logging.Handlers;
using GlobalPay.Logging.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.AwsCloudWatch;

namespace GlobalPay.Logging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStructuredLogging(this IServiceCollection services, IConfiguration configuration, string serviceName)
        {
            services.Configure<LoggingOptions>(configuration.GetSection("LoggingOptions"));

            services.AddHttpContextAccessor();
            services.AddTransient<CorrelationIdDelegatingHandler>();

            var loggingOptions = configuration.GetSection("LoggingOptions").Get<LoggingOptions>() ?? new LoggingOptions();

            var loggerConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service", serviceName)
                .WriteTo.Console(new JsonFormatter());


            if (loggingOptions.UseFile)
            {
                loggerConfig.WriteTo.File(
                    path: $"logs/{serviceName}-.log",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}");
            }

            if (loggingOptions.UseCloudWatch)
            {
                var region = configuration["LoggingOptions:Region"] ?? "us-east-1";
                var logGroup = configuration["LoggingOptions:LogGroup"] ?? $"{serviceName}-logs";

                var options = new CloudWatchSinkOptions
                {
                    LogGroupName = logGroup,
                    TextFormatter = new JsonFormatter(),
                    MinimumLogEventLevel = LogEventLevel.Information,

                    //CreateLogGroup = true,
                    LogStreamNameProvider = new DefaultLogStreamProvider()
                };

                var awsCredentials = new BasicAWSCredentials(configuration["LoggingOptions:AccessKey"], configuration["LoggingOptions:SecretKey"]);

                var cloudWatchClient = new AmazonCloudWatchLogsClient(
                            configuration["LoggingOptions:AccessKey"], configuration["LoggingOptions:SecretKey"], RegionEndpoint.GetBySystemName(region));

                loggerConfig.WriteTo.AmazonCloudWatch(options, cloudWatchClient).MinimumLevel.Verbose();
            }

            Log.Logger = loggerConfig.CreateLogger();
            Log.Logger.Information("works");
            return services;
        }

        //public static IHostBuilder UseSerilog(this IHostBuilder host)
        //{
        //    //host.UseSerilog();

        //    //return host;
        //}

        public static IApplicationBuilder UseStructuredLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoggingEnrichmentMiddleware>();
        }

    }

    public class LoggingOptions
    {
        public bool UseFile { get; set; } = false;
        public bool UseCloudWatch { get; set; } = false;
        public string LogGroup { get; set; }
        public string Region { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }
}
