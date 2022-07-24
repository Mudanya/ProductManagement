using AspNetCoreRateLimit;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Repository.DataShaping;
using UltimateApi.ActionFilters;
using UltimateApi.AutoMapper;
using UltimateApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//extensions methods
builder.Services.ConfigureCors();
builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureServices();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.Configure<ApiBehaviorOptions>(
    options =>
    { options.SuppressModelStateInvalidFilter = true; }
    );
builder.Services.AddScoped<ValidationAttributeFilter>();
builder.Services.AddScoped<ValidateCompanyExistAttribute>();
builder.Services.AddScoped<ValidateEmployeeForComapanyAttribute>();
builder.Services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureResponseCache();
builder.Services.ConfigureHttpHeaders();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureAuthentificationManager();
builder.Services.ConfigureSwagger();
builder.Services.AddControllers(
    config =>
    {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
        config.CacheProfiles.Add("120SecondDuration", new CacheProfile { Duration = 120 });
    }
    )
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();

var app = builder.Build();

// Configure the HTTP request pipeline.

//nlog configuration
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
using var scope = app.Services.CreateScope();
var logger = scope.ServiceProvider.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

app.UseHttpsRedirection();

//configure cors in the app
app.UseCors("CorsPolicy");

app.UseResponseCaching();
app.UseHttpCacheHeaders();
app.UseIpRateLimiting();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Ultimate API V1");
    s.SwaggerEndpoint("/swagger/v2/swagger.json", "Ultimate API V2");
});
app.MapControllers();

app.Run();
