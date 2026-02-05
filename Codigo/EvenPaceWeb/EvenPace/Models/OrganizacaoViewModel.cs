using System.ComponentModel.DataAnnotations;

namespace Models;

public class OrganizacaoViewModel
{
    [Display(Name = "Código")]
    [Key]
    [Required(ErrorMessage = "Código da Organização é obrigatorio")]
    public int Id { get; set; }

    [Display(Name = "Nome Completo")]
    [Required(ErrorMessage = "Nome da Organização é obrigatório")]
    public string? Nome { get; set; }

    [Display(Name = "CNPJ")]
    [StringLength(18, ErrorMessage = "O CNPJ deve conter até 18 caracteres")]
    public string? Cnpj { get; set; }

    [Display(Name = "CPF")]
    [Required(ErrorMessage = "Campo Requirido")]
    [StringLength(11, ErrorMessage = "O CNPJ deve conter 11 número")]
    public string? Cpf { get; set; }

    [Required(ErrorMessage = "Telefone é Obrigatorio")]
    [Phone(ErrorMessage = "Telefone invalido")]
    [DataType(DataType.PhoneNumber)]
    [StringLength(14, ErrorMessage = "O Telefone deve conter no maximo 14 número")]
    public string Telefone { get; set; } = null!;
    
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

    [Required(ErrorMessage = "Email é obrigatorio")]
    [EmailAddress(ErrorMessage = "Email invalido")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Senha é Obrigatoria")]
    [MinLength(6, ErrorMessage = "A senha deve pelo menos conter 6 caracteres")]
    [StringLength(45, ErrorMessage =  "A senha deve conter no máximo 45 caracteres")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = null!;
    
    public bool StatusSituacao { get; set; }
    public int? AdmistradorId { get; set; }
}