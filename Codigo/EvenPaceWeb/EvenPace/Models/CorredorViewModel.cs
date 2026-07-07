using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Models;

public class CorredorViewModel
{
    public int Id { get; set; }

    [Display(Name = "Foto de Perfil")]
    public IFormFile? FotoPerfilUpload { get; set; }

    public string? FotoPerfil { get; set; }

    [Display(Name = "CPF")]
    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [StringLength(11, MinimumLength = 11,
        ErrorMessage = "O CPF deve conter 11 números.")]
    public string CPF { get; set; } = null!;

    [Display(Name = "Nome")]
    [Required(ErrorMessage = "O nome do corredor é obrigatório.")]
    [StringLength(45, MinimumLength = 3,
        ErrorMessage = "O nome deve ter entre 3 e 45 caracteres.")]
    public string Nome { get; set; } = null!;

    [Display(Name = "Data de Nascimento")]
    [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
    [DataType(DataType.Date)]
    public DateTime DataNascimento { get; set; }

    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Digite um e-mail válido.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Senha")]
    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6,
        ErrorMessage = "A senha deve conter pelo menos 6 caracteres.")]
    public string Senha { get; set; } = null!;
}