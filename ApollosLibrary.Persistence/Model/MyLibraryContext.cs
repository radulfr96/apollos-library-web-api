using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ApollosLibrary.Persistence.Model
{
    public partial class ApollosLibraryContext : DbContext
    {
        public ApollosLibraryContext()
        {
        }

        public ApollosLibraryContext(DbContextOptions<ApollosLibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookAuthor> BookAuthors { get; set; }
        public virtual DbSet<BookGenre> BookGenres { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<ErrorCode> ErrorCodes { get; set; }
        public virtual DbSet<FictionType> FictionTypes { get; set; }
        public virtual DbSet<FormType> FormTypes { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<PublicationFormat> PublicationFormats { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserClaim> UserClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("Author", "Author");

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.CountryId)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CountryID")
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Authors)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuthorCountry");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book", "Book");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.CoverImage).HasColumnType("text");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EIsbn)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("eISBN");

                entity.Property(e => e.FictionTypeId).HasColumnName("FictionTypeID");

                entity.Property(e => e.FormTypeId).HasColumnName("FormTypeID");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("ISBN");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.NumberInSeries).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.PublicationFormatId).HasColumnName("PublicationFormatID");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.Property(e => e.SeriesId).HasColumnName("SeriesID");

                entity.Property(e => e.Subtitle)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.FictionType)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.FictionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FictionTypeBook");

                entity.HasOne(d => d.FormType)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.FormTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FormTypeBook");

                entity.HasOne(d => d.PublicationFormat)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PublicationFormatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PublicationFormatBook");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookPublisher");

                entity.HasOne(d => d.Series)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.SeriesId)
                    .HasConstraintName("FK_SeriesBook");
            });

            modelBuilder.Entity<BookAuthor>(entity =>
            {
                entity.HasKey(e => new { e.AuthorId, e.BookId });

                entity.ToTable("BookAuthor", "Book");

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.BookAuthors)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookAuthorAuthor");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookAuthors)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookAuthorBook");
            });

            modelBuilder.Entity<BookGenre>(entity =>
            {
                entity.HasKey(e => new { e.GenreId, e.BookId });

                entity.ToTable("BookGenre", "Book");

                entity.Property(e => e.GenreId).HasColumnName("GenreID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookGenres)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookGenreBook");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.BookGenres)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookGenreGenre");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.Property(e => e.CountryId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CountryID")
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ErrorCode>(entity =>
            {
                entity.ToTable("ErrorCode");

                entity.Property(e => e.ErrorCodeId).ValueGeneratedNever();

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FictionType>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.ToTable("FictionType", "Book");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FormType>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.ToTable("FormType", "Book");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genre", "Genre");

                entity.Property(e => e.GenreId).HasColumnName("GenreID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PublicationFormat>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.ToTable("PublicationFormat", "Book");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.ToTable("Publisher", "Publisher");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CountryId)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CountryID")
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Postcode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StreetAddress)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Street Address");

                entity.Property(e => e.Website)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Publishers)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PublisherCountry");
            });

            modelBuilder.Entity<Series>(entity =>
            {
                entity.ToTable("Series", "Book");

                entity.Property(e => e.SeriesId).HasColumnName("SeriesID");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "Users");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Password).HasColumnType("text");

                entity.Property(e => e.SecurityCode)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityCodeExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserClaim>(entity =>
            {
                entity.ToTable("UserClaim", "Users");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserClaims)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserUserClaim");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
