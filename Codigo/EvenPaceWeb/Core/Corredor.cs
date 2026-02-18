using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core;

public partial class Corredor
{
    public int Id { get; set; }

    public string Cpf { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime DataNascimento { get; set; }

    public string Senha { get; set; } = null!;

    [Column("imagem")]
    public string? Imagem { get; set; }

    public virtual CartaoCredito? CartaoCredito { get; set; }

    public virtual ICollection<Inscricao> Inscricaos { get; set; } = new List<Inscricao>();
}
