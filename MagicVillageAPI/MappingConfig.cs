using AutoMapper;
using MagicVillageAPI.Models;
using MagicVillageAPI.Models.Dto;

namespace MagicVillageAPI
{
    public class MappingConfig :Profile
    {
        public MappingConfig()
        {

           //	AutoMapper, cambia objetos de un tipo a otro, para cambiar modelos.
            CreateMap<Village, VillageDto>();
            CreateMap<VillageDto, Village>();

            CreateMap<Village, VillageCreateDto>().ReverseMap();
            CreateMap<Village, VillageUpdateDto>().ReverseMap();

        }

    }
}
