using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

# nullable disable

namespace ApollosLibrary.Domain
{
    public class ApollosLibraryContext : DbContext
    {

        public ApollosLibraryContext(DbContextOptions<ApollosLibraryContext> options) : base(options)
        {

        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<ErrorCode> ErrorCodes { get; set; }
        public DbSet<FictionType> FictionTypes { get; set; }
        public DbSet<FormType> FormTypes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<PublicationFormat> PublicationFormats { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Series> Series { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FictionType>().HasData(new List<FictionType>()
            {
                new FictionType()
                {
                    TypeId = 1,
                    Name = "Non-Fiction",
                },
                new FictionType()
                {
                    TypeId = 2,
                    Name = "Fiction",
                }
            });

            modelBuilder.Entity<FormType>().HasData(new List<FormType>()
            {
                new FormType()
                {
                    TypeId = 1,
                    Name = "Novel",
                },
                new FormType()
                {
                    TypeId = 2,
                    Name = "Novella",
                },
                new FormType()
                {
                    TypeId = 3,
                    Name = "Screenplay",
                },
                new FormType()
                {
                    TypeId = 4,
                    Name = "Manuscript",
                },
                new FormType()
                {
                    TypeId = 5,
                    Name = "Poem",
                },
                new FormType()
                {
                    TypeId = 6,
                    Name = "Textbook",
                },
            });

            modelBuilder.Entity<PublicationFormat>().HasData(new List<PublicationFormat>()
            {
                new PublicationFormat()
                {
                    TypeId = 1,
                    Name = "Printed",
                },
                new PublicationFormat()
                {
                    TypeId = 2,
                    Name = "eBook",
                },
                new PublicationFormat()
                {
                    TypeId = 3,
                    Name = "Audio Book",
                },
            });
        }
    }
}
