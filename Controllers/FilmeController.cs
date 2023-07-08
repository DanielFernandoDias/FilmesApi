using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers
{
    [ApiController] // Indica que a classe é um controller
    [Route("[controller]")] // Indica que a rota do controller é o nome da classe sem o sufixo Controller
    public class FilmeController : ControllerBase
    {
        // injeção de depedencia
        private FilmeContext _context;
        private IMapper _mapper;
        public FilmeController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        /// <summary>
        /// Adiciona um filme ao banco de dados
        /// </summary>
        /// <param name="filmeDto"></param>
        /// <returns>IactionResult</returns>
        /// <response code="201">Caso a informação seja feita com sucesso</response>
        [HttpPost] // Indica que o método responde a requisições Post
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
        {
                //Faz o mapeamento de um DTO para o Model
                Filme filme = _mapper.Map<Filme>(filmeDto);
                _context.Filmes.Add(filme);
                _context.SaveChanges();
                return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id }, filme);
        }

        [HttpGet]
        public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50, [FromQuery] string? nomeCinema = null)
        {
            if(nomeCinema is null) 
            {
                return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).ToList());
                
            }
            return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).Where(filme => filme.Sessoes.Any(sessao => sessao.Cinema.Nome == nomeCinema)).ToList());
        }


        [HttpGet("{id}")]
        public IActionResult RecuperaFilmePorId(int id)
        {
           var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
           if(filme is null)
           {
               return NotFound();
           }
           var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
           return Ok(filmeDto);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarFilme(int id, 
            [FromBody] UpdateFilmeDto filmeDto)
        {

            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if( filme is null)
            {
                return NotFound();
            }
            // Atualiza as propriedades do filme com os valores do DTO
            _mapper.Map(filmeDto, filme);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult AtualizarFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
        {

            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if( filme is null)
            {
                return NotFound();
            }
            UpdateFilmeDto filmeAtualizar = _mapper.Map<UpdateFilmeDto>(filme);
            patch.ApplyTo(filmeAtualizar, ModelState);

            if(!TryValidateModel(filmeAtualizar))
            {
                return ValidationProblem(ModelState);
            }
            

            _mapper.Map(filmeAtualizar, filme);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarFilmePorId(int id)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if( filme is null) return NotFound();

            _context.Filmes.Remove(filme);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
