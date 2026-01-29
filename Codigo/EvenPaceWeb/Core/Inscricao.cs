using System;
using System.Collections.Generic;

namespace Core;

public partial class Inscricao
{
    public uint Id { get; set; }

    public string Status { get; set; } = null!;

    public DateTime DataInscricao { get; set; }

    public string Distancia { get; set; } = null!;

    public string TamanhoCamisa { get; set; } = null!;

    public TimeSpan Tempo { get; set; }

    public int Posicao { get; set; }

    public uint IdKit { get; set; }

    public uint IdEvento { get; set; }

    public uint IdCorredor { get; set; }

    public uint IdAvaliacaoEvento { get; set; }

    public virtual AvaliacaoEvento IdAvaliacaoEventoNavigation { get; set; } = null!;

    public virtual Corredor IdCorredorNavigation { get; set; } = null!;

    public virtual Evento IdEventoNavigation { get; set; } = null!;

    public virtual Kit IdKitNavigation { get; set; } = null!;
}
