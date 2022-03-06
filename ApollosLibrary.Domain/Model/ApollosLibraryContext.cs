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

            });
        }
    }
}
