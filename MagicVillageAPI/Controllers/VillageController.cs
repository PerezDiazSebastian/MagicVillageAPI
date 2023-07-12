using MagicVillageAPI.Datos;
using MagicVillageAPI.Models;
using MagicVillageAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
//Clase de C# para crear los endopoints
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MagicVillageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillageController : ControllerBase
    {
        // GET: api/<VillageController>
        //Evitar error de documentacion 
        [HttpGet]
        public ActionResult <IEnumerable<VillageDto>> GetVillages()
       {
            return Ok (VillageStore.listVillage);
            //Action resultado como un codigo de consulta cliente
        }

        //GET api/<VillageController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)] //Definir codigos de estado.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<VillageDto>> GetVillage(int id)
        {

            if(id == 0)
            {

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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<VillageDto> CrearVillage([FromBody] VillageDto villageDto)
        {
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
            return Ok(villageDto);

        }

        //// PUT api/<VillageController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<VillageController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}




    }
}
