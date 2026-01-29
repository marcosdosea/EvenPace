using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core;

[Table("evento")] // Use "evento" em minúsculo se o nome da tabela no MySQL também for assim
public partial class Evento
{
    [Key]
    [Column("id")] // Garante que o ID mapeie para a coluna 'id' minúscula
    public uint Id { get; set; }

    [Column("data")]
    public DateTime Data { get; set; }


    //[Column("imagem")]
    //public string? Imagem { get; set; }

    [Column("numeroParticipantes")]
    public int NumeroParticipantes { get; set; }

    [Column("discricao")] // Conforme sua imagem do banco
    public string Descricao { get; set; } = null!;

    // Mapeamento das distâncias com 'd' minúsculo
    [Column("distancia3")]
    public bool Distancia3 { get; set; }

    [Column("distancia5")]
    public bool Distancia5 { get; set; }

    [Column("distancia7")]
    public bool Distancia7 { get; set; }

    [Column("distancia10")]
    public bool Distancia10 { get; set; }

    [Column("distancia15")]
    public bool Distancia15 { get; set; }

    [Column("distancia21")]
    public bool Distancia21 { get; set; }

    [Column("distancia42")]
    public bool Distancia42 { get; set; }

    [Column("rua")]
    public string Rua { get; set; } = null!;

    [Column("bairro")]
    public string Bairro { get; set; } = null!;

    [Column("cidade")]
    public string Cidade { get; set; } = null!;

    [Column("estado")]
    public string Estado { get; set; } = null!;

    [Column("infoRetiradaKit")]
    public string InfoRetiradaKit { get; set; } = null!;

    [Column("idOrganizacao")]
    public uint IdOrganizacao { get; set; }

    [Column("nome")]
    public string Nome { get; set; } = null!;

    // Relacionamentos e Navegação
    public virtual ICollection<Cupom> Cupoms { get; set; } = new List<Cupom>();

    [ForeignKey("IdOrganizacao")]
    public virtual Organizacao IdOrganizacaoNavigation { get; set; } = null!;

    public virtual ICollection<Inscricao> Inscricaos { get; set; } = new List<Inscricao>();
    public virtual ICollection<Kit> Kits { get; set; } = new List<Kit>();
}
