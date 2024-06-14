using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Identity;
using CST.API.Middleware;
using CST.BusinessLogic.Configuration;
using CST.Dal;
using CST.Dal.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using System.Net;
using System.Reflection;

NLogBuilder.ConfigureNLog("nlog.config");
var builder = WebApplication.CreateBuilder();

var config = builder.Configuration;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Task.CompletedTask;
        };
    
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();


builder.Services.AddHttpClient()
    .ConfigureBll()
    .ConfigureDal(config)
    .ConfigureApiOptions(config)
    .AddCors()
    .AddSwaggerGen(c =>
    {

        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "CST API",
            Version = "v1",
            Description = "The documentation Web API"
        });
        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    })
    .AddSwaggerGenNewtonsoftSupport();

var mappingConfig = new MapperConfiguration(cfg =>
{
    cfg.AddMaps(new[] {
        "CST.BusinessLogic",
        "CST.API"
    });

});
mappingConfig.AssertConfigurationIsValid();
var autoMapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(autoMapper);

builder.Logging
    .ClearProviders()
    .SetMinimumLevel(LogLevel.Trace)
    .AddConsole();
builder.Host.UseNLog();

var controllers = config["XsrfEnabled"].Equals("true")
    ? builder.Services.AddControllersWithViews(options =>
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()))
    : builder.Services.AddControllers();


builder.Services.AddAntiforgery(opt =>
{
    opt.HeaderName = "X-XSRF-TOKEN";
});

controllers.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
});

var app = builder.Build();
var env = app.Environment;

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseSwagger(c =>
{
    c.SerializeAsV2 = true;
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CST API V1");
    c.EnableTryItOutByDefault();
});

using var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope();
using var context = serviceScope.ServiceProvider.GetRequiredService<CstContext>();
context.Database.Migrate();

app.Run();

NLog.LogManager.Shutdown();