using System;
using System.Collections.Generic;

namespace Core
{
    public class Inscricao
    {
        public int Id { get; set; }

        public string Status { get; set; } = null!;

        public DateTime DataInscricao { get; set; }

        public string Distancia { get; set; } = null!;

        public string TamanhoCamisa { get; set; } = null!;

        public TimeSpan? Tempo { get; set; }
        public int? Posicao { get; set; }

        public int? IdAvaliacaoEvento { get; set; }

        public int? IdKit { get; set; }

        public int IdEvento { get; set; }
        public int IdCorredor { get; set; }

        public AvaliacaoEvento? IdAvaliacaoEventoNavigation { get; set; }
        public Evento IdEventoNavigation { get; set; } = null!;
        public Corredor IdCorredorNavigation { get; set; } = null!;
        public Kit? IdKitNavigation { get; set; }
    }
}
