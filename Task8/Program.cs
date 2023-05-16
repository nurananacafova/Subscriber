using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using SubscriberService.Models;
using SubscriberService.Services;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.WebHost.UseUrls("http://*:5000");

    builder.Services.AddDbContext<SubscriberDb>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddControllers()
        .AddFluentValidation(options =>
        {
            options.ImplicitlyValidateChildProperties = true;
            options.ImplicitlyValidateRootCollectionElements = true;

            options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        });

    builder.Services.AddScoped<ISubscriberService, SubscriberService.Services.SubscriberService>();

    builder.Services.AddControllersWithViews();

    var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
    NLog.GlobalDiagnosticsContext.Set("LogDirectory", logPath);
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();


    var app = builder.Build();
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.Run();
}
catch (Exception e)
{
    logger.Error(e);
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}

public partial class Program
{
}