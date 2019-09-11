using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CasoNaranjitoSac.Models
{
    public class AnalyticsContext : DbContext
    {
        public AnalyticsContext(DbContextOptions<AnalyticsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Link> Link { get; set; }
        public virtual DbSet<Page> Page { get; set; }
        public virtual DbSet<Session> Session { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Link>(entity =>
            {
                entity.HasKey(e => e.IdLink);

                entity.ToTable("link", "casonaranjitosac");

                entity.HasIndex(e => e.IdSession)
                    .HasName("idSession");

                entity.Property(e => e.IdLink)
                    .HasColumnName("idLink")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.IdSession)
                    .HasColumnName("idSession")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UrlLink)
                    .IsRequired()
                    .HasColumnName("urlLink")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdSessionNavigation)
                    .WithMany(p => p.Link)
                    .HasForeignKey(d => d.IdSession)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("link_ibfk_1");
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.HasKey(e => e.IdPage);

                entity.ToTable("page", "casonaranjitosac");

                entity.HasIndex(e => e.IdSession)
                    .HasName("idSession");

                entity.Property(e => e.IdPage)
                    .HasColumnName("idPage")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ended)
                    .HasColumnName("ended")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.IdSession)
                    .HasColumnName("idSession")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Initial)
                    .HasColumnName("initial")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.UrlVisit)
                    .IsRequired()
                    .HasColumnName("urlVisit")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdSessionNavigation)
                    .WithMany(p => p.Page)
                    .HasForeignKey(d => d.IdSession)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("page_ibfk_1");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.IdSession);

                entity.ToTable("session", "casonaranjitosac");

                entity.HasIndex(e => e.Uuid)
                    .HasName("uuid")
                    .IsUnique();

                entity.Property(e => e.IdSession)
                    .HasColumnName("idSession")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Created)
                    .HasColumnName("created")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.UrlOrigin)
                    .IsRequired()
                    .HasColumnName("urlOrigin")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Uuid)
                    .HasColumnName("uuid")
                    .HasColumnType("char(36)")
                    .HasDefaultValueSql("NULL");
            });
        }
    }
}