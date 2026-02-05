using System.ComponentModel.DataAnnotations;

namespace Models;

public class OrganizacaoViewModel
{
    [Display(Name = "Código")]
    [Key]
    public int Id { get; set; }

    [Display(Name = "Nome Completo")]
    [Required(ErrorMessage = "Nome da Organização é obrigatório")]
    public string? Nome { get; set; }

    [Display(Name = "CNPJ")]
    [StringLength(18, ErrorMessage = "O CNPJ deve conter até 18 caracteres")]
    public string? Cnpj { get; set; }

    [Display(Name = "CPF")]
    [StringLength(14, ErrorMessage = "O CPF deve conter até 14 caracteres")]
    public string? Cpf { get; set; }

    [Display(Name = "Telefone")]
    [Required(ErrorMessage = "Telefone é Obrigatório")]
    public string Telefone { get; set; } = null!;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Senha é Obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve conter pelo menos 6 caracteres")]
    [StringLength(45, ErrorMessage = "A senha deve conter no máximo 45 caracteres")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = null!;

    [Display(Name = "CEP")]
    [Required(ErrorMessage = "O CEP é Obrigatório")]
    [StringLength(9, ErrorMessage = "O CEP deve conter até 9 caracteres")]
    public string Cep { get; set; } = null!;

    [Required(ErrorMessage = "Rua é obrigatória")]
    [StringLength(45)]
    public string Rua { get; set; } = null!;

    [Required(ErrorMessage = "Bairro é obrigatório")]
    [StringLength(45)]
    public string Bairro { get; set; } = null!;

    [Required(ErrorMessage = "Cidade é obrigatória")]
    [StringLength(45)]
    public string Cidade { get; set; } = null!;

    [Required(ErrorMessage = "Número é obrigatório")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "Estado é obrigatório")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "Use a sigla do estado (ex: SE)")]
    public string Estado { get; set; } = null!;
    public bool StatusSituacao { get; set; }
    public int? AdmistradorId { get; set; }
}