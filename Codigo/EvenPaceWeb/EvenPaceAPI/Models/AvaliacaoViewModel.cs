using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class AvaliacaoViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EventoId { get; set; }

        [Required]
        public int CorredorId { get; set; }

        [Required(ErrorMessage = "A avaliação é obrigatória.")]
        [Range(1, 5, ErrorMessage = "A avaliação deve ser entre 1 e 5.")]
        public int Nota { get; set; }

        [StringLength(300)]
        public string? Comentario { get; set; }
    }
}
