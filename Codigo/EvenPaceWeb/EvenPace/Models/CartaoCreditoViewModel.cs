using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class CartaoCreditoViewModel
    {
        [Key]
        [Display(Name = "Código")]
        [Required(ErrorMessage = "O campo Id é obrigatório.")]
        public uint Id { get; set; }

        [Display(Name = "Corredor")]
        [Required(ErrorMessage = "O campo IdCorredor é obrigatório.")]
        public uint IdCorredor { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = null!;

        [Display(Name = "Número")]
        [Required(ErrorMessage = "O número é obrigatório.")]
        [StringLength(16)]
        public string Numero { get; set; } = null!;

        [Display(Name = "Data de Validade")]
        [Required(ErrorMessage = "A data de validade é obrigatória.")]
        public DateTime DataValidade { get; set; }

        [Display(Name = "Código de Segurança")]
        [Required(ErrorMessage = "O código de segurança é obrigatório.")]
        [StringLength(3)]
        public string CodigoSeguranca { get; set; } = null!;
    }
}
