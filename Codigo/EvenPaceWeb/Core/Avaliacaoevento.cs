using System;
using System.Collections.Generic;

namespace Core;

public partial class Avaliacaoevento
{
    public uint Id { get; set; }

    public string? Comentario { get; set; }

    public int Estrela { get; set; }

    public virtual ICollection<Inscricao> Inscricaos { get; set; } = new List<Inscricao>();
}
