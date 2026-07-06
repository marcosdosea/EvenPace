using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Models
{
    public class InscricaoViewModel
    {
        [Key]
        [Display(Name = "Código da Inscrição")]
        public uint Id { get; set; }

        public string? Nome { get; set; }
        public string? Email { get; set; }

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
        [ValidateNever]
        public virtual AvaliacaoEvento IdAvaliacaoEventoNavigation { get; set; } = null!;

        [Display(Name = "Kit Retirado")]
        public bool StatusRetiradaKit { get; set; }

        [ForeignKey("IdCorredor")]
        [ValidateNever]
        public virtual Corredor IdCorredorNavigation { get; set; } = null!;

        [ForeignKey("IdEvento")]
        [ValidateNever]
        public virtual Evento IdEventoNavigation { get; set; } = null!;

        [ForeignKey("IdKit")]
        [ValidateNever]
        public virtual Kit IdKitNavigation { get; set; } = null!;

        public string NomeEvento { get; set; } = string.Empty;

        public string? ImagemEvento { get; set; }

        public string Local { get; set; } = string.Empty;

        public DateTime DataEvento { get; set; }

        public string Descricao { get; set; } = string.Empty;

        [Display(Name = "Informações de Retirada do Kit")]
        public string InfoRetiradaKit { get; set; } = string.Empty;

        public List<string> Percursos { get; set; } = new();

        public List<KitViewModel> Kits { get; set; } = new();

        public InscricaoViewModel? Inscricao { get; set; }

        public string? NomeCorredor { get; set; }

        public string? NomeKit { get; set; }
    }
}
