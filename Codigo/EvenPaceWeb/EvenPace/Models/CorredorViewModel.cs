using System;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.Rendering;

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
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter apenas números")]
        public string CPF { get; set; } = null!;

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O nome do corredor é obrigatório")]
        [StringLength(45, MinimumLength = 3, ErrorMessage = "O nome deve ter até 45 caracteres")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "O nome deve conter apenas letras e espaços")]
        public string Nome { get; set; } = null!;

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [StringLength(45, ErrorMessage = "O e-mail deve ter até 45 caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "E-mail inválido")]
        public string Email { get; set; } = null!;

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; } 

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        [StringLength(45, MinimumLength = 6, ErrorMessage = "A senha deve ter  até 45 caracteres")]
        public string Senha { get; set; } = null!;
    }
}
