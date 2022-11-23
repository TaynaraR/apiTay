using Microsoft.EntityFrameworkCore;
using ProjetoEscola_API.Models;
using System.Diagnostics.CodeAnalysis;
namespace ProjetoEscola_API.Data
{
    public class bibliotecaContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public bibliotecaContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlServer(Configuration.GetConnectionString("StringConexaoSQLServer"));
        }

        public DbSet<Aluno>? Aluno { get; set; }
        public DbSet<Curso>? Curso { get; set; }
        public DbSet<usuario>? Usuario { get; set; }

         public DbSet<Filme>? Filme { get; set; }
  
    }
}