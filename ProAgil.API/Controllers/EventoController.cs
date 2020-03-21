using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.API.Dtos;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase //herda para trabalhar com http e etc
    {
        private readonly IProAgilRepository _repo;
        //injeção por meio de interface o repositorio
        private readonly IMapper _mapper;
        public EventoController(IProAgilRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            _repo = repo;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _repo.GetAllEventoAsync(true);

                var results = _mapper.Map<EventoDto[]>(eventos);//_mapper.Map<EventoDto[]>(eventos); works too!

                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"BD falhou{ex.Message}");
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> upload()
        {
            try
            {
                //todo arquivo vem como um array- logo [0]
                var file = Request.Form.Files[0];//pega o arquivo
                var folderName = Path.Combine("Resources","Images");// diretorio onde será armazenado
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);//combina o diretório que deseja salvar mais o diretório da aplicação

                if(file.Length > 0){
                    //filename que será armazenado esta vindo do header
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;//
                    var fullPath = Path.Combine(pathToSave, fileName.Replace("\"", "").Trim());//determina o caminho completo e trata se vier com " ou espaço

                    using (var stream = new FileStream(fullPath, FileMode.Create)){
                        //arquivo que recebeu e copia para o stream
                        file.CopyTo(stream);
                    }
                }
                return Ok();
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"BD falhou{ex.Message}");
            }

            return BadRequest("Erro ao tentar realizar upload");
        }
        [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int EventoId)
        {
            try
            {
                var evento = await _repo.GetAllEventoAsyncById(EventoId, true);

                var results = _mapper.Map<EventoDto>(evento);

                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "BD falhou");
            }
        }
        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                var eventos = await _repo.GetAllEventoAsyncByTema(tema, true);

                var results = _mapper.Map<IEnumerable<EventoDto>>(eventos);


                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"BD falhou {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {

                //Mapeando o Evento Dto (Dto/mapeamento inverso)
                var evento = _mapper.Map<Evento>(model);
                //mudança de estado
                _repo.Add(evento);

                //salva mudança de estado
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
                }
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"BD falhou {ex.Message}");
            }
            return BadRequest("Falhou");
        }
        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId, EventoDto model)
        {
            try
            {
                var evento = await _repo.GetAllEventoAsyncById(EventoId, false);
                if (evento == null) return NotFound();

                _mapper.Map(model,evento);
                //mudança de estado
                _repo.Update(evento);

                //salva mudança de estado
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "BD falhou");
            }
            return BadRequest("Falhou");
        }
        [HttpDelete("{EventoId}")]
        public async Task<IActionResult> Delete(int EventoId)
        {
            try
            {
                var evento = await _repo.GetAllEventoAsyncById(EventoId, false);
                if (evento == null) return NotFound();

                //mudança de estado
                _repo.Delete(evento);

                //salva mudança de estado
                if (await _repo.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "BD falhou");
            }
            return BadRequest("Falhou");
        }
    }
}