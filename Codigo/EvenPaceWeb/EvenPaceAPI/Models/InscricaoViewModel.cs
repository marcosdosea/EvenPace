    using System.ComponentModel.DataAnnotations;

    namespace EvenPaceAPI.Models
{
        public class InscricaoViewModel
        {
            [Key]
        public int Id { get; set; }

        [Required]
        public int IdEvento { get; set; }

        [Required]
        public int IdCorredor { get; set; }

        public int? IdKit { get; set; }

        [Required]
        public DateTime DataInscricao { get; set; }

        [Required]
        public string Status { get; set; } = null!;

        [Required]
        public string Distancia { get; set; } = null!;

        [Required]
        public string TamanhoCamisa { get; set; } = null!;

        public bool StatusRetiradaKit { get; set; }

        public TimeSpan? Tempo { get; set; }

        public int? Posicao { get; set; }

        public int? IdAvaliacaoEvento { get; set; }
    }
    }