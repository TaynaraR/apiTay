namespace ProjetoEscola_API.Models
{
    public class Filme
    {
        public int id {get; set;}

        public int codFilme {get; set;}

        public string? nomeFilme { get; set;}

        public string? dataFilme { get; set;}

         public string? imagem { get; set;}
          public string? alugadoPor { get; set;}

        public Boolean? alugado {get; set;}
    }
}