using MagicVillageAPI.Datos;
using MagicVillageAPI.Models;
using MagicVillageAPI.Repositorio.IRepositorio;
using System.Linq.Expressions;

namespace MagicVillageAPI.Repositorio
{
    public class VillaRepositorio : Repositorio<Village>, IVillaRepositorio

    {
        private readonly ApplicationDbContext _context;

        public VillaRepositorio(ApplicationDbContext context) : base(context)//Hereda del repositorio, del padre al hijo, ya que este ya tiene la inyeccion.
        {
            _context = context;
        }

        public async Task<Village> Actualizar(Village village)
        {
            village.FechaActualizacion = DateTime.Now;//Cuando la entidad es de fecha
            _context.Villages.Update(village);
            await _context.SaveChangesAsync();
            return village;
        }
    }
}
