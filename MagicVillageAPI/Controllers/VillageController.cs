using AutoMapper;
using MagicVillageAPI.Datos;
using MagicVillageAPI.Models;
using MagicVillageAPI.Models.Dto;
using MagicVillageAPI.Repositorio.IRepositorio;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
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
        private readonly IVillaRepositorio _villaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _apiresponse;


        public VillageController(ILogger<VillageController> logger, IVillaRepositorio villaRepositorio, IMapper mapper)
        {
            _logger = logger; //Inyeccion del servicio de logger, y inicializacion de la variables logger.
            _villaRepo = villaRepositorio;
            _mapper = mapper;
            _apiresponse = new APIResponse();
        }


        // GET: api/<VillageController>
        //Evitar error de documentacion 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillages()
        {
            try
            {

                _logger.LogInformation("Obtener las villas");

                IEnumerable<Village> villageList = await _villaRepo.ObtenerTodos();

                _apiresponse.Resultado = _mapper.Map<IEnumerable<VillageDto>>(villageList);
                _apiresponse.StatusCode = HttpStatusCode.OK;


            }
            catch (Exception ex)
            {
                _apiresponse.IsExitoso = false;
                _apiresponse.ErrorMessage = new List<string>
                {
                    ex.ToString()
                };

            }

            return _apiresponse;

            //Action resultado como un codigo de consulta cliente
        }

        //GET api/<VillageController>/5
        [HttpGet("{id}", Name = "Get Village")]
        [ProducesResponseType(200)] //Definir codigos de estado.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillage(int id)
        {

            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al mostrar la informacion con el Id" + id);
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiresponse.IsExitoso = false;
                    return BadRequest(_apiresponse);
                }

                //var village = (VillageStore.listVillage.FirstOrDefault(v => v.Id == id)); //Expresion lambda de igualdad
                var village = await _villaRepo.Obtener();

                if (village == null)
                {
                    _apiresponse.StatusCode = HttpStatusCode.NotFound;
                    _apiresponse.IsExitoso = false;
                    return NotFound(_apiresponse);
                }

                _apiresponse.Resultado = _mapper.Map<VillageDto>(village);
                _apiresponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiresponse);

            }
            catch (Exception ex)
            {
                _apiresponse.IsExitoso = false;
                _apiresponse.ErrorMessage = new List<string>
                {
                    ex.ToString()
                };

            }

            return _apiresponse;
        }

        // POST api/<VillageController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearVillage([FromBody] VillageCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid) //Validacion de datos del modelo, con propiedad Required.
                {
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null) //Validacion de datos del modelo, con propiedad Required.
                {
                    ModelState.AddModelError("Villa Existe", "La villa con ese nombre ya existe");
                    return BadRequest(ModelState);
                }


                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                Village modelo = _mapper.Map<Village>(createDto);
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;
                await _villaRepo.Crear(modelo);
                _apiresponse.Resultado = modelo;
                _apiresponse.StatusCode = HttpStatusCode.OK;
                return CreatedAtAction("GetVillage", new { id = modelo.Id }, _apiresponse); //Se debe indicar la url del recurso creado
            }
            catch (Exception ex)
            {
                _apiresponse.IsExitoso = false;
                _apiresponse.ErrorMessage = new List<string>
                {
                    ex.ToString()
                };

            }

            //await _context.Villages.AddAsync(modelo);//Insert en la bas
            //await _context.SaveChangesAsync();

            return _apiresponse;
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
                _apiresponse.IsExitoso = false;
                _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest();
            }

            //var villa = _context.Villages.FirstOrDefault(v => v.Id == id);

            //villa.Nombre = villageDto.Nombre;
            //villa.Ocupantes = villageDto.Ocupantes;
            //villa.MetrosCuadros = villageDto.MetrosCuadros;

            Village modelo = _mapper.Map<Village>(updateDto); //Convierte de un tipo de modelo  otro automaticamente.

            //_context.Villages.Update(modelo); //No existe Update Async
            //await _context.SaveChangesAsync();

            await _villaRepo.Actualizar(modelo);
            _apiresponse.StatusCode = HttpStatusCode.NoContent;

            return Ok(_apiresponse);

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

            var village = await _villaRepo.Obtener(v => v.Id == id, tracked: false);
            //Problema de tracking, permite consultar un registro sin trackearlo, al usar un registro y volverlo a instanciar.


            VillageUpdateDto villageDto = _mapper.Map<VillageUpdateDto>(village);

            if (village == null) return BadRequest(_apiresponse);

            patchDto.ApplyTo(villageDto, ModelState);

            if(!ModelState.IsValid){
    
                return BadRequest(ModelState);
            }

            Village modelo = _mapper.Map<Village>(villageDto);

           await _villaRepo.Actualizar(modelo);
            _apiresponse.StatusCode = HttpStatusCode.NoContent;
            return Ok(_apiresponse);

        }


        // DELETE api/<VillageController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVillage(int id)
        {

            //El IActionResult, no necesita el modelo, Delete devuelve un "No content".
            //Delete no es un metodo asyncrono

            try
            {

                if (id == 0)
                {
                    _apiresponse.IsExitoso = false;
                    _apiresponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiresponse);
                }

                var village = await _villaRepo.Obtener(v => v.Id == id);
                if (village == null)
                {
                    _apiresponse.IsExitoso = false;
                    _apiresponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_apiresponse);
                }

                await _villaRepo.Remover(village);
                _apiresponse.StatusCode = HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                _apiresponse.IsExitoso = false;
                _apiresponse.ErrorMessage = new List<string>
                {
                    ex.ToString()
                };

            }

            return BadRequest(_apiresponse);
        }

    }
}
