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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definindo chave primária composta na sessão
            modelBuilder.Entity<Sessao>()
                .HasKey(sessao => new { sessao.FilmeId, sessao.CinemaId });
            
            // Definindo relacionamento entre sessão e cinema
            modelBuilder.Entity<Sessao>().HasOne(Sessao => Sessao.Cinema)
                .WithMany(filme => filme.Sessoes).HasForeignKey(sessao => sessao.CinemaId);

            // Definindo relacionamento entre sessão e filme
            modelBuilder.Entity<Sessao>().HasOne(Sessao => Sessao.Filme)
                .WithMany(filme => filme.Sessoes).HasForeignKey(sessao => sessao.FilmeId);

            // Definindo o relacionamento entre cinema e endereco e o comportamento de deleção como restrict
            modelBuilder.Entity<Endereco>().HasOne(endereco => endereco.Cinema)
                .WithOne(cinema => cinema.Endereco)
                .OnDelete(DeleteBehavior.Restrict);
        }

        // DbSet é uma coleção de entidades
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Endereco> Enderecos { get; set;}
        public DbSet<Sessao> Sessoes { get; set; }
    }
}
