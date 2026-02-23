using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Service;

namespace  EvenPaceAPI;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

        builder.Services.AddTransient<ICorredorService, CorredorService>();
        
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddAutoMapper(typeof(Program).Assembly);
        builder.Services.AddTransient<IInscricaoService, InscricaoService>();
        builder.Services.AddTransient<IEventosService, EventoService>();
        builder.Services.AddTransient<IKitService, KitService>();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new Exception("Please set connection string in appsettings.json");
        }

        builder.Services.AddDbContext<EvenPaceContext>(options =>
            options.UseMySQL(connectionString));
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        
        app.UseAuthorization();
        
        app.MapControllers();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "EvenPace API V1");
            c.RoutePrefix = string.Empty; 
        });

        app.Run();
    }
}