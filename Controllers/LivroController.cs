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
    public class LivroController : ControllerBase
    {
       private  bibliotecaContext _context;
       public LivroController(bibliotecaContext context)
        {
            // construtor
            _context = context;
        }

        [HttpGet]
public ActionResult<List<Livro>> GetAll()
{
return _context.Livro.ToList();
}

        [HttpPost]
        public async Task<ActionResult> post(Livro model)
        {
            try
            {
                _context.Livro.Add(model);
                if (await _context.SaveChangesAsync() == 1)
                {
                    //return Ok();
                    return Created($"/api/Livro/{model.codLivro}", model);
                }
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
           
            return BadRequest();
        }

        [HttpGet("{LivroId}")]
        public ActionResult<List<Livro>> Get(int LivroId)
        {
            try
            {
                var result = _context.Livro.Find(LivroId);
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
        [HttpDelete("{LivroId}")]
        public async Task<ActionResult> delete(int LivroId)
        {
            try
            {
                //verifica se existe Livro a ser excluído
                var Livro = await _context.Livro.FindAsync(LivroId);
                if (Livro == null)
                {
                    //método do EF
                    return NotFound();
                }
                _context.Remove(Livro);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
        }
        [HttpPut("{LivroId}")]
        public async Task<IActionResult> put(int LivroId, Livro dadosLivroAlt)
        {
            try
            {
                var result = await _context.Livro.FindAsync(LivroId);
                if (LivroId != result.id)
                {
                    return BadRequest();
                }
                result.codLivro = dadosLivroAlt.codLivro;
                result.nomeLivro = dadosLivroAlt.nomeLivro;
                result.dataLivro = dadosLivroAlt.dataLivro;
                result.imagem = dadosLivroAlt.imagem;
                await _context.SaveChangesAsync();
                return Created($"/api/Livro/{dadosLivroAlt.codLivro}", dadosLivroAlt);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }


        }
    }
}