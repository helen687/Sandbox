using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CatalogCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogDBContext
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity => {
                entity.HasKey(a => a.Id);
            });
            modelBuilder.Entity<Book>(entity => {
                entity.HasKey(b => b.Id);
                entity.HasOne(b => b.Author);
                entity.HasMany(b => b.Reviews);
            });
            modelBuilder.Entity<Review>(entity => {
                entity.HasKey(r => r.Id);
            });
        }
    }
}
