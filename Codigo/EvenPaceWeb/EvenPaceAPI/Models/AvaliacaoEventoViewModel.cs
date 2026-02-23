using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class AvaliacaoViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InscricaoId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "A avaliação deve ser entre 1 e 5.")]
        public int Estrela { get; set; }

        [StringLength(300)]
        public string? Comentario { get; set; }
    }
}
