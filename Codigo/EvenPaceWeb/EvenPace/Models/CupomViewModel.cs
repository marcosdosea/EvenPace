using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EvenPaceWeb.Models
{
    public class CupomViewModel
    {
        public uint Id { get; set; }

        [Required(ErrorMessage = "O código/nome do cupom é obrigatório.")]
        [Display(Name = "Código do Cupom")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O valor do desconto é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O desconto deve ser maior que zero.")]
        [Display(Name = "Valor do Desconto")]
        public int Desconto { get; set; }

        [Display(Name = "Cupom Ativo")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        [Display(Name = "Data de Início")]
        [DataType(DataType.Date)] // Gera um calendário na tela
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de término é obrigatória.")]
        [Display(Name = "Data de Término")]
        [DataType(DataType.Date)]
        public DateTime DataTermino { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Display(Name = "Quantidade Disponível")]
        [Range(1, int.MaxValue, ErrorMessage = "Informe uma quantidade válida.")]
        public int QuantiadeDisponibilizada { get; set; }

        // Campo apenas para leitura (quantos já usaram)
        [Display(Name = "Já Utilizados")]
        public int QuantidadeUtilizada { get; set; }

        // --- Relacionamento com Evento ---

        [Required(ErrorMessage = "Selecione o evento associado.")]
        [Display(Name = "Evento")]
        public uint IdEvento { get; set; }

        // Lista para preencher o Dropdown (<select>) na View
        public IEnumerable<SelectListItem>? ListaEventos { get; set; }
    }
}
