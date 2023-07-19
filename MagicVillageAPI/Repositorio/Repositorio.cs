using MagicVillageAPI.Datos;
using MagicVillageAPI.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace MagicVillageAPI.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class //Implementación de interfaz generica.
    {

        private readonly ApplicationDbContext _context;
        internal DbSet<T> DbSeset;

        public Repositorio(ApplicationDbContext context)
        {
            _context = context;
            this.DbSeset = _context.Set<T>();
        }


        public async Task Crear(T entidad)
        {
            await DbSeset.AddAsync(entidad);
            await Grabar();
        }

        public async Task Grabar()
        {
           await _context.SaveChangesAsync();
        }

        public async Task<T> Obtener(Expression<Func<T, bool>> filtro = null, bool tracked = true)
        {

            IQueryable<T> query = DbSeset;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            return await query.FirstOrDefaultAsync();
           
        }

        public async Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null)
        {
            IQueryable<T> query = DbSeset;

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            return await query.ToListAsync();

        }

        public async Task Remover(T entidad)
        {
            DbSeset.Remove(entidad);
            await Grabar();
        }
    }
}
