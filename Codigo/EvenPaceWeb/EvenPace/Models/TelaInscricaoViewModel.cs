using System.ComponentModel.DataAnnotations;
using Models;

namespace EvenPaceWeb.Models
{
    public class TelaInscricaoViewModel
    {
        public int IdEvento { get; set; }

        public int? IdKit { get; set; }

        public string NomeEvento { get; set; } = null!;

        public string ImagemEvento { get; set; }

        public string Local { get; set; } = null!;

        public DateTime DataEvento { get; set; } 

        public string Descricao { get; set; } = null!;

        [Display(Name = "Informações de Retirada do Kit")]
        public string InfoRetiradaKit { get; set; } = null!;

        public List<string> Percursos { get; set; } = null!;

        public List<KitViewModel> Kits { get; set; } = null!;

        public InscricaoViewModel Inscricao { get; set; } = null!;

        public string NomeCorredor { get; set; } = null!;

        public string NomeKit { get; set; } = null!;

    }
}
