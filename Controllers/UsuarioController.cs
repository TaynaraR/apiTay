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
    public class usuarioController : ControllerBase
    {
        private bibliotecaContext _context;
        public usuarioController(bibliotecaContext context)
        {
            // construtor
            _context = context;
        }
        [HttpGet]
        public ActionResult<List<usuario>> GetAll()
        {
            return _context.usuario.ToList();
        }

        [HttpGet("{usuarioId}")]
        public ActionResult<List<usuario>> Get(int usuarioId)
        {
            try
            {
                var result = _context.usuario.Find(usuarioId);
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
        [HttpPost]
        public async Task<ActionResult> post(usuario model)
        {
            try
            {
                _context.usuario.Add(model);
                if (await _context.SaveChangesAsync() == 1)
                {
                    //return Ok();
                    return Created($"/api/usuario/{model.username}", model);
                }
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falha no acesso ao banco de dados.");
            }
            // retorna BadRequest se não conseguiu incluir
            return BadRequest();
        }
        [HttpDelete("{usuarioId}")]
        public async Task<ActionResult> delete(int usuarioId)
        {
            try
            {
                //verifica se existe usuario a ser excluído
                var usuario = await _context.usuario.FindAsync(usuarioId);
                if (usuario == null)
                {
                    //método do EF
                    return NotFound();
                }
                _context.Remove(usuario);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falhano acesso ao banco de dados.");
            }

        }
        [HttpPut("{usuarioId}")]
        public async Task<IActionResult> put(int usuarioId, usuario dadosusuarioAlt)
        {
            try
            {
                //verifica se existe usuario a ser alterado
                var result = await _context.usuario.FindAsync(usuarioId);
                if (usuarioId != result.id)
                {
                    return BadRequest();
                }
                result.username = dadosusuarioAlt.username;
                result.senha = dadosusuarioAlt.senha;
                result.role = dadosusuarioAlt.role;
                await _context.SaveChangesAsync();
                return Created($"/api/usuario/{dadosusuarioAlt.username}", dadosusuarioAlt);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Falhano acesso ao banco de dados.");
            }
        }
    }

}