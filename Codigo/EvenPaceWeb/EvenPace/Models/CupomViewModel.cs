using System.ComponentModel.DataAnnotations;

namespace EvenPaceWeb.Models
{
    public class CupomViewModel
    {
        [Key]
        [Display(Name = "Código do Cupom")]
        [Required(ErrorMessage = "Código do cupom é obrigatório.")]
        public uint Id { get; set; }

        [Required(ErrorMessage = "O código/nome do cupom é obrigatório.")]
        [Display(Name = "Código do Cupom")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O valor do desconto é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O desconto deve ser maior que zero.")]
        [Display(Name = "Valor do Desconto")]
        public int Desconto { get; set; }

        [Display(Name = "Cupom Ativo")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        [Display(Name = "Data de Início")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de término é obrigatória.")]
        [Display(Name = "Data de Término")]
        [DataType(DataType.Date)]
        public DateTime DataTermino { get; set; }

        
        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Display(Name = "Quantidade Disponível")]
        [Range(1, int.MaxValue, ErrorMessage = "Informe uma quantidade válida.")]
        public int QuantiadeDisponibilizada { get; set; }

        [Display(Name = "Já Utilizados")]
        public int QuantidadeUtilizada { get; set; }

        // --- Relacionamento com Evento ---

        [Required(ErrorMessage = "O ID do evento é obrigatório.")]
        [Display(Name = "ID do Evento")]
        public uint IdEvento { get; set; }
    }
}
