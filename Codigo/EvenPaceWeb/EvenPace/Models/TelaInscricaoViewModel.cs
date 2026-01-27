using System;
using System.Collections.Generic;

namespace EvenPace.Models
{
    public class TelaInscricaoViewModel
    {
        
        public int IdEvento { get; set; }
        public string NomeEvento { get; set; } = string.Empty;
        public string? ImagemEvento { get; set; }
        public string Local { get; set; } = string.Empty;
        public DateTime DataEvento { get; set; }
        public string? Descricao { get; set; }

        
        public List<string> Percursos { get; set; } = new();
        public List<KitViewModel> Kits { get; set; } = new();

        
        public InscricaoViewModel Inscricao { get; set; } = new();
    }
}
