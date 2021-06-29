using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BirdTrackerProject
{
    public partial class BirdTrackerMSSQLContext : DbContext
    {
        public BirdTrackerMSSQLContext()
        {
        }

        public BirdTrackerMSSQLContext(DbContextOptions<BirdTrackerMSSQLContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bird> Birds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:birdtracker.database.windows.net,1433;Initial Catalog=BirdTracker-MSSQL;Persist Security Info=False;User ID=btAdmin;Password=btDHBW21;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Bird>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Adress)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.BoxKind)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Compass)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.NestDate).HasColumnType("date");

                entity.Property(e => e.Plz).HasColumnName("PLZ");

                entity.Property(e => e.Species)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
