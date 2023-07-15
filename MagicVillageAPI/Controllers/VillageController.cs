using MagicVillageAPI.Datos;
using MagicVillageAPI.Models;
using MagicVillageAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext _context;//Conexión base de datos

        
        
        public VillageController(ILogger<VillageController> logger, ApplicationDbContext context)
        {
            _logger = logger; //Inyeccion del servicio de logger, y inicializacion de la variables logger.
            _context = context;
        }


        // GET: api/<VillageController>
        //Evitar error de documentacion 
        [HttpGet]
        public ActionResult<IEnumerable<VillageDto>> GetVillages()
        {
            _logger.LogInformation("Obtener las villas");
            return Ok(_context.Villages.ToList());
            //Action resultado como un codigo de consulta cl n iente
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
                _logger.LogError("Error al mostrar la informacion con el Id"+id);
                return BadRequest();
            }

            //var village = (VillageStore.listVillage.FirstOrDefault(v => v.Id == id)); //Expresion lambda de igualdad
            var village = _context.Villages.FirstOrDefault(v => v.Id == id);

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

            if (_context.Villages.FirstOrDefault(v => v.Nombre.ToLower() == villageDto.Nombre.ToLower()) != null) //Validacion de datos del modelo, con propiedad Required.
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


            Village modelo = new()
            {
                Nombre = villageDto.Nombre,
                Detalle = villageDto.Detalle,
                ImagenUrl = villageDto.ImagenUrl,
                Ocupantes = villageDto.Ocupantes,
                Tarifa = villageDto.Tarifa,
                MetrosCuadros = villageDto.MetrosCuadros
            };

            _context.Villages.Add(modelo);//Insert en la base
            _context.SaveChanges();

            return CreatedAtAction("GetVillage", new { id = villageDto.Id }, villageDto); //Se debe indicar la url del recurso creado
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

            //var villa = _context.Villages.FirstOrDefault(v => v.Id == id);

            //villa.Nombre = villageDto.Nombre;
            //villa.Ocupantes = villageDto.Ocupantes;
            //villa.MetrosCuadros = villageDto.MetrosCuadros;


            Village modelo = new() //Creaión de un modelo y asignación de valores
            {
                Nombre = villageDto.Nombre,
                Detalle = villageDto.Detalle,
                ImagenUrl = villageDto.ImagenUrl,
                Ocupantes = villageDto.Ocupantes,
                Tarifa = villageDto.Tarifa,
                MetrosCuadros = villageDto.MetrosCuadros
            };

            _context.Villages.Update(modelo);
            _context.SaveChanges();

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

            var village = _context.Villages.AsNoTracking().FirstOrDefault(v => v.Id == id);
            //Problema de tracking, permite consultar un registro sin trackearlo, al usar un registro y volverlo a instanciar.


            VillageDto villageDto = new() //Crear un modelo de Dto , al estar trabajando con esa clase en el patchDto
            {
                Id = village.Id,
                Nombre = village.Nombre,
                Detalle = village.Detalle,
                ImagenUrl = village.ImagenUrl,
                Ocupantes = village.Ocupantes,
                Tarifa = village.Tarifa,
                MetrosCuadros = village.MetrosCuadros

                //Antes de actualizar, se hace una actualizción temporal en el modelo Dto
                
            };

            if (villageDto == null)
            {
                return BadRequest();
            }

            patchDto.ApplyTo(villageDto, ModelState);

            if(!ModelState.IsValid){
                return BadRequest();
            }

            Village modelo = new() 
            {
                Id = villageDto.Id,
                Nombre = villageDto.Nombre,
                Detalle = villageDto.Detalle,
                ImagenUrl = villageDto.ImagenUrl,
                Ocupantes = villageDto.Ocupantes,
                Tarifa = villageDto.Tarifa,
                MetrosCuadros = villageDto.MetrosCuadros

                //Creacion de modelo tipo Village, para actualizar finalmente en la tabla real, luego de pasar el patchoDto ApplyTo 
            };

            _context.Villages.Update(modelo);
            _context.SaveChanges();
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

            var village = _context.Villages.FirstOrDefault(v => v.Id == id);
            if ( village == null )
            {
                return NotFound();
            }

            _context.Villages.Remove(village);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
