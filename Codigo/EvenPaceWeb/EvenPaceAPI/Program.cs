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
        
        builder.Services.AddControllers();

        builder.Services.AddTransient<ICorredorService, CorredorService>();
        builder.Services.AddTransient<IAvaliacaoEventoService, AvaliacaoEventoService>();
        
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddAutoMapper(typeof(Program).Assembly);
        
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
        
        app.Run();
    }
}
