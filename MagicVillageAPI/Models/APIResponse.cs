using System.Net;

namespace MagicVillageAPI.Models
{

    //Se encarga de las respuesta de los endpoints, y se ajusten segun el contenido de la respuesta
    public class APIResponse
    {

        public HttpStatusCode StatusCode { get; set; }
        public bool IsExitoso { get; set; } = true;
        public List<String> ErrorMessage { get; set; }
        public object Resultado { get; set; }
    }
}
