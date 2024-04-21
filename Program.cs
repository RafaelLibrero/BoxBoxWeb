using BoxBoxClient.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ADMIN",
        policy => policy.RequireRole("1"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(config =>
{
    config.AccessDeniedPath = "/Auth/ErrorAcceso";
});

builder.Services.AddControllersWithViews
    (options => options.EnableEndpointRouting = false);

builder.Services.AddTransient<ServiceApiBoxBox>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseMvc(routes =>
{
    routes.MapRoute(
    name: "default",
    template: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
