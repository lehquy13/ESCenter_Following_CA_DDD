using System.Net;
using System.Security.Claims;
using ESCenter.Admin.Host;
using ESCenter.Administrator;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllersWithViews(options =>
    {
        options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
    });
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
    builder.Services.AddHost(builder.Configuration);
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/home/error");
}

app.UseStatusCodePages(context => {
    var request = context.HttpContext.Request;
    var response = context.HttpContext.Response;

    if (!context.HttpContext.User.Claims.Any(x => x is { Type: ClaimTypes.Role, Value: "Admin" }))   
        // you may also check requests path to do this only for specific methods       
        // && request.Path.Value.StartsWith("/specificPath")
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

app.Run();