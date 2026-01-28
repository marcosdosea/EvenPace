using System;
using System.Collections.Generic;

namespace Core;

public partial class CartaoCredito
{
    public uint Id { get; set; }

    public string Numero { get; set; } = null!;

    public DateTime DataValidade { get; set; }

    public int CodeSeguranca { get; set; }

    public string Nome { get; set; } = null!;

    public uint IdCorredor { get; set; }

    public virtual Corredor IdCorredorNavigation { get; set; } = null!;
}
