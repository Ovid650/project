using LibraryDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryDataAccess.Data
{
    public class LibraryContext : DbContext
    {
        private string ConnectionString;

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }

        public LibraryContext(DbContextOptions<LibraryContext> optionsBuilder) : base(optionsBuilder)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            ConnectionString = System.IO.Path.Join(path, "library.db");
        }

       /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={ConnectionString}");
            base.OnConfiguring(optionsBuilder);
        }
*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            { 
                entity.HasKey(b => b.BookId);

                entity.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(b => b.Price)
                .IsRequired()
                .HasColumnType("decimal(10, 2)");

                entity.Property(b => b.AuthorId)
                .IsRequired();

                entity.HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .OnDelete(DeleteBehavior.Restrict);

                entity.Property(b => b.CategoryId)
                .IsRequired();

                entity.HasOne(b => b.Category)
                .WithMany(a => a.Books)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(a => a.AuthorName)
                .IsRequired()
                .HasMaxLength(50);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
