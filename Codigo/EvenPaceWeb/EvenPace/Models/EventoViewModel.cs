using System.ComponentModel.DataAnnotations;

namespace EvenPaceWeb.Models
{
    public class EventoViewModel
    {
        [Key]
        [Display(Name = "Código do Evento")]
        [Required(ErrorMessage = "Código do evento é obrigatório.")]
        public uint Id { get; set; }

        [Required(ErrorMessage = "O nome do evento é obrigatório.")]
        [Display(Name = "Nome do Evento")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "A data é obrigatória.")]
        [Display(Name = "Data do Evento")]
        [DataType(DataType.DateTime)]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "O número de participantes é obrigatório.")]
        [Display(Name = "Nº de Participantes")]
        [Range(1, int.MaxValue, ErrorMessage = "O número deve ser maior que zero.")]
        public int NumeroParticipantes { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [Display(Name = "Descrição")]
        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; } = null!;

        // --- Distâncias (Checkboxes) ---

        [Display(Name = "3 km")]
        public bool Distancia3 { get; set; }

        [Display(Name = "5 km")]
        public bool Distancia5 { get; set; }

        [Display(Name = "7 km")]
        public bool Distancia7 { get; set; }

        [Display(Name = "10 km")]
        public bool Distancia10 { get; set; }

        [Display(Name = "15 km")]
        public bool Distancia15 { get; set; }

        [Display(Name = "21 km")]
        public bool Distancia21 { get; set; }

        [Display(Name = "42 km")]
        public bool Distancia42 { get; set; }

        // --- Endereço ---

        [Required(ErrorMessage = "A rua é obrigatória.")]
        public string Rua { get; set; } = null!;

        [Required(ErrorMessage = "O bairro é obrigatório.")]
        public string Bairro { get; set; } = null!;

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string Cidade { get; set; } = null!;

        [Required(ErrorMessage = "O estado é obrigatório.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Use a sigla do estado (ex: SE).")]
        public string Estado { get; set; } = null!;

        [Display(Name = "Informações de Retirada do Kit")]
        [DataType(DataType.MultilineText)]
        public string InfoRetiradaKit { get; set; } = null!;

        // --- Relacionamento (Organização) ---

        [Required(ErrorMessage = "Selecione a organização responsável.")]
        [Display(Name = "Organização")]
        public uint IdOrganizacao { get; set; }
    }
}