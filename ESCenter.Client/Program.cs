using ESCenter.Client;
using ESCenter.Persistence;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

logger.Information("Starting web host");

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
});
builder.Host
    .UseSerilog((context, configuration)
        => configuration.ReadFrom.Configuration(context.Configuration)
    );
builder.Services.AddHost(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.Logger.LogInformation("Adding Development middleware...");
    app.UseDeveloperExceptionPage();
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.AddInfrastructureMiddleware();

app.Logger.LogInformation("LAUNCHING");

app.Run();