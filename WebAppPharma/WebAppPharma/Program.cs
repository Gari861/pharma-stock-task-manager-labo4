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
    .AddRoles<IdentityRole>() // Agrega roles
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

// Seed de datos de roles y usuarios
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Crear roles
    var roles = new[] { "Admin", "Manager", "Empleado", "Cliente" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Crear usuario administrador
    var adminEmail = "admin@gmail.com";
    var adminPassword = "Admin123";
    // Verifica que si ya existe, sino existe lo crea
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    // Crear usuario manager
    var managerEmail = "manager@gmail.com";
    var managerPassword = "Manager123";
    if (await userManager.FindByEmailAsync(managerEmail) == null)
    {
        var managerUser = new IdentityUser { UserName = managerEmail, Email = managerEmail };
        var result = await userManager.CreateAsync(managerUser, managerPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(managerUser, "Manager");
        }
    }

    // Crear usuario empleado
    var empleadoEmail = "empleado@gmail.com";
    var empleadoPassword = "Empleado123";
    if (await userManager.FindByEmailAsync(empleadoEmail) == null)
    {
        var empleadoUser = new IdentityUser { UserName = empleadoEmail, Email = empleadoEmail };
        var result = await userManager.CreateAsync(empleadoUser, empleadoPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(empleadoUser, "Empleado");
        }
    }

    // Crear usuario cliente
    var clienteEmail = "cliente@gmail.com";
    var clientePassword = "Cliente123";
    if (await userManager.FindByEmailAsync(clienteEmail) == null)
    {
        var clienteUser = new IdentityUser { UserName = clienteEmail, Email = clienteEmail };
        var result = await userManager.CreateAsync(clienteUser, clientePassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(clienteUser, "Cliente");
        }
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
