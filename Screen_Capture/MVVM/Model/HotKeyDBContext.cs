using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Screen_Capture.MVVM.Model
{
    public partial class HotKeyDBContext : DbContext
    {
        public HotKeyDBContext()
        {
        }

        public HotKeyDBContext(DbContextOptions<HotKeyDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HotKeys> HotKeys { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlite("Data Source=HotKeyDB.db");
            }
        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HotKeys>(entity =>
            {
                entity.HasIndex(e => e.HotKeyId)
                    .IsUnique();

                entity.Property(e => e.HotKeyId).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
