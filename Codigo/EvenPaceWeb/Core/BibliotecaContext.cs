using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Core;

public partial class BibliotecaContext : DbContext
{
    public BibliotecaContext()
    {
    }

    public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admistrador> Admistradors { get; set; }

    public virtual DbSet<Avaliacaoevento> Avaliacaoeventos { get; set; }

    public virtual DbSet<Cartaocredito> Cartaocreditos { get; set; }

    public virtual DbSet<Corredor> Corredors { get; set; }

    public virtual DbSet<Cupom> Cupoms { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<Inscricao> Inscricaos { get; set; }

    public virtual DbSet<Kit> Kits { get; set; }

    public virtual DbSet<Organizacao> Organizacaos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=123456;database=mydb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admistrador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("admistrador");

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(45)
                .HasColumnName("nome");
            entity.Property(e => e.Senha)
                .HasMaxLength(45)
                .HasColumnName("senha");
        });

        modelBuilder.Entity<Avaliacaoevento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("avaliacaoevento");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comentario)
                .HasMaxLength(250)
                .HasColumnName("comentario");
            entity.Property(e => e.Estrela).HasColumnName("estrela");
        });

        modelBuilder.Entity<Cartaocredito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cartaocredito");

            entity.HasIndex(e => e.IdCorredor, "fk_CartaoCredito_Corredor1_idx");

            entity.HasIndex(e => e.IdCorredor, "idCorredor_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Numero, "numero_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodeSeguranca).HasColumnName("codeSeguranca");
            entity.Property(e => e.DataValidade)
                .HasColumnType("date")
                .HasColumnName("dataValidade");
            entity.Property(e => e.IdCorredor).HasColumnName("idCorredor");
            entity.Property(e => e.Nome)
                .HasMaxLength(45)
                .HasColumnName("nome");
            entity.Property(e => e.Numero).HasColumnName("numero");

            entity.HasOne(d => d.IdCorredorNavigation).WithOne(p => p.Cartaocredito)
                .HasForeignKey<Cartaocredito>(d => d.IdCorredor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_CartaoCredito_Corredor1");
        });

        modelBuilder.Entity<Corredor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("corredor");

            entity.HasIndex(e => e.Cpf, "cpf_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("cpf");
            entity.Property(e => e.DataNascimento)
                .HasColumnType("date")
                .HasColumnName("dataNascimento");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(45)
                .HasColumnName("nome");
            entity.Property(e => e.Senha)
                .HasMaxLength(45)
                .HasColumnName("senha");
        });

        modelBuilder.Entity<Cupom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cupom");

            entity.HasIndex(e => e.IdEvento, "fk_Cupom_Evento1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataInicio)
                .HasColumnType("date")
                .HasColumnName("dataInicio");
            entity.Property(e => e.DataTermino)
                .HasColumnType("date")
                .HasColumnName("dataTermino");
            entity.Property(e => e.Desconto).HasColumnName("desconto");
            entity.Property(e => e.IdEvento).HasColumnName("idEvento");
            entity.Property(e => e.Nome)
                .HasMaxLength(45)
                .HasColumnName("nome");
            entity.Property(e => e.QuantiadeDisponibilizada).HasColumnName("quantiadeDisponibilizada");
            entity.Property(e => e.QuantidadeUtilizada).HasColumnName("quantidadeUtilizada");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.Cupoms)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Cupom_Evento1");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("evento");

            entity.HasIndex(e => e.IdOrganizacao, "fk_Evento_Organizacao_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(45)
                .HasColumnName("bairro");
            entity.Property(e => e.Cidade)
                .HasMaxLength(45)
                .HasColumnName("cidade");
            entity.Property(e => e.Data)
                .HasColumnType("datetime")
                .HasColumnName("data");
            entity.Property(e => e.Discricao)
                .HasMaxLength(250)
                .HasColumnName("discricao");
            entity.Property(e => e.Distancia15).HasColumnName("distancia15");
            entity.Property(e => e.Distancia21).HasColumnName("distancia21");
            entity.Property(e => e.Distancia3).HasColumnName("distancia3");
            entity.Property(e => e.Distancia42).HasColumnName("distancia42");
            entity.Property(e => e.Distancia5).HasColumnName("distancia5");
            entity.Property(e => e.Distancia7).HasColumnName("distancia7");
            entity.Property(e => e.Estado)
                .HasMaxLength(45)
                .HasColumnName("estado");
            entity.Property(e => e.IdOrganizacao).HasColumnName("idOrganizacao");
            entity.Property(e => e.InfoRetiradaKit)
                .HasMaxLength(45)
                .HasColumnName("infoRetiradaKit");
            entity.Property(e => e.NumeroParticipantes).HasColumnName("numeroParticipantes");
            entity.Property(e => e.Rua)
                .HasMaxLength(45)
                .HasColumnName("rua");

            entity.HasOne(d => d.IdOrganizacaoNavigation).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.IdOrganizacao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Evento_Organizacao");
        });

        modelBuilder.Entity<Inscricao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("inscricao");

            entity.HasIndex(e => e.IdAvaliacaoEvento, "fk_Inscricao_AvaliacaoEvento1_idx");

            entity.HasIndex(e => e.IdCorredor, "fk_Inscricao_Corredor1_idx");

            entity.HasIndex(e => e.IdEvento, "fk_Inscricao_Evento1_idx");

            entity.HasIndex(e => e.IdKit, "fk_Inscricao_Kit1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataInscricao)
                .HasColumnType("date")
                .HasColumnName("dataInscricao");
            entity.Property(e => e.Distancia)
                .HasMaxLength(45)
                .HasColumnName("distancia");
            entity.Property(e => e.IdAvaliacaoEvento).HasColumnName("idAvaliacaoEvento");
            entity.Property(e => e.IdCorredor).HasColumnName("idCorredor");
            entity.Property(e => e.IdEvento).HasColumnName("idEvento");
            entity.Property(e => e.IdKit).HasColumnName("idKit");
            entity.Property(e => e.Posicao).HasColumnName("posicao");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasColumnName("status");
            entity.Property(e => e.TamanhoCamisa)
                .HasMaxLength(45)
                .HasColumnName("tamanhoCamisa");
            entity.Property(e => e.Tempo)
                .HasColumnType("time")
                .HasColumnName("tempo");

            entity.HasOne(d => d.IdAvaliacaoEventoNavigation).WithMany(p => p.Inscricaos)
                .HasForeignKey(d => d.IdAvaliacaoEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Inscricao_AvaliacaoEvento1");

            entity.HasOne(d => d.IdCorredorNavigation).WithMany(p => p.Inscricaos)
                .HasForeignKey(d => d.IdCorredor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Inscricao_Corredor1");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.Inscricaos)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Inscricao_Evento1");

            entity.HasOne(d => d.IdKitNavigation).WithMany(p => p.Inscricaos)
                .HasForeignKey(d => d.IdKit)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Inscricao_Kit1");
        });

        modelBuilder.Entity<Kit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("kit");

            entity.HasIndex(e => e.IdEvento, "fk_Kit_Evento1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataRetirada)
                .HasColumnType("datetime")
                .HasColumnName("dataRetirada");
            entity.Property(e => e.Descricao)
                .HasMaxLength(45)
                .HasColumnName("descricao");
            entity.Property(e => e.DisponibilidadeG).HasColumnName("disponibilidadeG");
            entity.Property(e => e.DisponibilidadeM).HasColumnName("disponibilidadeM");
            entity.Property(e => e.DisponibilidadeP).HasColumnName("disponibilidadeP");
            entity.Property(e => e.IdEvento).HasColumnName("idEvento");
            entity.Property(e => e.Nome)
                .HasMaxLength(45)
                .HasColumnName("nome");
            entity.Property(e => e.StatusRetiradaKit).HasColumnName("statusRetiradaKit");
            entity.Property(e => e.UtilizadaG).HasColumnName("utilizadaG");
            entity.Property(e => e.UtilizadaM).HasColumnName("utilizadaM");
            entity.Property(e => e.UtilizadaP).HasColumnName("utilizadaP");
            entity.Property(e => e.Valor).HasColumnName("valor");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.Kits)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Kit_Evento1");
        });

        modelBuilder.Entity<Organizacao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("organizacao");

            entity.HasIndex(e => e.Cnpj, "cnpj_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.AdmistradorId, "fk_Organizacao_Admistrador1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdmistradorId).HasColumnName("Admistrador_id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(45)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(45)
                .HasColumnName("cidade");
            entity.Property(e => e.Cnpj)
                .HasMaxLength(14)
                .IsFixedLength()
                .HasColumnName("cnpj");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("cpf");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasMaxLength(45)
                .HasColumnName("estado");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.Rua)
                .HasMaxLength(45)
                .HasColumnName("rua");
            entity.Property(e => e.Senha)
                .HasMaxLength(45)
                .HasColumnName("senha");
            entity.Property(e => e.StatusSituacao).HasColumnName("statusSituacao");
            entity.Property(e => e.Telefone).HasColumnName("telefone");

            entity.HasOne(d => d.Admistrador).WithMany(p => p.Organizacaos)
                .HasForeignKey(d => d.AdmistradorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Organizacao_Admistrador1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
