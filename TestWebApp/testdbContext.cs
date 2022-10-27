using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using TestWebApp.Extensions;

namespace TestWebApp
{
    public partial class testdbContext : DbContext
    {
        public testdbContext()
        {
        }

        public testdbContext(DbContextOptions<testdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Curve> Curves { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=testdb")
                    .ReplaceService<IQueryableMethodTranslatingExpressionVisitorFactory,
                        ForSystemtimeQueryableMethodTranslatingExpressionVisitorFactory>()
                    .ReplaceService<IQuerySqlGeneratorFactory, ForSystemtimeQuerySqlGeneratorFactory>()
                    .ReplaceService<ISqlExpressionFactory, ForSystemtimeSqlExpressionFactory>();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Curve>(entity =>
            {
                entity.HasKey(e => new { e.Company, e.Segment, e.CurveType, e.VersionNumber });

                entity.ToTable("Curve");

                entity.Property(e => e.Company)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Segment)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.CurveType)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.CreationTime).HasColumnType("datetime");

                entity.Property(e => e.Creator)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.LastUser)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.NextVersionNumber).HasComputedColumnSql("(case when [IsLastVersion]=(1) then NULL else [VersionNumber]+(1) end)", true);

                entity.Property(e => e.ValidFrom).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
