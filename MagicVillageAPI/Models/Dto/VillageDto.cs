using System.ComponentModel.DataAnnotations;

namespace MagicVillageAPI.Models.Dto
{
    public class VillageDto
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Nombre { get; set; }

        public string Detalle { get; set; }

        [Required]
        public double Tarifa { get; set; }

        public int Ocupantes { get; set; }

        public int MetrosCuadros { get; set; }

        public string ImagenUrl { get; set; } //Ruta de la imagen


    }
}

