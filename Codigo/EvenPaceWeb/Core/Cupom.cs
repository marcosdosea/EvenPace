using System;
using System.Collections.Generic;

namespace Core;

public partial class Cupom
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public float Desconto { get; set; }

    public bool Status { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime DataTermino { get; set; }

    public int QuantidadeUtilizada { get; set; }

    public int QuantiadeDisponibilizada { get; set; }

    public int IdEvento { get; set; }

    public virtual Evento IdEventoNavigation { get; set; } = null!;
}
