using System;
using System.Collections.Generic;

namespace Core;

public partial class Inscricao
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public float? DistanciaPercorida { get; set; }

    public DateTime DataInscricao { get; set; }

    public TimeSpan Tempo { get; set; }

    public int Posicao { get; set; }

    public int IdKit { get; set; }

    public int IdEvento { get; set; }

    public int IdCorredor { get; set; }

    public int IdAvaliacaoEvento { get; set; }

    public virtual AvaliacaoEvento IdAvaliacaoEventoNavigation { get; set; } = null!;

    public virtual Corredor IdCorredorNavigation { get; set; } = null!;

    public virtual Evento IdEventoNavigation { get; set; } = null!;

    public virtual Kit IdKitNavigation { get; set; } = null!;
}
