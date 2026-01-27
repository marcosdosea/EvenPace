using System.ComponentModel.DataAnnotations;


namespace Models
{
    public class KitViewModel
    {
        [Key]
        [Display(Name = "Código do Kit")]
        public uint Id { get; set; }

        [Required(ErrorMessage = "O nome do kit é obrigatório")]
        [Display(Name = "Tipo de Kit")]
        [StringLength(50, MinimumLength = 3)]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "A descrição do kit é obrigatória")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; } = null!;

        [Required(ErrorMessage = "O valor do kit é obrigatório")]
        [Display(Name = "Valor")]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        [Required]
        [Display(Name = "Disponibilidade P")]
        public int DisponibilidadeP { get; set; }

        [Required]
        [Display(Name = "Disponibilidade M")]
        public int DisponibilidadeM { get; set; }

        [Required]
        [Display(Name = "Disponibilidade G")]
        public int DisponibilidadeG { get; set; }

        [Required]
        [Display(Name = "Utilizado P")]
        public int UtilizadaP { get; set; }

        [Required]
        [Display(Name = "Utilizado M")]
        public int UtilizadaM { get; set; }

        [Required]
        [Display(Name = "Utilizado G")]
        public int UtilizadaG { get; set; }

        [Required]
        [Display(Name = "Evento")]
        public int IdEvento { get; set; }

        [Required]
        [Display(Name = "Status da Retirada do Kit")]
        public bool StatusRetiradaKit { get; set; }

        [Display(Name = "Data da Retirada")]
        [DataType(DataType.Date)]
        public DateTime? DataRetirada { get; set; }

        // Propriedade para RECEBER o arquivo do formulário
        public IFormFile? ImagemUpload { get; set; }

        // Propriedade para SALVAR o nome do arquivo no banco (string)
        public string? Imagem { get; set; }
    }
}
