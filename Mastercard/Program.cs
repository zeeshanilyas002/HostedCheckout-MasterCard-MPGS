using Client.Interfaces;
using Client.Models;
using Client.Repositories;
using GlobalPay.HostedCheckouts.Mastercard.Authorization;
using GlobalPay.HostedCheckouts.Mastercard.Extensions;
using GlobalPay.HostedCheckouts.Mastercard.Models;
using GlobalPay.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStructuredLogging(builder.Configuration, "HostedCheckouts.Mastercard");
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddControllers(o =>
//{
//    o.Filters.Add<BasicAuthFilter>();
//});
builder.Services.AddHttpClient();
builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AWS"));
builder.Services.AddScoped<IHostedCheckOutService, HostedCheckOutService>();
builder.Services.AddScoped<BasicAuthFilter>();
//builder.Services.Configure<MPGSCreds>(builder.Configuration.GetSection("MPGSCreds"));
builder.Services.AddSingleton<AwsSecretManagerService>();
//
builder.Services.Configure<MPGSCreds>(builder.Configuration.GetSection("MPGS"));

// Fetch credentials from AWS Secrets Manager and update the configuration
//using (var scope = builder.Services.BuildServiceProvider().CreateScope())
//{
//    var awsSecretService = scope.ServiceProvider.GetRequiredService<AwsSecretManagerService>();
//    try
//    {
//        var fabCreds = await awsSecretService.GetFABCredentialsAsync();
//        builder.Services.Configure<MPGSCreds>(opts =>
//        {
//            opts.apiBaseUrl = fabCreds.BaseUrl;
//            opts.APIKey = fabCreds.password;
//            opts.merchantId = fabCreds.MerchantId;
//            opts.apiVersion = fabCreds.Version;
//        });
//        builder.Services.Configure<BasicAuth>(auth =>
//        {
//            auth.Password= fabCreds.APIKEY;
//            auth.UserName = fabCreds.userName;
//        });

//        Console.WriteLine("AWS Secret loaded successfully.");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Error fetching AWS Secret: {ex.Message}");
//    }
//}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStructuredLogging();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
