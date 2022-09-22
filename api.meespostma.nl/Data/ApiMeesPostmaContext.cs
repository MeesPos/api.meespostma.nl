using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace api.meespostma.nl.Data
{
    public partial class ApiMeesPostmaContext : DbContext
    {
        public ApiMeesPostmaContext()
        {
        }

        public ApiMeesPostmaContext(DbContextOptions<ApiMeesPostmaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Project> Projects { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Logo).HasColumnType("image");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Url).HasMaxLength(250);

                entity.Property(e => e.UrlPlaceholder).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
