using System.ComponentModel.DataAnnotations.Schema;
using Core;

public partial class Kit
{
    [Column("id")]
    public int Id { get; set; }

    [Column("valor")]
    public decimal Valor { get; set; }

    [Column("nome")]
    public string Nome { get; set; } = null!;

    [Column("descricao")]
    public string Descricao { get; set; } = null!;

    [Column("disponibilidadep")]
    public int DisponibilidadeP { get; set; }

    [Column("disponibilidadeg")]
    public int DisponibilidadeG { get; set; }

    [Column("disponibilidadem")]
    public int DisponibilidadeM { get; set; }

    [Column("utilizadap")]
    public sbyte UtilizadaP { get; set; }

    [Column("utilizadag")]
    public sbyte UtilizadaG { get; set; }

    [Column("utilizadam")]
    public sbyte UtilizadaM { get; set; }

    [Column("idevento")]
    public int IdEvento { get; set; }

    [Column("statusretiradakit")]
    public bool StatusRetiradaKit { get; set; }

    [Column("dataretirada")]
    public DateTime DataRetirada { get; set; }

    [Column("imagem")]
    public string? Imagem { get; set; }

    public virtual Evento IdEventoNavigation { get; set; } = null!;

    public virtual ICollection<Inscricao> Inscricao { get; set; } = new List<Inscricao>();
}
