using System.Reflection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Logging.AzureAppServices;
using Scheduler.Authorization;
using Scheduler.Interfaces;
using Scheduler.Services;
using Scheduler.SharedCode;

var builder = WebApplication.CreateBuilder(args);

//Terminal command: ASPNETCORE_ENVIRONMENT=Staging dotnet Scheduler.dll
if (builder.Environment.IsStaging())
{
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(5020);
    });
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });
}

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddMvc(setupAction =>
{
    setupAction.EnableEndpointRouting = false;
}).AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
})
    .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IActivityLoggerSql, LoggerSql>();

var app = builder.Build();

var conStr = builder.Configuration.GetSection("ConnectionStrings:schedulerDb").Get<string>();
//var DbPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/Db/log.txt";
StaticConfig.ConStr = conStr;
//StaticConfig.LogFilePath = DbPath;

///builder.Logging.AddAzureWebAppDiagnostics();
//builder.Services.Configure<AzureFileLoggerOptions>(options =>
//{
   // options.FileName = "azure-diagnostics-";
  //  options.FileSizeLimit = 50 * 1024;
 //   options.RetainedFileCountLimit = 5;
//});
//builder.Services.Configure<AzureBlobLoggerOptions>(options =>
//{
  //  options.BlobName = "log.txt";
//});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHttpsRedirection();
}

app.UseForwardedHeaders();

app.UseAuthentication();
app.UseStaticFiles();
app.UseRouting();

app.UseMiddleware<CustomBasicAuthMiddleware>();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
