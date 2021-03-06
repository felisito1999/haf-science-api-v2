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

        public virtual DbSet<CategoriasPregunta> CategoriasPreguntas { get; set; }
        public virtual DbSet<CentrosEducativo> CentrosEducativos { get; set; }
        public virtual DbSet<DetallePartida> DetallePartidas { get; set; }
        public virtual DbSet<Directore> Directores { get; set; }
        public virtual DbSet<Distrito> Distritos { get; set; }
        public virtual DbSet<Estado> Estados { get; set; }
        public virtual DbSet<ImagenesInsignia> ImagenesInsignias { get; set; }
        public virtual DbSet<Insignia> Insignias { get; set; }
        public virtual DbSet<Juego> Juegos { get; set; }
        public virtual DbSet<Municipio> Municipios { get; set; }
        public virtual DbSet<Nivele> Niveles { get; set; }
        public virtual DbSet<Pregunta> Preguntas { get; set; }
        public virtual DbSet<Provincia> Provincias { get; set; }
        public virtual DbSet<PruebasDiagnostica> PruebasDiagnosticas { get; set; }
        public virtual DbSet<PruebasPregunta> PruebasPreguntas { get; set; }
        public virtual DbSet<PruebasSesione> PruebasSesiones { get; set; }
        public virtual DbSet<Regionale> Regionales { get; set; }
        public virtual DbSet<Respuesta> Respuestas { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Sesione> Sesiones { get; set; }
        public virtual DbSet<TiposCentrosEducativo> TiposCentrosEducativos { get; set; }
        public virtual DbSet<TiposEstado> TiposEstados { get; set; }
        public virtual DbSet<TiposInsignia> TiposInsignias { get; set; }
        public virtual DbSet<TiposLog> TiposLogs { get; set; }
        public virtual DbSet<UserHash> UserHashes { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<UsuarioRealizaPrueba> UsuarioRealizaPruebas { get; set; }
        public virtual DbSet<UsuariosDetalle> UsuariosDetalles { get; set; }
        public virtual DbSet<UsuariosLog> UsuariosLogs { get; set; }
        public virtual DbSet<UsuariosSesione> UsuariosSesiones { get; set; }
        public virtual DbSet<UsuariosSesionesInsignia> UsuariosSesionesInsignias { get; set; }
        public virtual DbSet<UsuariosModel> UsuariosModel { get; set; }
        public virtual DbSet<UsuarioView> UsuariosView { get; set; }
        public virtual DbSet<PaginatedUsuariosView> PaginatedUsuariosView { get; set; }
        public virtual DbSet<PaginatedCentrosEducativosView> PaginatedCentrosEducativosView { get; set; }
        public virtual DbSet<CentrosEducativosModel> CentrosEducativosModel { get; set; }
        public virtual DbSet<RecordsTotalModel> TotalRecordsModel { get; set; }
        public virtual DbSet<CentrosEducativosSelectModel> CentrosEducativosSelectModel { get; set; }
        public virtual DbSet<SesionesModel> SesionesModel { get; set; }
        public virtual DbSet<PaginatedSesionesView> PaginatedSesionesView { get; set; }
        public virtual DbSet<SessionStudents> SessionStudents { get; set; }

        [DbFunction("fnCreateUsername", "dbo")]
        public static string CreateUsername(string nombre, string Apellido, DateTime FechaNacimiento)
        {
            throw new NotImplementedException();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.WriteLine("Arreglar la conexión a la base de datos");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CategoriasPregunta>(entity =>
            {
                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CentrosEducativo>(entity =>
            {
                entity.HasIndex(e => e.CodigoCentro, "UQ_CodigoCentro")
                    .IsUnique();

                entity.Property(e => e.CodigoCentro)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Director)
                    .WithMany(p => p.CentrosEducativos)
                    .HasForeignKey(d => d.DirectorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Directores_CentrosEducativos");

                entity.HasOne(d => d.Distrito)
                    .WithMany(p => p.CentrosEducativos)
                    .HasForeignKey(d => d.DistritoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Distritos_CentrosEducativos");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.CentrosEducativos)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Estados_CentrosEducativos");

                entity.HasOne(d => d.Municipio)
                    .WithMany(p => p.CentrosEducativos)
                    .HasForeignKey(d => d.MunicipioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Municipios_CentrosEducativos");

                entity.HasOne(d => d.Provincia)
                    .WithMany(p => p.CentrosEducativos)
                    .HasForeignKey(d => d.ProvinciaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Provincias_CentrosEducativos");

                entity.HasOne(d => d.Regional)
                    .WithMany(p => p.CentrosEducativos)
                    .HasForeignKey(d => d.RegionalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Regionales_CentrosEducativos");

                entity.HasOne(d => d.TipoCentroEducativo)
                    .WithMany(p => p.CentrosEducativos)
                    .HasForeignKey(d => d.TipoCentroEducativoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TiposCentrosEducativos_CentrosEducativos");
            });

            modelBuilder.Entity<DetallePartida>(entity =>
            {
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.FechaRealizacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<Directore>(entity =>
            {
                entity.HasIndex(e => e.CorreoElectronico, "UQ__Director__531402F352E8E6C9")
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
            });

            modelBuilder.Entity<Distrito>(entity =>
            {
                entity.HasIndex(e => e.Nombre, "UQ__Distrito__75E3EFCF3A96C1E0")
                    .IsUnique();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Regional)
                    .WithMany(p => p.Distritos)
                    .HasForeignKey(d => d.RegionalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Distritos__Regio__36470DEF");
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.HasOne(d => d.TipoEstado)
                    .WithMany(p => p.Estados)
                    .HasForeignKey(d => d.TipoEstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TiposEstados_Estados");
            });

            modelBuilder.Entity<ImagenesInsignia>(entity =>
            {
                entity.Property(e => e.ImgData)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.MimeType)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Insignia>(entity =>
            {
                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreadoPorNavigation)
                    .WithMany(p => p.Insignia)
                    .HasForeignKey(d => d.CreadoPor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuarios_Insignias");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Insignia)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Estados_Insignias");

                entity.HasOne(d => d.ImagenesInsigniasNavigation)
                    .WithMany(p => p.Insignia)
                    .HasForeignKey(d => d.ImagenesInsignias)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImagenesInsignias_Insignias");

                entity.HasOne(d => d.TipoInsignia)
                    .WithMany(p => p.Insignia)
                    .HasForeignKey(d => d.TipoInsigniaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TiposInsignias_Insignias");
            });

            modelBuilder.Entity<Juego>(entity =>
            {
                entity.Property(e => e.DescripcionJuego)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NombreJuego)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RecompensaJuego)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ReglasJuego)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Juegos)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Estados_Juegos");
            });

            modelBuilder.Entity<Municipio>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.HasOne(d => d.Provincia)
                    .WithMany(p => p.Municipios)
                    .HasForeignKey(d => d.ProvinciaId)
                    .HasConstraintName("FK_Provincias_Municipios");
            });

            modelBuilder.Entity<Nivele>(entity =>
            {
                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.RecompensaNivel)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Niveles)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Estados_Niveles");

                entity.HasOne(d => d.Juego)
                    .WithMany(p => p.Niveles)
                    .HasForeignKey(d => d.JuegoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Juegos_Niveles");
            });

            modelBuilder.Entity<Pregunta>(entity =>
            {
                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.CategoriaPregunta)
                    .WithMany(p => p.Pregunta)
                    .HasForeignKey(d => d.CategoriaPreguntaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoriasPreguntas_Preguntas");

                entity.HasOne(d => d.CreadoPorNavigation)
                    .WithMany(p => p.Pregunta)
                    .HasForeignKey(d => d.CreadoPor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsuariosPreguntas");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Pregunta)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Estados_Preguntas");
            });

            modelBuilder.Entity<Provincia>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PruebasDiagnostica>(entity =>
            {
                entity.Property(e => e.CalificacionMaxima).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreadoPorNavigation)
                    .WithMany(p => p.PruebasDiagnosticas)
                    .HasForeignKey(d => d.CreadoPor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuarios_PruebasDiagnosticas");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.PruebasDiagnosticas)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Estados_PruebasDiagnosticas");
            });

            modelBuilder.Entity<PruebasPregunta>(entity =>
            {
                entity.HasKey(e => new { e.PreguntaId, e.PruebaId });

                entity.Property(e => e.CalificacionMaximaPrueba).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.TituloPregunta)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TituloPrueba)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Valoracion).HasColumnType("decimal(4, 1)");

                entity.HasOne(d => d.Pregunta)
                    .WithMany(p => p.PruebasPregunta)
                    .HasForeignKey(d => d.PreguntaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Preguntas_PruebasPreguntas");

                entity.HasOne(d => d.Prueba)
                    .WithMany(p => p.PruebasPregunta)
                    .HasForeignKey(d => d.PruebaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pruebas_PruebasPreguntas");
            });

            modelBuilder.Entity<PruebasSesione>(entity =>
            {
                entity.HasKey(e => new { e.PruebaDiagnosticaId, e.SesionId });

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.FechaLimite).HasColumnType("datetime");

                entity.Property(e => e.NombreSesion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TituloPrueba)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.PruebaDiagnostica)
                    .WithMany(p => p.PruebasSesiones)
                    .HasForeignKey(d => d.PruebaDiagnosticaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PruebasDiagnosticas_PruebasSesiones");

                entity.HasOne(d => d.Sesion)
                    .WithMany(p => p.PruebasSesiones)
                    .HasForeignKey(d => d.SesionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sesiones_PruebasSesiones");
            });

            modelBuilder.Entity<Regionale>(entity =>
            {
                entity.HasIndex(e => e.Nombre, "UQ__Regional__75E3EFCFBF5A177F")
                    .IsUnique();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(35)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Respuesta>(entity =>
            {
                entity.Property(e => e.Contenido)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.CreadoPorNavigation)
                    .WithMany(p => p.Respuesta)
                    .HasForeignKey(d => d.CreadoPor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuarios_Respuestas");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Respuesta)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Estados_Respuestas");

                entity.HasOne(d => d.Pregunta)
                    .WithMany(p => p.Respuesta)
                    .HasForeignKey(d => d.PreguntaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Preguntas_Respuestas");
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

            modelBuilder.Entity<Sesione>(entity =>
            {
                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.CentroEducativo)
                    .WithMany(p => p.Sesiones)
                    .HasForeignKey(d => d.CentroEducativoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CentrosEducativos_Sesiones");

                entity.HasOne(d => d.CreadoPorNavigation)
                    .WithMany(p => p.Sesiones)
                    .HasForeignKey(d => d.CreadoPor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsuarioCreador_Sesiones");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Sesiones)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Estados_Sesiones");
            });

            modelBuilder.Entity<TiposCentrosEducativo>(entity =>
            {
                entity.HasIndex(e => e.Nombre, "UQ__TiposCen__75E3EFCF6B202F9F")
                    .IsUnique();

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(140)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposEstado>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposInsignia>(entity =>
            {
                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposLog>(entity =>
            {
                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserHash>(entity =>
            {
                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");

                entity.Property(e => e.Hash)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserHashes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserHashes_Usuarios");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.Codigo, "UQ__Usuarios__06370DAC1D3C1B2C")
                    .IsUnique();

                entity.HasIndex(e => e.NombreUsuario, "UQ__Usuarios__6B0F5AE0D557DE37")
                    .IsUnique();

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Contrasena)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.NombreUsuario)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordChangeRequired)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.CreadoPorNavigation)
                    .WithMany(p => p.InverseCreadoPorNavigation)
                    .HasForeignKey(d => d.CreadoPor)
                    .HasConstraintName("FK_Usuarios_Usuarios");

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

            modelBuilder.Entity<UsuarioRealizaPrueba>(entity =>
            {
                entity.ToTable("UsuarioRealizaPrueba");

                entity.Property(e => e.Calificacion).HasColumnType("decimal(4, 1)");

                entity.Property(e => e.FechaCompletado).HasColumnType("datetime");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.NombreUsuario)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TituloPrueba)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.PruebaDiagnostica)
                    .WithMany(p => p.UsuarioRealizaPruebas)
                    .HasForeignKey(d => d.PruebaDiagnosticaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PruebasDiagnosticas_UsuarioRealizaPrueba");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.UsuarioRealizaPruebas)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sesiones_UsuarioRealizaPrueba");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.UsuarioRealizaPruebas)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuarios_UsuarioRealizaPrueba");
            });

            modelBuilder.Entity<UsuariosDetalle>(entity =>
            {
                entity.HasIndex(e => e.CorreoElectronico, "UQ__Usuarios__531402F3CDF8A553")
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
                    .HasConstraintName("FK_CentrosEducativos_Usuarios");
            });

            modelBuilder.Entity<UsuariosLog>(entity =>
            {
                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.TipoLogNavigation)
                    .WithMany(p => p.UsuariosLogs)
                    .HasForeignKey(d => d.TipoLog)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TipoActividad_ActividadUsuarios");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.UsuariosLogs)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_ActividadUsuarios");
            });

            modelBuilder.Entity<UsuariosSesione>(entity =>
            {
                entity.HasKey(e => new { e.UsuarioId, e.SesionId });

                entity.Property(e => e.NombreSesion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NombreUsuario)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Sesion)
                    .WithMany(p => p.UsuariosSesiones)
                    .HasForeignKey(d => d.SesionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sesiones_UsuariosSesiones");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.UsuariosSesiones)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuarios_UsuariosSesiones");
            });

            modelBuilder.Entity<UsuariosSesionesInsignia>(entity =>
            {
                entity.HasKey(e => new { e.UsuarioId, e.SesionId, e.InsigniaId });

                entity.HasOne(d => d.Insignia)
                    .WithMany(p => p.UsuariosSesionesInsignia)
                    .HasForeignKey(d => d.InsigniaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Insignias_UsuariosSesionesInsignias");

                entity.HasOne(d => d.Sesion)
                    .WithMany(p => p.UsuariosSesionesInsignia)
                    .HasForeignKey(d => d.SesionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sesiones_UsuariosSesionesInsignias");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.UsuariosSesionesInsignia)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuarios_UsuariosSesionesInsignias");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
