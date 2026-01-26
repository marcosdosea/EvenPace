using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class CartaoCreditoViewViewModel
    {
        [Display(Name = "Nome do Titular")]
        [Required(ErrorMessage = "O nome do titular é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string NomeTitular { get; set; } = null!;

        [Display(Name = "Número do Cartão")]
        [Required(ErrorMessage = "O número do cartão é obrigatório.")]
        [StringLength(16, ErrorMessage = "O número do cartão deve conter 16 dígitos.")]
        public float Numero { get; set; }

        [Display(Name = "Validade")]
        [Required(ErrorMessage = "A validade é obrigatória.")]
        [StringLength(7, ErrorMessage = "Formato esperado: MM/AAAA.")]
        public string Validade { get; set; } = null!;
    }
}
