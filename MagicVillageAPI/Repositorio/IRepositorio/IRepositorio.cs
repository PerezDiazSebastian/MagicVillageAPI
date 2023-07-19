using System.Linq.Expressions;

namespace MagicVillageAPI.Repositorio.IRepositorio
{
    public interface IRepositorio<T> where T : class //Interfaz generico, donde T es cualquier tipo de entidad, dependiendo con la que se este pasando.
    {

        Task Crear(T entidad);

        Task<List<T>> ObtenerTodos(Expression<Func<T,bool>>? filtro = null); //Para recibir xpresion flecha.

        Task<T> Obtener(Expression<Func<T, bool>> filtro = null, bool tracked = true);

        Task Remover(T entidad);

        Task Grabar();
    }
}
