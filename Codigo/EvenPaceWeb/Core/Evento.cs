using System;
using System.Collections.Generic;

namespace Core;

public partial class Evento
{
    public uint Id { get; set; }

    public DateTime Data { get; set; }

    public int NumeroParticipantes { get; set; }

    public string Discricao { get; set; } = null!;

    public bool Distancia3 { get; set; }

    public bool Distancia5 { get; set; }

    public bool Distancia7 { get; set; }

    public bool Distancia10 { get; set; }

    public bool Distancia15 { get; set; }

    public bool Distancia21 { get; set; }

    public bool Distancia42 { get; set; }

    public string Rua { get; set; } = null!;

    public string Bairro { get; set; } = null!;

    public string Cidade { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string InfoRetiradaKit { get; set; } = null!;

    public uint IdOrganizacao { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Cupom> Cupoms { get; set; } = new List<Cupom>();

    public virtual Organizacao IdOrganizacaoNavigation { get; set; } = null!;

    public virtual ICollection<Inscricao> Inscricaos { get; set; } = new List<Inscricao>();

    public virtual ICollection<Kit> Kits { get; set; } = new List<Kit>();
}
