using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class InscricaoModel
    {
        [Key]
        [Display(Name = "Código da Inscrição")]
        public int Id { get; set; }

        [Display(Name = "Status")]
        public string? Status { get; set; }

        [(ErrorMessage = "A data da inscrição é obrigatória")]
        [Display(Name = "Data da Inscrição")]
        [DataType(DataType.Date)]
        public DateTime DataInscricao { get; set; }

        [(ErrorMessage = "Selecione a distância")]
        [Display(Name = "Distância")]
        public string Distancia { get; set; } = null!;

        [(ErrorMessage = "Selecione o tamanho da camisa")]
        [Display(Name = "Tamanho da Camisa")]
        public string TamanhoCamisa { get; set; } = null!;

        [Display(Name = "Tempo")]
        public TimeSpan? Tempo { get; set; }

        [Display(Name = "Posição")]
        public int? Posicao { get; set; }

        [(ErrorMessage = "Selecione o kit")]
        [Display(Name = "Kit")]
        public int IdKit { get; set; }

        [(ErrorMessage = "Selecione o evento")]
        [Display(Name = "Evento")]
        public int IdEvento { get; set; }

        [(ErrorMessage = "O corredor é obrigatório")]
        public int IdCorredor { get; set; }

        public int? IdAvaliacaoEvento { get; set; }
    }
}
