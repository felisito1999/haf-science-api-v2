using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace haf_science_api.Models
{
    public partial class HafScienceDbContext : DbContext
    {

        public HafScienceDbContext(DbContextOptions<HafScienceDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActividadUsuario> ActividadUsuarios { get; set; }
        public virtual DbSet<CentrosEducativo> CentrosEducativos { get; set; }
        public virtual DbSet<Estado> Estados { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<TipoActividad> TipoActividads { get; set; }
        public virtual DbSet<TiposEstado> TiposEstados { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<UsuariosDetalle> UsuariosDetalles { get; set; }
        public virtual DbSet<UsuariosModel> UsuariosModel { get; set; }
        public virtual DbSet<UsuarioView> UsuariosView { get; set; }
        public virtual DbSet<PaginatedUsuariosView> PaginatedUsuariosView { get; set; }
        public virtual DbSet<PaginatedCentrosEducativosView> PaginatedCentrosEducativosView { get; set; }
        public virtual DbSet<CentrosEducativosModel> CentrosEducativosModel { get; set; }
        public virtual DbSet<RecordsTotalModel> TotalRecordsModel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HafScienceDb;Integrated Security=True");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ActividadUsuario>(entity =>
            {
                entity.HasOne(d => d.TipoActividad)
                    .WithMany(p => p.ActividadUsuarios)
                    .HasForeignKey(d => d.TipoActividadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TipoActividad_ActividadUsuarios");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.ActividadUsuarios)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_ActividadUsuarios");
            });

            modelBuilder.Entity<CentrosEducativo>(entity =>
            {
                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.HasOne(d => d.TipoEstado)
                    .WithMany(p => p.Estados)
                    .HasForeignKey(d => d.TipoEstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TiposEstados_Estados");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoActividad>(entity =>
            {
                entity.ToTable("TipoActividad");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposEstado>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.NombreUsuario, "UQ__Usuarios__6B0F5AE0A935EB0E")
                    .IsUnique();

                entity.Property(e => e.Contrasena)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.NombreUsuario)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Estados_Usuarios");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.RolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rol_Usuarios");

                entity.HasOne(d => d.UsuarioDetalle)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.UsuarioDetalleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetalleUsuarios_Usuarios");
            });

            modelBuilder.Entity<UsuariosDetalle>(entity =>
            {
                entity.HasIndex(e => e.CorreoElectronico, "UQ__Usuarios__531402F364AEC5CD")
                    .IsUnique();

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.FechaNacimiento).HasColumnType("date");

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.HasOne(d => d.CentroEducativo)
                    .WithMany(p => p.UsuariosDetalles)
                    .HasForeignKey(d => d.CentroEducativoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CentrosEducativos_Usuarios");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
