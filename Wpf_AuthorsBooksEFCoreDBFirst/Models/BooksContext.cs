using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace Wpf_AuthorsBooksEFCoreDBFirst.Models
{
    public class BooksContext : DbContext
    {
        static DbContextOptions<BooksContext> _options;

        static BooksContext()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }
           

            var optionsBuilder = new DbContextOptionsBuilder<BooksContext>();
            _options = optionsBuilder.UseSqlServer(connectionString)                                  
                                     .Options;
        }

        public BooksContext() : base(_options)
        {
          
            if (Database.EnsureCreated())
            {
                SeedData();
            }
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        private void SeedData()
        {
            var authors = new[]
            {
        new Author { Name = "William Shakespeare", Books = new List<Book> {
            new Book { Title = "Hamlet" },
            new Book { Title = "Othello" }
        }},
        new Author { Name = "Johann Wolfgang von Goethe", Books = new List<Book> {
            new Book { Title = "Faust" }
        }},
        new Author { Name = "Victor Hugo", Books = new List<Book> {
            new Book { Title = "Les Misérables" }
        }}
    };

            Authors.AddRange(authors);
            SaveChanges();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(false);//отключила          
        }
    }
}