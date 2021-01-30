using MediatR;
using MyLibrary.Application.Common.Exceptions;
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
        public string EISBN { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int? SeriesID { get; set; }
        public int? NumberInSeries { get; set; }
        public int? Edition { get; set; }
        public int PublicationFormatID { get; set; }
        public int FictionTypeID { get; set; }
        public int FormTypeID { get; set; }
        public int PublisherID { get; set; }
        public byte[] CoverImage { get; set; }
        public List<int> Genres { get; set; } = new List<int>();
        public List<int> Authors { get; set; } = new List<int>();
    }

    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, AddBookCommandDto>
    {
        private readonly IBookUnitOfWork _bookUnitOfWork;
        private readonly IReferenceUnitOfWork _referenceUnitOfWork;
        private readonly IPublisherUnitOfWork _publisherUnitOfWork;
        private readonly IAuthorUnitOfWork _authorUnitOfWork;
        private readonly IGenreUnitOfWork _genreUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public AddBookCommandHandler(
            IBookUnitOfWork bookUnitOfWork
            , IReferenceUnitOfWork referenceUnitOfWork
            , IPublisherUnitOfWork publisherUnitOfWork
            , IAuthorUnitOfWork authorUnitOfWork
            , IGenreUnitOfWork genreUnitOfWork
            , IUserService userService
            , IDateTimeService dateTimeService)
        {
            _bookUnitOfWork = bookUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
            _referenceUnitOfWork = referenceUnitOfWork;
            _publisherUnitOfWork = publisherUnitOfWork;
            _authorUnitOfWork = authorUnitOfWork;
            _genreUnitOfWork = genreUnitOfWork;
        }

        public async Task<AddBookCommandDto> Handle(AddBookCommand command, CancellationToken cancellationToken)
        {
            var response = new AddBookCommandDto();

            if (!string.IsNullOrEmpty(command.EISBN))
            {
                var existingISBN = await _bookUnitOfWork.BookDataLayer.GetBookByeISBN(command.EISBN);

                if (existingISBN != null)
                {
                    throw new ISBNAlreadyAddedException("Book with that eISBN already exists.");
                }
            }

            if (!string.IsNullOrEmpty(command.ISBN))
            {
                var existingeISBN = _bookUnitOfWork.BookDataLayer.GetBookByISBN(command.ISBN);

                if (existingeISBN != null)
                {
                    throw new ISBNAlreadyAddedException("Book with that ISBN already exists.");
                }
            }

            if (command.SeriesID.HasValue)
            {
                var series = await _bookUnitOfWork.BookDataLayer.GetSeries(command.SeriesID.Value);

                if (series == null)
                    throw new SeriesNotFoundException($"Unable to find series with id [{command.SeriesID}]");
            }

            var publicationFormat = await _referenceUnitOfWork.ReferenceDataLayer.GetPublicationFormat(command.PublicationFormatID);

            if (publicationFormat == null)
            {
                throw new PublicationFormatNotFoundException($"Unable to find publication format with id [{command.PublicationFormatID}]");
            }

            var fictionType = await _referenceUnitOfWork.ReferenceDataLayer.GetFictionType(command.FictionTypeID);

            if (fictionType == null)
            {
                throw new FictionTypeNotFoundException($"Unable to find fiction type with id [{command.FictionTypeID}]");
            }

            var formType = await _referenceUnitOfWork.ReferenceDataLayer.GetFormType(command.FormTypeID);

            if (formType == null)
            {
                throw new FictionTypeNotFoundException($"Unable to find form type with id [{command.FormTypeID}]");
            }

            var publisher = await _publisherUnitOfWork.PublisherDataLayer.GetPublisher(command.PublisherID);

            if (publisher == null)
            {
                throw new PublisherNotFoundException($"Unable to find publisher with id [{command.PublisherID}]");
            }

            var book = new Persistence.Model.Book()
            {
                CoverImage = command.CoverImage == null ? null : Convert.ToBase64String(command.CoverImage),
                CreatedBy = _userService.GetUserId(),
                CreatedDate = _dateTimeService.Now,
                Edition = command.Edition,
                EIsbn = command.EISBN,
                FictionTypeId = command.FictionTypeID,
                FormTypeId = command.FormTypeID,
                Isbn = command.ISBN,
                NumberInSeries = command.NumberInSeries,
                PublicationFormatId = command.PublicationFormatID,
                PublisherId = command.PublisherID,
                SeriesId = command.SeriesID,
                Subtitle = command.Subtitle,
                Title = command.Title,
            };

            await _bookUnitOfWork.BookDataLayer.AddBook(book);

            foreach (int authorId in command.Authors)
            {
                var author = _authorUnitOfWork.AuthorDataLayer.GetAuthor(authorId);

                if (author == null)
                {
                    throw new AuthorNotFoundException($"Unable to find author with id [{authorId}]");
                }

                await _bookUnitOfWork.BookDataLayer.AddBookAuthor(new BookAuthor()
                {
                    AuthorId = authorId,
                    BookId = book.BookId,
                });
            }

            foreach (int genreId in command.Genres)
            {
                var genre = _genreUnitOfWork.GenreDataLayer.GetGenre(genreId);

                if (genre == null)
                {
                    throw new GenreNotFoundException($"Unable to find genre with id [{genreId}]");
                }

                await _bookUnitOfWork.BookDataLayer.AddBookGenre(new BookGenre()
                {
                    GenreId = genreId,
                    BookId = book.BookId,
                });
            }

            await _bookUnitOfWork.Save();

            response.BookId = book.BookId;

            return response;
        }
    }
}
