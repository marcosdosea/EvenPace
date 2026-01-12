using System;
using System.Collections.Generic;

namespace Core;

public partial class Kit
{
    public uint Id { get; set; }

    public double Valor { get; set; }

    public string Nome { get; set; } = null!;

    public string Descricao { get; set; } = null!;

    public int DisponibilidadeP { get; set; }

    public int DisponibilidadeG { get; set; }

    public int DisponibilidadeM { get; set; }

    public int UtilizadaP { get; set; }

    public int UtilizadaG { get; set; }

    public int UtilizadaM { get; set; }

    public uint IdEvento { get; set; }

    public bool StatusRetiradaKit { get; set; }

    public DateTime? DataRetirada { get; set; }

    public virtual Evento IdEventoNavigation { get; set; } = null!;

    public virtual ICollection<Inscricao> Inscricaos { get; set; } = new List<Inscricao>();
}
