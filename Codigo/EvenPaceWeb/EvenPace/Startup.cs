using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Service;

namespace EvenPaceWeb
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Injeção de dependência Services
            services.AddTransient<IAdministradorService, AdministradorService>();
            services.AddTransient<IAvaliacaoEventoService, AvaliacaoEventoService>();
            services.AddTransient<ICartaoCreditoService, CartaoCreditoService>();
            services.AddTransient<IEventosService, EventoService>();
            services.AddTransient<ICorredorService, CorredorService>();
            services.AddTransient<IInscricaoService, InscricaoService>();
            services.AddTransient<ICupomService, CupomService>();
            services.AddTransient<IKitService, KitService>();
            services.AddTransient<IOrganizacaoService, OrganizacaoService>();

            // Injeção de dependência AutoMapper
            services.AddAutoMapper(typeof(Startup).Assembly);

                        
            var connectionString = Configuration.GetConnectionString("EvenPaceDatabase");
            
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string 'EvenPaceDatabase' não encontrada.");

            services.AddDbContext<EvenPaceContext>(options =>
                options.UseMySQL(connectionString));
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Corredor}/{action=Index}/{id?}");
            });
        }
    }
}
