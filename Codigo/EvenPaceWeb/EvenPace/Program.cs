using Castle.Core.Smtp;
using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Service;
using EvenPaceWeb.Areas.Identity.Data;
using EvenPaceWeb.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(EvenPaceWeb.Mappers.AutoMapperProfile));
builder.Services.AddScoped<ICorredorService, CorredorService>();
builder.Services.AddScoped<IAuthService, EvenPace.Service.AuthService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<EvenPaceContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    )
);

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("IdentityDatabase"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("IdentityDatabase")
        )
    ));

builder.Services.AddScoped<IInscricaoService, InscricaoService>();
builder.Services.AddScoped<IEventosService, EventoService>();
builder.Services.AddScoped<IKitService, KitService>();
builder.Services.AddScoped<ICorredorService, CorredorService>();
builder.Services.AddScoped<IAdministradorService, AdministradorService>();
builder.Services.AddScoped<IAvaliacaoEventoService, AvaliacaoEventoService>();
builder.Services.AddScoped<ICartaoCreditoService, CartaoCreditoService>();
builder.Services.AddScoped<ICupomService, CupomService>();
builder.Services.AddScoped<IOrganizacaoService, OrganizacaoService>();


builder.Services.AddDefaultIdentity<UsuarioIdentity>(options =>
    {
        // SignIn settings
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;

        // Passowrd settings
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;

        // Default User settings
        options.User.AllowedUserNameCharacters =
            "abcdefgkijklmnopkrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true; // Trocado de false para true para realziação de teste -- Voltar ao normal depois 

        // Default Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

    }
).AddEntityFrameworkStores<IdentityContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.Cookie.Name = "EvenPace";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Corredor/Login";
    // ReturnUrlParameter requires
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
