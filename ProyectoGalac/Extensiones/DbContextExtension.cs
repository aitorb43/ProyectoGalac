using Microsoft.EntityFrameworkCore;
using ProyectoGalac.Interfaces;
using ProyectoGalac.Servicios;

namespace ProyectoGalac.Extensiones
{
    public static class DbContextExtension
    {

        public static WebApplicationBuilder RegisterDbContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContextFactory<AplicationDBContext>(opt =>
            {

                string connectionString = builder.Configuration.GetConnectionString("TestDb");

                opt.UseSqlServer(connectionString);
            }, ServiceLifetime.Scoped);

            builder.Services.AddScoped<AplicationDBContext>(
            sp => sp.GetRequiredService<IDbContextFactory<AplicationDBContext>>()
            .CreateDbContext());

            return builder;
        }

        public static void ExecuteMigrations(this WebApplication app)
        {
            using var serviceScope = app.Services.CreateScope();
            serviceScope
           .ServiceProvider
           .GetRequiredService<IDbContextFactory<AplicationDBContext>>()
           .CreateDbContext()
           .Database
           .Migrate();

        }

        public static void RegisterAppServices(this WebApplicationBuilder builder)
        {
            
            builder.Services.AddScoped<IServicioProducto, ServicioProducto>();
 
        }
    }
}
