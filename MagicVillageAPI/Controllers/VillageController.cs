using MagicVillageAPI.Datos;
using MagicVillageAPI.Models;
using MagicVillageAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
//Clase de C# para crear los endopoints
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MagicVillageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillageController : ControllerBase
    {

        private readonly ILogger<VillageController> _logger; //Las variables privadas llevan guion bajo.
        public VillageController(ILogger<VillageController> logger)
        {
            _logger = logger; //Inyeccion del servicio de logger, y inicializacion de la variables logge.
        }


        // GET: api/<VillageController>
        //Evitar error de documentacion 
        [HttpGet]
        public ActionResult<IEnumerable<VillageDto>> GetVillages()
        {
            _logger.LogInformation("Obtener las villas");
            return Ok(VillageStore.listVillage);
            //Action resultado como un codigo de consulta cliente
        }

        //GET api/<VillageController>/5
        [HttpGet("{id}", Name = "Get Village")]
        [ProducesResponseType(200)] //Definir codigos de estado.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<VillageDto>> GetVillage(int id)
        {

            if (id == 0)
            {
                _logger.LogInformation("Error al mostrar la informacion con el Id"+id);
                return BadRequest();
            }

            var village = (VillageStore.listVillage.FirstOrDefault(v => v.Id == id)); //Expresion lambda de igualdad


            if (village == null) {

                return NotFound();
            }

            return Ok(village);

        }

        // POST api/<VillageController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillageDto> CrearVillage([FromBody] VillageDto villageDto)
        {


            if (!ModelState.IsValid) //Validacion de datos del modelo, con propiedad Required.
            {
                return BadRequest(villageDto);
            }

            if (VillageStore.listVillage.FirstOrDefault(v => v.Nombre.ToLower() == villageDto.Nombre.ToLower()) != null) //Validacion de datos del modelo, con propiedad Required.
            {
                ModelState.AddModelError("Villa Existe", "La villa con ese nombre ya existe");
                return BadRequest(ModelState);
            }


            if (villageDto == null)
            {
                return BadRequest(villageDto);
            }

            if (villageDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            villageDto.Id = VillageStore.listVillage.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            VillageStore.listVillage.Add(villageDto);

            return CreatedAtRoute("GetVillage", new { id = villageDto.Id }, villageDto); //Se debe indicar la url del recurso creado
        }

        // PUT api/<VillageController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVillage(int id, [FromBody] VillageDto villageDto)
        {
            //El IActionResult, no necesita el modelo, Delete devuelve un "No content".

            if (villageDto.Id != id || villageDto == null)
            {
                return BadRequest();
            }

            var villa = VillageStore.listVillage.FirstOrDefault(v => v.Id == id);

            villa.Nombre = villageDto.Nombre;
            villa.Ocupantes = villageDto.Ocupantes;
            villa.MetrosCuadros = villageDto.MetrosCuadros;

            return NoContent();

        }


        // PUT api/<VillageController>/5
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVillage(int id, JsonPatchDocument <VillageDto> patchDto)
        {
            //El IActionResult, no necesita el modelo, Delete devuelve un "No content".

            if (id == 0 || patchDto == null)
            {
                return BadRequest();
            }

            var villa = VillageStore.listVillage.FirstOrDefault(v => v.Id == id);

            patchDto.ApplyTo(villa, ModelState);

            if(!ModelState.IsValid){
                return BadRequest();
            }

            return NoContent();

        }


        // DELETE api/<VillageController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVillage(int id)
        {

            //El IActionResult, no necesita el modelo, Delete devuelve un "No content".       

            if (id == 0)
            {
                return BadRequest();
            }

            var villa = VillageStore.listVillage.FirstOrDefault(v => v.Id == id);
            if ( id == 0)
            {
                return NotFound();
            }

            return NoContent();

        }




    }
}
