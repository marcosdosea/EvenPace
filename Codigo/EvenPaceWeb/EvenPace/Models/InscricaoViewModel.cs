using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Models
{
    public class InscricaoViewModel
    {
        [Key]
        [Display(Name = "Código da Inscrição")]
        public int Id { get; set; }

        [Display(Name = "Status")]
        public string? Status { get; set; }

        [Display(Name = "Distância Percorida")]
        [Range(0, float.MaxValue, ErrorMessage = "A distância percorrida deve ser maior que 0")]
        [DataType(DataType.Currency)]
        public float? DistanciaPercorida { get; set; }

        [Required(ErrorMessage = "A data da inscrição é obrigatória")]
        [Display(Name = "Data da Inscrição")]
        [DataType(DataType.Date)]
        public DateTime DataInscricao { get; set; }

        [Display(Name = "Tempo")]
        [DataType(DataType.Time)]
        public TimeSpan Tempo { get; set; }

        [Display(Name = "Posição")]
        [Range(1, int.MaxValue, ErrorMessage = "A posição deve ser maior que 0")]
        public int Posicao { get; set; }

        [Required(ErrorMessage = "Selecione o kit")]
        [Display(Name = "Kit")]
        public int IdKit { get; set; }

        [Required(ErrorMessage = "Selecione o evento")]
        [Display(Name = "Evento")]
        public int IdEvento { get; set; }

        [Required(ErrorMessage = "O corredor é obrigatório")]
        [Display(Name = "Corredor")]
        public int IdCorredor { get; set; }

        [Required(ErrorMessage = "Selecione a avaliação do evento")]
        [Display(Name = "Avaliação do Evento")]
        public int IdAvaliacaoEvento { get; set; }
        
        [ForeignKey("IdAvaliacaoEvento")]
        public virtual AvaliacaoEvento IdAvaliacaoEventoNavigation { get; set; } = null!;

        [ForeignKey("IdCorredor")]
        public virtual Corredor IdCorredorNavigation { get; set; } = null!;

        [ForeignKey("IdEvento")]
        public virtual Evento IdEventoNavigation { get; set; } = null!;

        [ForeignKey("IdKit")]
        public virtual Kit IdKitNavigation { get; set; } = null!;
    }
}
 
