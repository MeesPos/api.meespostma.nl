using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace api.meespostma.nl.Data
{
    public partial class ApiMeesPostmaContext : IdentityDbContext<ApiUser>
    {
        private readonly IConfiguration configuration;

        public ApiMeesPostmaContext()
        {
        }

        public ApiMeesPostmaContext(DbContextOptions<ApiMeesPostmaContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        public virtual DbSet<Project> Projects { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Logo).HasColumnType("image");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Url).HasMaxLength(250);

                entity.Property(e => e.UrlPlaceholder).HasMaxLength(50);
            });

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    Id = "21ef1e08-b29f-44da-9554-9d68548e7ed7"
                }
            );

            var hasher = new PasswordHasher<ApiUser>();

            modelBuilder.Entity<ApiUser>().HasData(
                new ApiUser
                {
                    Id = "bcb16a85-fee6-4666-bf8d-5f30523f990b",
                    Email = "mail@meespostma.nl",
                    NormalizedEmail = "MAIL@MEESPOSTMA.NL",
                    UserName = "MeesPos",
                    NormalizedUserName = "MEESPOS",
                    PasswordHash = hasher.HashPassword(null, configuration["User:Password"])
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "21ef1e08-b29f-44da-9554-9d68548e7ed7",
                    UserId = "bcb16a85-fee6-4666-bf8d-5f30523f990b"
                }
            );

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
