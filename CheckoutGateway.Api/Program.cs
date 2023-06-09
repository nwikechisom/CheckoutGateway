using CheckoutGateway.Api.Middlewares;
using CheckoutGateway.Api.ServiceExtensions;
using CheckoutGateway.BusinessLogic.Proxy.Bank.Models;
using CheckoutGateway.DataLayer.Context;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config
IWebHostEnvironment environment = builder.Environment;
string serilogUrl = configuration.GetValue<string>("Serilog:Url");

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDatabaseService(configuration);
builder.Services.AddProjectServices();
builder.Services.AddProjectRepositories();

if (serilogUrl != null)
    builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.Seq(serverUrl: serilogUrl));

builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        $"{configuration.GetValue<string>("Redis:Host")}:{configuration.GetValue<int>("Redis:Port")},password={configuration.GetValue<string>("Redis:Password")}";
});

builder.Services.AddApiVersioning(config =>
{
    // default API Version as 1.0
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});
builder.Services.Configure<BankProxyCredentials>(configuration.GetSection("BankOptions"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Build application
var app = builder.Build();

//Add Automigration of Database Context
using(var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var dbContext = services.GetRequiredService<DatabaseContext>();
    var conn = dbContext.Database.GetConnectionString();
    ServiceExtension.RunMigration<DatabaseContext>(dbContext).Wait();
}

//Add SwaggerUI for development Environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline
app.UseHttpsRedirection();

app.UseMiddleware<AuthMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
