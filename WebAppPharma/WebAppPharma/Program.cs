using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebAppPharma.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configuración de Entity Framework con SQL Server
var connectionString = builder.Configuration.GetConnectionString("cadena");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'cadena' not found.");
}

builder.Services.AddDbContext<AppDBcontext>(options =>
    options.UseSqlServer(connectionString));

// Configuración de Identity sin roles
builder.Services.AddDbContext<AppDBcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    // Configuración de contraseñas
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 1;

    // Configuración de bloqueo
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // Configuración de usuario
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._+";
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<AppDBcontext>();

// Configuración de cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    // Configuración de cookies
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

// Configurar la cultura predeterminada a "es-ES" (España)
var defaultCulture = new CultureInfo("es-ES")
{
    NumberFormat = new NumberFormatInfo
    {
        NumberDecimalSeparator = ".",
        CurrencyDecimalSeparator = ".",
        NumberGroupSeparator = ",",
        CurrencyGroupSeparator = ","
    }
};
CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

var app = builder.Build();

// Establecer la cultura para cada solicitud
var supportedCultures = new[] { defaultCulture };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(defaultCulture),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
