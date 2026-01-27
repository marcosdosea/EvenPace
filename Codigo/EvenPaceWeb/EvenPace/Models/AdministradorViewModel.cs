using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class AdministradorViewModel
    {
        [Key]
        [Display(Name = "Código")]
        [Required(ErrorMessage = "O campo Id é obrigatório.")]
        public uint Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = null!;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        public string Email { get; set; } = null!;

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Senha { get; set; } = null!;
    }
}
