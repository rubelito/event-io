﻿using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.EntityFrameworkCore;
using Scheduler.Authorization;
using Scheduler.Entity;
using Scheduler.Services;
using Scheduler.SharedCode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

//retrieve port from environment variables
var port = builder.Configuration["PORT"];
 	
builder.WebHost.UseUrls($"http://0.0.0.0:{port};http://localhost:3000
//set listening urls

//builder.WebHost.UseUrls("https://0.0.0.0:5010");

//builder.WebHost.ConfigureKestrel(serverOptions =>
   // {
      //  serverOptions.ListenAnyIP(5010, listenOptions =>
      //  {
      //      listenOptions.UseHttps();
     //   });
  //  });


builder.Services.AddMvc(setupAction =>
{
    setupAction.EnableEndpointRouting = false;
}).AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
})
    .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest);

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

var conStr = builder.Configuration.GetSection("ConnectionStrings:schedulerDb").Get<string>();
StaticConfig.ConStr = conStr;

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
