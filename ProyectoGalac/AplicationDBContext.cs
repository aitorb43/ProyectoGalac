using Microsoft.EntityFrameworkCore;
using ProyectoGalac.Entidades;

namespace ProyectoGalac
{
    public class AplicationDBContext : DbContext
    {
        public AplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Producto> Productos { set; get; }
        public DbSet<Log> Logs { set; get; }

    }
}
