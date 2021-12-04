﻿using MediatR;
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

namespace MyLibrary.Application.Book.Commands.UpdateBookCommand
{
    public class UpdateBookCommand : IRequest<UpdateBookCommandDto>
    {
        public int BookID { get; set; }
        public string ISBN { get; set; }
        public string EISBN { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int? SeriesID { get; set; }
        public decimal? NumberInSeries { get; set; }
        public int? Edition { get; set; }
        public int PublicationFormatID { get; set; }
        public int FictionTypeID { get; set; }
        public int FormTypeID { get; set; }
        public int PublisherID { get; set; }
        public byte[] CoverImage { get; set; }
        public List<int> Genres { get; set; } = new List<int>();
        public List<int> Authors { get; set; } = new List<int>();
    }

    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, UpdateBookCommandDto>
    {
        private readonly IBookUnitOfWork _bookUnitOfWork;
        private readonly IPublisherUnitOfWork _publisherUnitOfWork;
        private readonly IGenreUnitOfWork _genreUnitOfWork;
        private readonly IAuthorUnitOfWork _authorUnitOfWork;
        private readonly IReferenceUnitOfWork _referenceUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public UpdateBookCommandHandler(
            IBookUnitOfWork bookUnitOfWork
            , IReferenceUnitOfWork referenceUnitOfWork
            , IPublisherUnitOfWork publisherUnitOfWork
            , IGenreUnitOfWork genreUnitOfWork
            , IAuthorUnitOfWork authorUnitOfWork
            , IUserService userService
            , IDateTimeService dateTimeService)
        {
            _bookUnitOfWork = bookUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
            _referenceUnitOfWork = referenceUnitOfWork;
            _genreUnitOfWork = genreUnitOfWork;
            _authorUnitOfWork = authorUnitOfWork;
            _publisherUnitOfWork = publisherUnitOfWork;
        }

        public async Task<UpdateBookCommandDto> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
        {
            var response = new UpdateBookCommandDto();

            var book = await _bookUnitOfWork.BookDataLayer.GetBook(command.BookID);

            if (book == null)
            {
                throw new BookNotFoundException($"Unable to find book with id {command.BookID}");
            }

            if (!string.IsNullOrEmpty(command.ISBN))
            {
                var existingISBN = await _bookUnitOfWork.BookDataLayer.GetBookByISBN(command.ISBN);

                if (existingISBN != null)
                {
                    throw new ISBNAlreadyAddedException("Book with that ISBN already exists.");
                }

            }

            if (!string.IsNullOrEmpty(command.EISBN))
            {
                var existingeISBN = await _bookUnitOfWork.BookDataLayer.GetBookByeISBN(command.EISBN);

                if (existingeISBN != null)
                {
                    throw new ISBNAlreadyAddedException("Book with that eISBN already exists.");
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
                throw new FormTypeNotFoundException($"Unable to find form type with id [{command.FormTypeID}]");
            }

            var publisher = await _publisherUnitOfWork.PublisherDataLayer.GetPublisher(command.PublisherID);

            if (publisher == null)
            {
                throw new PublisherNotFoundException($"Unable to find publisher with id [{command.PublisherID}]");
            }

            book.CoverImage = command.CoverImage == null ? null : Convert.ToBase64String(command.CoverImage);
            book.CreatedBy = _userService.GetUserID();
            book.CreatedDate = _dateTimeService.Now;
            book.Edition = command.Edition;
            book.EIsbn = command.EISBN;
            book.FictionTypeId = command.FictionTypeID;
            book.FormTypeId = command.FormTypeID;
            book.Isbn = command.ISBN;
            book.NumberInSeries = command.NumberInSeries;
            book.PublicationFormatId = command.PublicationFormatID;
            book.PublisherId = command.PublisherID;
            book.SeriesId = command.SeriesID;
            book.Subtitle = command.Subtitle;
            book.Title = command.Title;
            book.ModifiedBy = _userService.GetUserID();
            book.ModifiedDate = _dateTimeService.Now;

            await _bookUnitOfWork.Begin();

            _bookUnitOfWork.BookDataLayer.DeleteBookAuthorRelationships(command.BookID);
            _bookUnitOfWork.BookDataLayer.DeleteBookGenreRelationships(command.BookID);
            await _bookUnitOfWork.Save();

            foreach (int authorId in command.Authors)
            {
                var author = await _authorUnitOfWork.AuthorDataLayer.GetAuthor(authorId);

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
                var genre = await _genreUnitOfWork.GenreDataLayer.GetGenre(genreId);

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
            await _bookUnitOfWork.Commit();

            return response;
        }
    }
}
