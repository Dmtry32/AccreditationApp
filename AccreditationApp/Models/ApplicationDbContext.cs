using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure; // Add this using directive
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore.Metadata.Builders; // Add this using directive to fix CS0246

namespace AccreditationApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Ensure Accreditation type is defined somewhere in your project
        public DbSet<Accreditation> Accreditations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Accreditation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Bank).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Status).HasMaxLength(50);
            });
        }
    }
}