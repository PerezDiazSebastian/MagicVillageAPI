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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Village>().HasData(
                new Village(){
                    Id = 1,
                    Nombre = "Villa Real",
                    Detalle = "Detalle de la Villa..",
                    ImagenUrl = "",
                    Ocupantes = 5,
                    MetrosCuadros = 50, 
                    Tarifa = 200,
                    FechaCreacion= DateTime.Now,
                    FechaActualizacion = DateTime.Now
                },
                new Village()
                {
                    Id = 2,
                    Nombre = "Premium vista a la piscina",
                    Detalle = "Detalle de la Villa..",
                    ImagenUrl = "",
                    Ocupantes = 4,
                    MetrosCuadros = 40,
                    Tarifa = 150,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                });

        } //Alimentar tabla de regirstros 

    }
}
