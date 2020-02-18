using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class PalestranteController : ControllerBase
    {
        private readonly IProAgilRepository _repo;
        //injeção por meio de interface o repositorio
        public PalestranteController(IProAgilRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _repo.GetAllPalestranteAsync(true);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"BD falhou");
            }
        }
        [HttpGet("{PalestranteId}")]
        public async Task<IActionResult> Get(int PalestranteId)
        {
            try
            {
                var results = await _repo.GetAllEventoAsyncById(PalestranteId, true);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"BD falhou");
            }
        }
        [HttpGet("getByName/{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                var results = await _repo.GetAllPalestranteAsyncByName(name, true);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"BD falhou");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(Palestrante model)
        {
            try
            {
                //mudança de estado
                _repo.Add(model);

                //salva mudança de estado
                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", model);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"BD falhou");
            }
            return BadRequest("Falhou");
        }
        [HttpPut]
        public async Task<IActionResult> Put(int PalestranteId, Evento model)
        {
            try
            {
                var evento = await _repo.GetAllPalestranteAsyncById(PalestranteId, false);
                if(evento == null) return NotFound();
                //mudança de estado
                _repo.Update(model);

                //salva mudança de estado
                if(await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", model);
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"BD falhou");
            }
            return BadRequest("Falhou");
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int PalestranteId)
        {
            try
            {
                var evento = await _repo.GetAllPalestranteAsyncById(PalestranteId, false);
                if(evento == null) return NotFound();

                //mudança de estado
                _repo.Delete(evento);

                //salva mudança de estado
                if(await _repo.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"BD falhou");
            }
            return BadRequest("Falhou");
        }
    }
}