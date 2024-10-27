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

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 0;
})
    .AddEntityFrameworkStores<AppDBcontext>();

// Configurar la cultura predeterminada a "es-ES" (España) para los precios
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

// Establecer la cultura para cada solicitud para los precios
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

//esto permite el identity
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
