using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVillageAPI.Models
{
    public class NumeroVilla
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNum { get; set; }


        [Required]
        public string VillaId { get; set; }

        //Creación una navegación a la tabla que queremos relacionar.

        [ForeignKey("VillaId")]

        public Village Village { get; set; }

        public String? DetalleEspecial { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime Actualizacoin { get; set; }
    }
}
