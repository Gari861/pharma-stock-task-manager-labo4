using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebAppPharma.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDBcontext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("cadena")));

// Configurar la cultura predeterminada a "es-ES" (España)
var defaultCulture = new CultureInfo("es-ES")
{
    NumberFormat = new NumberFormatInfo
    {
        NumberDecimalSeparator = ".",    // Separador decimal con punto
        CurrencyDecimalSeparator = ".",  // También para monedas, si las usas
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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
