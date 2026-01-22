using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Models
{
    public class KitModel
    {
        [Key]
        [Display(Name = "Código do Kit")]
        public uint Id { get; set; }

        [Required(ErrorMessage = "O nome do kit é obrigatório")]
        [Display(Name = "Tipo de Kit")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome do kit deve ter entre 3 e 50 caracteres")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O valor do kit é obrigatório")]
        [Display(Name = "Valor")]
        [DataType(DataType.Currency, ErrorMessage = "Valor inválido")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Selecione o tamanho da camisa")]
        [Display(Name = "Tamanho da Camisa")]
        [StringLength(5, ErrorMessage = "Tamanho inválido")]
        public string TamanhoCamisa { get; set; } = null!;

        // Propriedade auxiliar para dropdown de tamanhos, se necessário
        public SelectList? ListaTamanhos { get; set; }
    }
}
