using System;
using System.Collections.Generic;

namespace Core;

public partial class Corredor
{
    public uint Id { get; set; }

    public string Cpf { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime DataNascimento { get; set; }

    public string Senha { get; set; } = null!;

    public string? Imagem { get; set; }

    public virtual CartaoCredito? CartaoCredito { get; set; }

    public virtual ICollection<Inscricao> Inscricaos { get; set; } = new List<Inscricao>();
}
