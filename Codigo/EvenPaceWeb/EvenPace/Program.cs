using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Service;
using EvenPace.Mappers; // Adicionei para garantir que ache o AutoMapper

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURAÇÃO DOS SERVIÇOS (Trazido do Startup.cs) ---

builder.Services.AddControllersWithViews();

// Configuração do Banco de Dados (MySQL)
// Verifica se a string de conexão existe para evitar o aviso amarelo CS8604
var connectionString = builder.Configuration.GetConnectionString("EvenPaceDatabase");
builder.Services.AddDbContext<EvenPaceContext>(options =>
    options.UseMySQL(connectionString ?? ""));

// Registro dos Serviços (Injeção de Dependência)
builder.Services.AddTransient<IAdministradorService, AdministradorService>();
builder.Services.AddTransient<IAvaliacaoEventoService, AvaliacaoEventoService>();
builder.Services.AddTransient<ICartaoCreditoService, CartaoCreditoService>();
builder.Services.AddTransient<IEventosService, EventoService>();
builder.Services.AddTransient<ICorredorService, CorredorService>();
builder.Services.AddTransient<IInscricaoService, InscricaoService>();
builder.Services.AddTransient<ICupomService, CupomService>();

// A LINHA MÁGICA QUE FALTAVA:
builder.Services.AddTransient<IKitService, KitService>();

builder.Services.AddTransient<IOrganizacaoService, OrganizacaoService>();

// Configuração do AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// --- FIM DAS CONFIGURAÇÕES ---

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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