using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Models
{
    public class InscricaoModel
    {
        public int Id { get; set; }

        [Display(Name = "Status")]
        public string? Status { get; set; }

        [Display(Name = "Data da Inscrição")]
        [DataType(DataType.Date)]
        public DateTime DataInscricao { get; set; }

        [Display(Name = "Distância")]
        [Required(ErrorMessage = "Selecione a distância")]
        public string Distancia { get; set; } = null!;

        [Display(Name = "Tamanho da Camisa")]
        [Required(ErrorMessage = "Selecione o tamanho da camisa")]
        public string TamanhoCamisa { get; set; } = null!;

        [Display(Name = "Tempo")]
        public TimeSpan? Tempo { get; set; }

        [Display(Name = "Posição")]
        public int? Posicao { get; set; }

        [Display(Name = "Kit")]
        [Required(ErrorMessage = "Selecione o kit")]
        public int IdKit { get; set; }

        [Display(Name = "Evento")]
        [Required(ErrorMessage = "Selecione o evento")]
        public int IdEvento { get; set; }

        public int IdCorredor { get; set; }

        public int? IdAvaliacaoEvento { get; set; }

  
    }
}
