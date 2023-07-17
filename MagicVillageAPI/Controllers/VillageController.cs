using AutoMapper;
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
        private readonly IMapper _mapper;


        public VillageController(ILogger<VillageController> logger, ApplicationDbContext context, IMapper mapper)
        {
            _logger = logger; //Inyeccion del servicio de logger, y inicializacion de la variables logger.
            _context = context;
        }


        // GET: api/<VillageController>
        //Evitar error de documentacion 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillageDto>>> GetVillagesAsync()
        {
            _logger.LogInformation("Obtener las villas");

            IEnumerable<Village> villageList = await _context.Villages.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<VillageDto>>(villageList));

            //Action resultado como un codigo de consulta cliente
        }

        //GET api/<VillageController>/5
        [HttpGet("{id}", Name = "Get Village")]
        [ProducesResponseType(200)] //Definir codigos de estado.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<VillageDto>>> GetVillage(int id)
        {

            if (id == 0)
            {
                _logger.LogError("Error al mostrar la informacion con el Id" + id);
                return BadRequest();
            }

            //var village = (VillageStore.listVillage.FirstOrDefault(v => v.Id == id)); //Expresion lambda de igualdad
            var village = await _context.Villages.FirstOrDefaultAsync(v => v.Id == id);

            if (village == null) {

                return NotFound();
            }

            return Ok(_mapper.Map<VillageDto>(village));

        }

        // POST api/<VillageController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillageDto>> CrearVillage([FromBody] VillageCreateDto createDto)
        {


            if (!ModelState.IsValid) //Validacion de datos del modelo, con propiedad Required.
            {
                return BadRequest(ModelState);
            }

            if (await _context.Villages.FirstOrDefaultAsync(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null) //Validacion de datos del modelo, con propiedad Required.
            {
                ModelState.AddModelError("Villa Existe", "La villa con ese nombre ya existe");
                return BadRequest(ModelState);
            }


            if (createDto == null)
            {
                return BadRequest(createDto);
            }

            Village modelo = _mapper.Map<Village>(createDto);


            await _context.Villages.AddAsync(modelo);//Insert en la bas
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVillage", new { id = modelo.Id }, modelo); //Se debe indicar la url del recurso creado
        }

        // PUT api/<VillageController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVillage(int id, [FromBody] VillageUpdateDto updateDto)
        {
            //El IActionResult, no necesita el modelo, Delete devuelve un "No content".

            if (updateDto.Id != id || updateDto == null)
            {
                return BadRequest();
            }

            //var villa = _context.Villages.FirstOrDefault(v => v.Id == id);

            //villa.Nombre = villageDto.Nombre;
            //villa.Ocupantes = villageDto.Ocupantes;
            //villa.MetrosCuadros = villageDto.MetrosCuadros;

            Village modelo = _mapper.Map<Village>(updateDto); //Convierte de un tipo de modelo  otro automaticamente.

            _context.Villages.Update(modelo); //No existe Update Async
            await _context.SaveChangesAsync();

            return NoContent();

        }


        // PUT api/<VillageController>/5
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVillage(int id, JsonPatchDocument<VillageUpdateDto> patchDto)
        {
            //El IActionResult, no necesita el modelo, Delete devuelve un "No content".

            if (id == 0 || patchDto == null)
            {
                return BadRequest();
            }

            var village = _context.Villages.AsNoTracking().FirstOrDefault(v => v.Id == id);
            //Problema de tracking, permite consultar un registro sin trackearlo, al usar un registro y volverlo a instanciar.

        
            VillageUpdateDto villageDto = _mapper.Map<VillageUpdateDto>(village);

            if (village == null)
            {
                return BadRequest();
            }

            patchDto.ApplyTo(villageDto, ModelState);

            if(!ModelState.IsValid){
                return BadRequest();
            }

            Village modelo = _mapper.Map<Village>(villageDto);


            _context.Villages.Update(modelo);
             await  _context.SaveChangesAsync();
            return NoContent();

        }


        // DELETE api/<VillageController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task< IActionResult> DeleteVillage(int id)
        {

            //El IActionResult, no necesita el modelo, Delete devuelve un "No content".
            //Delete no es un metodo asyncrono

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
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
