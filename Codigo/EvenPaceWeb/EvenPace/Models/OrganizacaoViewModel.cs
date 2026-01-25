using System.ComponentModel.DataAnnotations;

namespace EvenPace.Models;

public class OrganizacaoViewModel
{
    [Display(Name = "Código")]
    [Key]
    [Required(ErrorMessage = "Código da Organização é obrigatorio")]
    public int Id { get; set; }
    
    [Display(Name = "CNPJ")]
    [Required(ErrorMessage = "Campo Requirido")]
    [StringLength(14, ErrorMessage = "O CNPJ deve conter 14 número")]
    public string? Cnpj { get; set; }
    
    [Display(Name = "CPF")]
    [Required(ErrorMessage = "Campo Requirido")]
    [StringLength(11, ErrorMessage = "O CNPJ deve conter 14 número")]
    public string? Cpf { get; set; }

    [Required(ErrorMessage = "Telefone é Obrigatorio")]
    [Phone(ErrorMessage = "Telefone invalido")]
    [DataType(DataType.PhoneNumber)]
    [StringLength(14, ErrorMessage = "O Telefone deve conter no maximo 14 número")]
    public string Telefone { get; set; } = null!;
    
    [Display(Name = "CEP")]
    [Required(ErrorMessage = "O CEP é Obrigatorio")]
    [StringLength(8, ErrorMessage = "O CEP deve conter 8 números")]
    public string Cep { get; set; } = null!;

    [Required]
    [StringLength(45)]
    public string Rua { get; set; } = null!;

    [Required]
    [StringLength(45)]
    public string Bairro { get; set; } = null!;

    [Required]
    [StringLength(45)]
    public string Cidade { get; set; } = null!;

    [Required]
    public int Numero { get; set; }

    [Required]
    [StringLength(2)]
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