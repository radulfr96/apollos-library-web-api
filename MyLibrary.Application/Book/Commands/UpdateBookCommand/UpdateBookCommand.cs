using MediatR;
using MyLibrary.Application.Interfaces;
using MyLibrary.Persistence.Model;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Book.Commands.UpdateBookCommand
{
    public class UpdateBookCommand : IRequest<UpdateBookCommandDto>
    {
        public int BookID { get; set; }
        public string ISBN { get; set; }
        public string eISBN { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a title")]
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int? SeriesID { get; set; }
        public int? NumberInSeries { get; set; }
        public int? Edition { get; set; }
        public int PublicationFormatID { get; set; }
        public int FictionTypeID { get; set; }
        public int FormTypeID { get; set; }
        public int PublisherIDs { get; set; }
        public byte[] CoverImage { get; set; }
        public List<int> Genres { get; set; } = new List<int>();
        public List<int> Authors { get; set; } = new List<int>();

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (string.IsNullOrEmpty(eISBN) && string.IsNullOrEmpty(ISBN))
            {
                results.Add(new ValidationResult("You must provide a ISBN or an eISBN"));
            }

            return results;
        }
    }

    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, UpdateBookCommandDto>
    {
        private readonly IBookUnitOfWork _bookUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public UpdateBookCommandHandler(IBookUnitOfWork bookUnitOfWork, IUserService userService, IDateTimeService dateTimeService)
        {
            _bookUnitOfWork = bookUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
        }

        public async Task<UpdateBookCommandDto> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
        {
            var response = new UpdateBookCommandDto();

            var book = await _bookUnitOfWork.BookDataLayer.GetBook(command.BookID);

            if (book == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            if (!string.IsNullOrEmpty(command.ISBN))
            {
                var existingISBN = await _bookUnitOfWork.BookDataLayer.GetBookByISBN(command.ISBN);

                if (existingISBN != null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Messages.Add("Book with that ISBN already exists.");
                    return response;
                }

            }

            if (!string.IsNullOrEmpty(command.eISBN))
            {
                var existingeISBN = await _bookUnitOfWork.BookDataLayer.GetBookByeISBN(command.eISBN);

                if (existingeISBN != null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Messages.Add("Book with that eISBN already exists.");
                    return response;
                }

            }


            book.CoverImage = command.CoverImage == null ? null : Convert.ToBase64String(command.CoverImage);
            book.CreatedBy = _userService.GetUserId();
            book.CreatedDate = _dateTimeService.Now;
            book.Edition = command.Edition;
            book.EIsbn = command.eISBN;
            book.FictionTypeId = command.FictionTypeID;
            book.FormTypeId = command.FormTypeID;
            book.Isbn = command.ISBN;
            book.NumberInSeries = command.NumberInSeries;
            book.PublicationFormatId = command.PublicationFormatID;
            book.PublisherId = command.PublisherIDs;
            book.SeriesId = command.SeriesID;
            book.Subtitle = command.Subtitle;
            book.Title = command.Title;

            _bookUnitOfWork.BookDataLayer.DeleteBookAuthorRelationships(command.BookID);
            _bookUnitOfWork.BookDataLayer.DeleteBookGenreRelationships(command.BookID);

            await _bookUnitOfWork.Begin();

            foreach (int authorId in command.Authors)
            {
                await _bookUnitOfWork.BookDataLayer.AddBookAuthor(new BookAuthor()
                {
                    AuthorId = authorId,
                    BookId = book.BookId,
                });
            }

            foreach (int genreId in command.Genres)
            {
                await _bookUnitOfWork.BookDataLayer.AddBookGenre(new BookGenre()
                {
                    GenreId = genreId,
                    BookId = book.BookId,
                });
            }

            await _bookUnitOfWork.Save();
            await _bookUnitOfWork.Commit();

            return response;
        }
    }
}
