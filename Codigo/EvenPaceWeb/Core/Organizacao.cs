using System;
using System.Collections.Generic;

namespace Core;

public partial class Organizacao
{
    public uint Id { get; set; }

    public string Cnpj { get; set; } = null!;

    public string? Cpf { get; set; }

    public int Telefone { get; set; }

    public string Cep { get; set; } = null!;

    public string Rua { get; set; } = null!;

    public string Bairro { get; set; } = null!;

    public string Cidade { get; set; } = null!;

    public int Numero { get; set; }

    public string Estado { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public bool StatusSituacao { get; set; }

    public int AdmistradorId { get; set; }

    public virtual Admistrador Admistrador { get; set; } = null!;

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();
}
