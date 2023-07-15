using MagicVillageAPI.Models.Dto;

namespace MagicVillageAPI.Datos
{
    public static class VillageStore
    {

        public static List<VillageDto> listVillage = new List<VillageDto>
       {
            new VillageDto{Id=1, Nombre="Vista a la piscina", Ocupantes=3, MetrosCuadros=50},
            new VillageDto{Id=2, Nombre="Vista a la playa", Ocupantes=4, MetrosCuadros=80}

        };

    }

}


