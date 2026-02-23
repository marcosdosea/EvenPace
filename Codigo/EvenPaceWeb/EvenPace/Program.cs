using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Service;
using EvenPaceWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(EvenPaceWeb.Mappers.AutoMapperProfile));
builder.Services.AddScoped<ICorredorService, CorredorService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<EvenPaceContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    )
);

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    ));

builder.Services.AddDefaultIdentity<UsuarioIdentity>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<IdentityContext>();

builder.Services.AddScoped<IInscricaoService, InscricaoService>();
builder.Services.AddScoped<IEventosService, EventoService>();
builder.Services.AddScoped<IKitService, KitService>();
builder.Services.AddScoped<ICorredorService, CorredorService>();
builder.Services.AddScoped<IAdministradorService, AdministradorService>();
builder.Services.AddScoped<IAvaliacaoEventoService, AvaliacaoEventoService>();
builder.Services.AddScoped<ICartaoCreditoService, CartaoCreditoService>();
builder.Services.AddScoped<ICupomService, CupomService>();
builder.Services.AddScoped<IOrganizacaoService, OrganizacaoService>();

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

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
