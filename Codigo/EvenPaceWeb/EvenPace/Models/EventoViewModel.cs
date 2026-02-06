using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

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

        // --- CAMPOS PARA A VIEW (HTML5) ---
        [Required(ErrorMessage = "A data é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime? DataOnly { get; set; } // Vinculado ao input type="date"

        [Required(ErrorMessage = "O horário é obrigatório.")]
        [DataType(DataType.Time)]
        public TimeSpan? HoraOnly { get; set; } // Vinculado ao input type="time"

        // --- CAMPO DO BANCO (Combinado) ---
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "O número de participantes é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mínimo de 1 participante.")]
        public int NumeroParticipantes { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; } = null!;

        // --- DISTÂNCIAS ---
        public bool Distancia3 { get; set; }
        public bool Distancia5 { get; set; }
        public bool Distancia7 { get; set; }
        public bool Distancia10 { get; set; }
        public bool Distancia15 { get; set; }
        public bool Distancia21 { get; set; }
        public bool Distancia42 { get; set; }

        // --- ENDEREÇO ---
        [Required] public string Rua { get; set; } = null!;
        [Required] public string Bairro { get; set; } = null!;
        [Required] public string Cidade { get; set; } = null!;
        [Required] public string Estado { get; set; } = null!;

        [Display(Name = "Informações de Retirada")]
        public string InfoRetiradaKit { get; set; } = null!;

        // --- UPLOADS ---
        public string? Imagem { get; set; } // Nome do arquivo no banco
        [Display(Name = "Imagem do Evento")]
        public IFormFile? ImagemUpload { get; set; } // Arquivo vindo da tela

        public uint IdOrganizacao { get; set; }
    }
}