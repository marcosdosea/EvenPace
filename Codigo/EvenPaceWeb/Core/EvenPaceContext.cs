using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Core
{
    public partial class EvenPaceContext : DbContext
    {
        public EvenPaceContext(DbContextOptions<EvenPaceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrador> Administradors { get; set; }
        public virtual DbSet<AvaliacaoEvento> AvaliacaoEventos { get; set; }
        public virtual DbSet<CartaoCredito> CartaoCreditos { get; set; }
        public virtual DbSet<Corredor> Corredors { get; set; }
        public virtual DbSet<Cupom> Cupoms { get; set; }
        public virtual DbSet<Evento> Eventos { get; set; }
        public virtual DbSet<Inscricao> Inscricaos { get; set; }
        public virtual DbSet<Kit> Kits { get; set; }
        public virtual DbSet<Organizacao> Organizacaos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrador>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("Administrador");

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

            modelBuilder.Entity<AvaliacaoEvento>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("AvaliacaoEvento");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Comentario)
                    .HasMaxLength(250)
                    .HasColumnName("comentario");
                entity.Property(e => e.Estrela).HasColumnName("estrela");
            });

            modelBuilder.Entity<CartaoCredito>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("CartaoCredito");

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
                entity.Property(e => e.Numero)
                    .HasMaxLength(16)
                    .IsFixedLength()
                    .HasColumnName("numero");

                entity.HasOne(d => d.IdCorredorNavigation)
                    .WithOne(p => p.CartaoCredito)
                    .HasForeignKey<CartaoCredito>(d => d.IdCorredor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CartaoCredito_Corredor1");
            });

            modelBuilder.Entity<Corredor>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("Corredor");

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

                entity.ToTable("Cupom");

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

                entity.HasOne(d => d.IdEventoNavigation)
                    .WithMany(p => p.Cupoms)
                    .HasForeignKey(d => d.IdEvento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Cupom_Evento1");
            });

            modelBuilder.Entity<Evento>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("Evento");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nome)
                    .HasMaxLength(45)
                    .HasColumnName("nome");
                entity.Property(e => e.Data)
                    .HasColumnType("datetime")
                    .HasColumnName("data");
            });

            modelBuilder.Entity<Inscricao>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("Inscricao");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DataInscricao)
                    .HasColumnType("date")
                    .HasColumnName("dataInscricao");
                entity.Property(e => e.Distancia)
                    .HasMaxLength(45)
                    .HasColumnName("distancia");
            });

            modelBuilder.Entity<Kit>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("Kit");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nome)
                    .HasMaxLength(45)
                    .HasColumnName("nome");
            });

            modelBuilder.Entity<Organizacao>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("Organizacao");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Email)
                    .HasMaxLength(45)
                    .HasColumnName("email");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
