using System.Security.Claims;
using ESCenter.Admin.Host;
using ESCenter.Administrator;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

logger.Information("Starting web host");

builder.Services.AddControllersWithViews(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
});

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddHost(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.Logger.LogInformation("Adding Development middleware...");

    app.UseDeveloperExceptionPage();

    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/home/error");
}

app.UseSerilogRequestLogging();

app.UseStatusCodePages(context =>
{
    var request = context.HttpContext.Request;
    var response = context.HttpContext.Response;

    if (!context.HttpContext.User.Claims.Any(x => x is { Type: ClaimTypes.Role, Value: "Admin" }))
    {
        response.Redirect("/admin/authentication");
    }

    return Task.CompletedTask;
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSession();

app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Logger.LogInformation("LAUNCHING");

app.Run();