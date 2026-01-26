using System;
using System.Collections.Generic;

namespace Models
{
    public class TelaInscricaoViewModel
    {
        public int IdEvento { get; set; }
        public string NomeEvento { get; set; } = null!;
        public string? ImagemEvento { get; set; }
        public string Local { get; set; } = null!;
        public DateTime DataEvento { get; set; }
        public string Descricao { get; set; } = null!;

        public List<string> Percursos { get; set; } = new();
        public List<KitViewModel> Kits { get; set; } = new();

        public InscricaoViewModel Inscricao { get; set; } = new();
    }
}
