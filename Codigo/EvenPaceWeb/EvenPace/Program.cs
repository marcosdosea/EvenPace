using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Service;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(EvenPaceWeb.Mappers.AutoMapperProfile));

var connectionString = "Server=localhost;Database=evenpace;Uid=root;Pwd=Hevellyn09;";

builder.Services.AddDbContext<EvenPaceContext>(options =>
    options.UseMySQL(connectionString));

builder.Services.AddScoped<IInscricaoService, InscricaoService>();
builder.Services.AddScoped<IEventosService, EventoService>();
builder.Services.AddScoped<IKitService, KitService>();

var app = builder.Build();


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
    pattern: "{controller=Inscricao}/{action=TelaInscricao}/{id?}");

app.Run();
