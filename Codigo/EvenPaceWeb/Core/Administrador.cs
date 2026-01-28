using System;
using System.Collections.Generic;

namespace Core;

public partial class Administrador
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public virtual ICollection<Organizacao> Organizacaos { get; set; } = new List<Organizacao>();
}
