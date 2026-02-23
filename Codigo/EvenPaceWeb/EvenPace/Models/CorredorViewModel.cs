using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class CorredorViewModel
    {
        [Key]
        [Display(Name = "Código do Corredor")]
        public int Id { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve conter 11 caracteres")]
        public string CPF { get; set; } = null!;

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O nome do corredor é obrigatório")]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "O nome deve ter até 45 caracteres")]
        public string Nome { get; set; } = null!;

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

    }
}
