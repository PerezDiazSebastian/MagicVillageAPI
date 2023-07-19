using MagicVillageAPI.Models;

namespace MagicVillageAPI.Repositorio.IRepositorio
{
    public interface IVillaRepositorio : IRepositorio<Village>
    {
        Task<Village> Actualizar (Village village);

    }
}
