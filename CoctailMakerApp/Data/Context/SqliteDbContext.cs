using CoctailMakerApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Reflection;

namespace CoctailMakerApp.Data.Context
{
    public partial class SqliteDbContext : DbContext
    {
        private static readonly string DATABASE_FILENAME = "Database.sqlite";
        public SqliteDbContext() : base()
        {
            Database.EnsureCreated();
        }

        //public DbSet<Blog> Blogs { get; set; }
        public DbSet<LogEvent> LogEvents { get; set; }
        public DbSet<SystemJsonConfig> SystemJsonConfigs { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DATABASE_FILENAME}", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Blog>(entity =>
            //{
            //    entity.HasKey(e => e.BlogId);
            //    entity.HasIndex(e => e.Title).IsUnique();
            //    entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
            //});

            modelBuilder.Entity<LogEvent>(entity =>
            {
                entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<SystemJsonConfig>(entity =>
            {
                entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            base.OnModelCreating(modelBuilder);
        }

        internal static void DeleteDatabase()
        {
            if (File.Exists(DATABASE_FILENAME))
            {
                File.Delete(DATABASE_FILENAME);
            }
        }
    }
}
