using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVillageAPI.Models
{
    public class Village
    {
        [Key] //Llave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Incrementacion de Id, automatico.
        public int Id { get; set; }
     
        public string Nombre { get; set; }

        public string Detalle { get; set; }

        [Required]
        public  double Tarifa { get; set; }

        public int Ocupantes { get; set; }

        public int MetrosCuadros { get; set; }

        public string ImagenUrl { get; set; } //Ruta de la imagen

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }
    }
}
