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

namespace MyLibrary.Application.Book.Commands.AddBookCommand
{
    public class AddBookCommand : IRequest<AddBookCommandDto>
    {
        public string ISBN { get; set; }
        public string eISBN { get; set; }
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
    }

    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, AddBookCommandDto>
    {
        private readonly IBookUnitOfWork _bookUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public AddBookCommandHandler(IBookUnitOfWork bookUnitOfWork, IUserService userService, IDateTimeService dateTimeService)
        {
            _bookUnitOfWork = bookUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
        }

        public async Task<AddBookCommandDto> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var response = new AddBookCommandDto();

            if (!string.IsNullOrEmpty(request.eISBN))
            {
                var existingISBN = await _bookUnitOfWork.BookDataLayer.GetBookByeISBN(request.eISBN);

                if (existingISBN != null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Messages.Add("Book with that eISBN already exists.");
                    return response;
                }

            }

            if (!string.IsNullOrEmpty(request.ISBN))
            {
                var existingeISBN = _bookUnitOfWork.BookDataLayer.GetBookByISBN(request.ISBN);

                if (existingeISBN != null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Messages.Add("Book with that ISBN already exists.");
                    return response;
                }

            }

            var book = new Persistence.Model.Book()
            {
                CoverImage = request.CoverImage == null ? null : Convert.ToBase64String(request.CoverImage),
                CreatedBy = _userService.GetUserId(),
                CreatedDate = _dateTimeService.Now,
                Edition = request.Edition,
                EIsbn = request.eISBN,
                FictionTypeId = request.FictionTypeID,
                FormTypeId = request.FormTypeID,
                Isbn = request.ISBN,
                NumberInSeries = request.NumberInSeries,
                PublicationFormatId = request.PublicationFormatID,
                PublisherId = request.PublisherIDs,
                SeriesId = request.SeriesID,
                Subtitle = request.Subtitle,
                Title = request.Title,
            };

            await _bookUnitOfWork.Begin();

            await _bookUnitOfWork.BookDataLayer.AddBook(book);

            foreach (int authorId in request.Authors)
            {
                await _bookUnitOfWork.BookDataLayer.AddBookAuthor(new BookAuthor()
                {
                    AuthorId = authorId,
                    BookId = book.BookId,
                });
            }

            foreach (int genreId in request.Genres)
            {
                await _bookUnitOfWork.BookDataLayer.AddBookGenre(new BookGenre()
                {
                    GenreId = genreId,
                    BookId = book.BookId,
                });
            }

            await _bookUnitOfWork.Save();
            await _bookUnitOfWork.Commit();

            response.BookId = book.BookId;

            return response;
        }
    }
}
