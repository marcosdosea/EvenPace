using Models;

namespace EvenPaceWeb.Models
{
        public class TelaInscricaoViewModel
        {
           public uint IdEvento { get; set; }
           public string NomeEvento { get; set; }
            public string ImagemEvento { get; set; }
            public string Local { get; set; }
            public DateTime DataEvento { get; set; }
            public string Descricao { get; set; }
            public List<string> Percursos { get; set; }
            public List<KitViewModel> Kits { get; set; }
            public InscricaoViewModel Inscricao { get; set; }
        }
    }

