using System.ComponentModel.DataAnnotations;

namespace FilmesApi.Models
{
    public class Cinema
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo de nome é obrigatório.")]
        public string Nome { get; set; }
        // Proriedade para conter o ID do endereco PK para o relacionamento 1:1
        public int EnderecoId { get; set; }
        // Propriedade de navegação para o relacionamento 1:1
        public virtual Endereco Endereco { get; set; }
    }
}
