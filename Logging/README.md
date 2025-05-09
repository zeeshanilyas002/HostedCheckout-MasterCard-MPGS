# GlobalPay.Logging

`GlobalPay.Logging` is a structured logging library for .NET applications. It provides seamless integration with console, file-based logging, and AWS CloudWatch, enabling developers to capture and manage logs effectively.

## Features

- **Console Logging**: Outputs logs in JSON format for easy parsing.
- **File Logging**: Supports daily rolling log files.
- **AWS CloudWatch Logging**: Sends logs to AWS CloudWatch with customizable log groups and streams.
- **Correlation ID Support**: Includes a delegating handler for distributed tracing.

## Requirements

- **.NET Version**: .NET 8
- **C# Version**: 12.0
- **Dependencies**:
  - `Serilog`
  - `Serilog.Formatting.Json`
  - `Serilog.Sinks.File`
  - `Serilog.Sinks.AwsCloudWatch`
  - `AWSSDK.CloudWatchLogs`
  - `Microsoft.Extensions.DependencyInjection`
  - `Microsoft.Extensions.Configuration`

## Installation

1. Add the required NuGet packages to your project:
   
   dotnet add package Serilog
   dotnet add package Serilog.Formatting.Json
   dotnet add package Serilog.Sinks.File
   dotnet add package Serilog.Sinks.AwsCloudWatch
   dotnet add package AWSSDK.CloudWatchLogs


2. Add a reference to the `GlobalPay.Logging` library in your project.

## Configuration

### 1. Add Logging Configuration to `appsettings.json`

Add the following configuration to your `appsettings.json` file:

{
  "LoggingOptions": {
    "UseFile": true,
    "UseCloudWatch": true
  },
  "AWS": {
    "Region": "us-east-1",
    "LogGroup": "YourServiceName-logs",
    "AccessKey": "YourAWSAccessKey",
    "SecretKey": "YourAWSSecretKey"
  }
}


- **`UseFile`**: Enables file-based logging.
- **`UseCloudWatch`**: Enables AWS CloudWatch logging.
- **`AWS`**: Contains AWS credentials and configuration for CloudWatch.

### 2. Register Logging in `Program.cs`

Modify your `Program.cs` to register the logging service:

using GlobalPay.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add structured logging
builder.Services.AddStructuredLogging(builder.Configuration, "YourServiceName");

var app = builder.Build();

// Example usage of logging
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application has started.");

app.Run();


Replace `"YourServiceName"` with the name of your service to include it in log metadata.

## Usage

Inject `ILogger<T>` into your classes to log messages:

using Microsoft.Extensions.Logging;

public class ExampleService
{
    private readonly ILogger<ExampleService> _logger;

    public ExampleService(ILogger<ExampleService> logger)
    {
        _logger = logger;
    }

    public void DoWork()
    {
        _logger.LogInformation("Work started.");
        try
        {
            // Perform some work
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while doing work.");
        }
        _logger.LogInformation("Work completed.");
    }
}


## Notes

- **AWS Credentials**: Ensure AWS credentials are securely stored. Avoid hardcoding them in production environments. Use AWS Secrets Manager or environment variables for better security.
- **Correlation ID**: The library registers a `CorrelationIdDelegatingHandler` to support correlation IDs in HTTP requests. Use it for distributed tracing.

