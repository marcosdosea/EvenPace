using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models;

namespace EvenPaceWeb.Models
{
    public class TelaListaEventosViewModel
    {

      
            public int Id { get; set; }
            public string Nome { get; set; }
            public DateTime Data { get; set; }
            public string Cidade { get; set; }
            public string Estado { get; set; }
            public string Imagem { get; set; }
        }

    
}
