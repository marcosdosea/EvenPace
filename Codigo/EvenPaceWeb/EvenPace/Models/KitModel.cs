
namespace Models
{
    public class KitModel
    {
        [Key]
        [Display(Name = "Código do Kit")]
        public int Id { get; set; }

        [(ErrorMessage = "O nome do kit é obrigatório")]
        [Display(Name = "Tipo de Kit")]
        [StringLength(50, MinimumLength = 3)]
        public string Nome { get; set; } = null!;

        [(ErrorMessage = "A descrição do kit é obrigatória")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; } = null!;

        [(ErrorMessage = "O valor do kit é obrigatório")]
        [Display(Name = "Valor")]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        
        [Display(Name = "Disponibilidade P")]
        public int DisponibilidadeP { get; set; }

       
        [Display(Name = "Disponibilidade M")]
        public int DisponibilidadeM { get; set; }

     
        [Display(Name = "Disponibilidade G")]
        public int DisponibilidadeG { get; set; }

     
        [Display(Name = "Utilizado P")]
        public int UtilizadaP { get; set; }

   
        [Display(Name = "Utilizado M")]
        public int UtilizadaM { get; set; }

       
        [Display(Name = "Utilizado G")]
        public int UtilizadaG { get; set; }

       
        [Display(Name = "Evento")]
        public int IdEvento { get; set; }

       
        [Display(Name = "Status da Retirada do Kit")]
        public bool StatusRetiradaKit { get; set; }

        [Display(Name = "Data da Retirada")]
        [DataType(DataType.Date)]
        public DateTime? DataRetirada { get; set; }
    }
}
