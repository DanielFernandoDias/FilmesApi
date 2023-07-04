using FilmesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmesApi.Data
{
    public class FilmeContext : DbContext
    {
        // Construtor padrão
        public FilmeContext(DbContextOptions<FilmeContext> options) : base(options)
        {
            
        }

        // DbSet é uma coleção de entidades
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Endereco> Enderecos { get; set;}
    }
}
