using MagicVillageAPI.Models;
using Microsoft.EntityFrameworkCore; //Necesario para conexion a la base de datos

namespace MagicVillageAPI.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options ) :base(options) 
        {
            
        }



        public DbSet<Village> Villages { get; set; }
    }
}
