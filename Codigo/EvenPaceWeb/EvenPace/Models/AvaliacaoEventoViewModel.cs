using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class AvaliacaoEventoViewModel
    {
        [Key]
        [Display(Name = "Código")]
        [Required(ErrorMessage = "O campo Id é obrigatório.")]
        public int Id { get; set; }

        [Display(Name = "Estrela")]
        [Required(ErrorMessage = "A avaliação é obrigatória.")]
        [Range(1, 5)]
        public int Estrela { get; set; }

        [Display(Name = "Comentário")]
        [StringLength(300)]
        public string Comentario { get; set; } = null!;

        public DateTime DataAvaliacao { get; set; }



    }
}
