using System.ComponentModel.DataAnnotations;

namespace EvenPaceWeb.Models
{
    public class AvaliacaoEventoViewModel
    {
        [Display(Name = "Avaliação do Evento")]
        [Required(ErrorMessage = "A avaliação é obrigatória.")]
        [Range(1, 5, ErrorMessage = "A avaliação deve variar entre 1 e 5 estrelas.")]
        public int Estrela { get; set; }

        [Display(Name = "Comentário")]
        [StringLength(300, ErrorMessage = "O comentário não pode exceder 300 caracteres.")]
        public string Comentario { get; set; } = null!;

        [DataType(DataType.Date)]
        [Display(Name = "Data da Avaliação")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataAvaliacao { get; set; }
    }
}
