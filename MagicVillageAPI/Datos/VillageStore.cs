using MagicVillageAPI.Models.Dto;

namespace MagicVillageAPI.Datos
{
    public static class VillageStore
    {

        public static List<VillageUpdateDto> listVillage = new List<VillageUpdateDto>
       {
            new VillageUpdateDto{Id=1, Nombre="Vista a la piscina", Ocupantes=3, MetrosCuadros=50},
            new VillageUpdateDto{Id=2, Nombre="Vista a la playa", Ocupantes=4, MetrosCuadros=80}

        };

    }

}


