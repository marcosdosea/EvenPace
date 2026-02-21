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
        public uint Id { get; set; }

        [Display(Name = "Status")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "A data da inscrição é obrigatória")]
        [Display(Name = "Data da Inscrição")]
        [DataType(DataType.Date)]
        public DateTime DataInscricao { get; set; }

        [Required(ErrorMessage = "Selecione a distância")]
        [Display(Name = "Distância")]
        public string Distancia { get; set; } = null!;

        [Required(ErrorMessage = "Selecione o tamanho da camisa")]
        [Display(Name = "Tamanho da Camisa")]
        public string TamanhoCamisa { get; set; } = null!;

        [Display(Name = "Tempo")]
        public TimeSpan? Tempo { get; set; }

        [Display(Name = "Posição")]
        public int? Posicao { get; set; }

        [Required(ErrorMessage = "Selecione o kit")]
        [Display(Name = "Kit")]
        public int IdKit { get; set; }

        [Required(ErrorMessage = "Selecione o evento")]
        [Display(Name = "Evento")]
        public int IdEvento { get; set; }

        [Required(ErrorMessage = "O corredor é obrigatório")]
        public int IdCorredor { get; set; }

        public int? IdAvaliacaoEvento { get; set; }
        
        [ForeignKey("IdAvaliacaoEvento")]
        public virtual AvaliacaoEvento IdAvaliacaoEventoNavigation { get; set; } = null!;

        [ForeignKey("IdCorredor")]
        public virtual Corredor IdCorredorNavigation { get; set; } = null!;

        [ForeignKey("IdEvento")]
        public virtual Evento IdEventoNavigation { get; set; } = null!;

        [ForeignKey("IdKit")]
        public virtual Kit IdKitNavigation { get; set; } = null!;

        public string NomeEvento { get; set; }

        public string ImagemEvento { get; set; }

        public string Local { get; set; }

        public DateTime DataEvento { get; set; }

        public string Descricao { get; set; }

        [Display(Name = "Informações de Retirada do Kit")]
        public string InfoRetiradaKit { get; set; } = null!;

        public List<string> Percursos { get; set; }

        public List<KitViewModel> Kits { get; set; }

        public InscricaoViewModel Inscricao { get; set; }

        public string NomeCorredor { get; set; }

        public string NomeKit { get; set; }
    }
}
 
