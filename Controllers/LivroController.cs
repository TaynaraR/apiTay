using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetoEscola_API.Data;
using ProjetoEscola_API.Models;

namespace ProjetoEscola_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmeController : ControllerBase
    {
       private  bibliotecaContext _context;
       public FilmeController(bibliotecaContext context)
        {
            // construtor
            _context = context;
        }

        [HttpGet]
public ActionResult<List<Filme>> GetAll()
{
return _context.Filme.ToList();
}

        [HttpPost]
        public async Task<ActionResult> post(Filme model)
        {
            try
            {
                _context.Filme.Add(model);
                if (await _context.SaveChangesAsync() == 1)
                {
                    //return Ok();
                    return Created($"/api/Filme/{model.codFilme}", model);
                }
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
           
            return BadRequest();
        }

        [HttpGet("{FilmeId}")]
        public ActionResult<List<Filme>> Get(int FilmeId)
        {
            try
            {
                var result = _context.Filme.Find(FilmeId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }
        [HttpDelete("{FilmeId}")]
        public async Task<ActionResult> delete(int FilmeId)
        {
            try
            {
                //verifica se existe Filme a ser excluído
                var Filme = await _context.Filme.FindAsync(FilmeId);
                if (Filme == null)
                {
                    //método do EF
                    return NotFound();
                }
                _context.Remove(Filme);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }
        [HttpPut("{FilmeId}")]
        public async Task<IActionResult> put(int FilmeId, Filme dadosFilmeAlt)
        {
            try
            {
                var result = await _context.Filme.FindAsync(FilmeId);
                if (FilmeId != result.id)
                {
                    return BadRequest();
                }
                result.codFilme = dadosFilmeAlt.codFilme;
                result.nomeFilme = dadosFilmeAlt.nomeFilme;
                result.dataFilme = dadosFilmeAlt.dataFilme;
                result.imagem = dadosFilmeAlt.imagem;
                await _context.SaveChangesAsync();
                return Created($"/api/Filme/{dadosFilmeAlt.codFilme}", dadosFilmeAlt);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }


        }
    }
}