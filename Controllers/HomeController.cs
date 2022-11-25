using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjetoEscola_API.Data;
using ProjetoEscola_API.Models;
namespace ProjetoEscola_API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly bibliotecaContext? _context;
        public HomeController(

        IConfiguration configuration,
        bibliotecaContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]

        // mando meu email e senha esperando a resposta que vira 
        // um token e o user em contexto se acaso essa conta existir
        public ActionResult<dynamic> Login([FromBody] usuario usuario)
        {
            
            var perfil = _context.usuario.Where(u => u.username == usuario.username &&

            u.senha == usuario.senha)

            .FirstOrDefault();

           
            if (perfil == null)
                return Unauthorized("Usuário ou senha inválidos");
            var authClaims = new List<Claim> {
            new Claim(ClaimTypes.Name, perfil.username),
            new Claim(ClaimTypes.Role, perfil.role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
};
            var token = GetToken(authClaims);
            perfil.senha = "";
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                user = perfil
            });
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => String.Format("Autenticado - {0}",
        User.Identity.Name);


        // Saber se o usuario é admin
        [HttpGet]
        [Route("Admin")]
        [Authorize(Roles = "Admin")]
        public bool admin() => true;

        // Saber se o usuario é um cliente
        [HttpGet]
        [Route("Cliente")]
        [Authorize(Roles = "Cliente")]
        public bool cliente() => true;

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new

            SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
            // tempo que o token fica disponivel
            expires: DateTime.Now.AddHours(3),
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey,

            SecurityAlgorithms.HmacSha256)

            );
            return token;
        }
    }
}