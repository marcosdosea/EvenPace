using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class EventoViewModel
    {
        [Key]
        [Display(Name = "Código")]
        public uint Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [Display(Name = "Nome do Evento")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "A data é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime? DataOnly { get; set; }

        [Required(ErrorMessage = "O horário é obrigatório.")]
        [DataType(DataType.Time)]
        public TimeSpan? HoraOnly { get; set; } 

        public DateTime Data { get; set; }

        [Required(ErrorMessage = "O número de participantes é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mínimo de 1 participante.")]
        public int NumeroParticipantes { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; } = null!;

        public bool Distancia3 { get; set; }
        public bool Distancia5 { get; set; }
        public bool Distancia7 { get; set; }
        public bool Distancia10 { get; set; }
        public bool Distancia15 { get; set; }
        public bool Distancia21 { get; set; }
        public bool Distancia42 { get; set; }

        [Required] public string Rua { get; set; } = null!;
        [Required] public string Bairro { get; set; } = null!;
        [Required] public string Cidade { get; set; } = null!;
        [Required] public string Estado { get; set; } = null!;

        [Display(Name = "Informações de Retirada")]
        public string InfoRetiradaKit { get; set; } = null!;

        public string? Imagem { get; set; } 
        [Display(Name = "Imagem do Evento")]
        public IFormFile? ImagemUpload { get; set; } 

        public uint IdOrganizacao { get; set; }
    }
}