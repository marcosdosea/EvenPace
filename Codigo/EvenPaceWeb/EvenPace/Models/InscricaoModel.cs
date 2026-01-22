using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class InscricaoModel
    {
        [Display(Name = "Evento")]
        [Required(ErrorMessage = "Selecione o evento")]
        public uint IdEvento { get; set; }

        [Display(Name = "Distância")]
        [Required(ErrorMessage = "Selecione a distância")]
        public string Distancia { get; set; } = null!;

        [Display(Name = "Kit")]
        [Required(ErrorMessage = "Selecione o kit")]
        public uint IdKit { get; set; }

        [Display(Name = "Tamanho da Camisa")]
        [Required(ErrorMessage = "Selecione o tamanho da camisa")]
        public string TamanhoCamisa { get; set; } = null!;

        public SelectList? ListaEventos { get; set; }
        public SelectList? ListaKits { get; set; }
    }



}

