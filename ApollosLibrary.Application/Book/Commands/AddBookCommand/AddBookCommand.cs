﻿using MediatR;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApollosLibrary.Domain.Enums;

namespace ApollosLibrary.Application.Book.Commands.AddBookCommand
{
    public class AddBookCommand : IRequest<AddBookCommandDto>
    {
        public string ISBN { get; set; }
        public string EISBN { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int? Edition { get; set; }
        public int PublicationFormatId { get; set; }
        public int FictionTypeId { get; set; }
        public int FormTypeId { get; set; }
        public int? BusinessId { get; set; }
        public string CoverImage { get; set; }
        public List<int> Genres { get; set; } = new List<int>();
        public List<int> Authors { get; set; } = new List<int>();
        public List<int> Series { get; set; } = new List<int>();
    }

    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, AddBookCommandDto>
    {
        private readonly IBookUnitOfWork _bookUnitOfWork;
        private readonly IReferenceUnitOfWork _referenceUnitOfWork;
        private readonly IBusinessUnitOfWork _businessUnitOfWork;
        private readonly IAuthorUnitOfWork _authorUnitOfWork;
        private readonly IGenreUnitOfWork _genreUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;
        private readonly ISeriesUnitOfWork _seriesUnitOfWork;

        public AddBookCommandHandler(
            IBookUnitOfWork bookUnitOfWork
            , IReferenceUnitOfWork referenceUnitOfWork
            , IBusinessUnitOfWork businessUnitOfWork
            , IAuthorUnitOfWork authorUnitOfWork
            , IGenreUnitOfWork genreUnitOfWork
            , IUserService userService
            , IDateTimeService dateTimeService
            , ISeriesUnitOfWork seriesUnitOfWork)
        {
            _bookUnitOfWork = bookUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
            _referenceUnitOfWork = referenceUnitOfWork;
            _businessUnitOfWork = businessUnitOfWork;
            _authorUnitOfWork = authorUnitOfWork;
            _genreUnitOfWork = genreUnitOfWork;
            _seriesUnitOfWork = seriesUnitOfWork;
        }

        public async Task<AddBookCommandDto> Handle(AddBookCommand command, CancellationToken cancellationToken)
        {
            var response = new AddBookCommandDto();

            Domain.Book book = null;

            if (!string.IsNullOrEmpty(command.EISBN))
            {
                book = await _bookUnitOfWork.BookDataLayer.GetBookByeISBN(command.EISBN);
            }

            if (!string.IsNullOrEmpty(command.ISBN) && book == null)
            {
                book = await _bookUnitOfWork.BookDataLayer.GetBookByISBN(command.ISBN);
            }

            var publicationFormat = await _referenceUnitOfWork.ReferenceDataLayer.GetPublicationFormat(command.PublicationFormatId);

            if (publicationFormat == null)
            {
                throw new PublicationFormatNotFoundException($"Unable to find publication format with id [{command.PublicationFormatId}]");
            }

            var fictionType = await _referenceUnitOfWork.ReferenceDataLayer.GetFictionType(command.FictionTypeId);

            if (fictionType == null)
            {
                throw new FictionTypeNotFoundException($"Unable to find fiction type with id [{command.FictionTypeId}]");
            }

            var formType = await _referenceUnitOfWork.ReferenceDataLayer.GetFormType(command.FormTypeId);

            if (formType == null)
            {
                throw new FormTypeNotFoundException($"Unable to find form type with id [{command.FormTypeId}]");
            }

            Domain.Business business;

            if (command.BusinessId.HasValue)
            {
                business = await _businessUnitOfWork.BusinessDataLayer.GetBusiness(command.BusinessId.Value);

                if (business == null)
                {
                    throw new BusinessNotFoundException($"Unable to find Business with id [{command.BusinessId}]");
                }
                else if (business.BusinessTypeId != (int)BusinessTypeEnum.Publisher)
                {
                    throw new BusinessIsNotPublisherException($"Business with id of [{command.BusinessId}] is not a publisher");
                }
            }

            await _bookUnitOfWork.Begin();

            if (book == null)
            {
                book = new Domain.Book()
                {
                    CoverImage = command.CoverImage ?? null,
                    CreatedBy = _userService.GetUserId(),
                    CreatedDate = _dateTimeService.Now,
                    Edition = command.Edition,
                    EIsbn = command.EISBN,
                    FictionTypeId = command.FictionTypeId,
                    FormTypeId = command.FormTypeId,
                    Isbn = command.ISBN,
                    PublicationFormatId = command.PublicationFormatId,
                    BusinessId = command.BusinessId,
                    Subtitle = command.Subtitle,
                    Title = command.Title,
                    Authors = new List<Domain.Author>()
                };

                await _bookUnitOfWork.BookDataLayer.AddBook(book);

                await _bookUnitOfWork.Save();

                foreach (int authorId in command.Authors)
                {
                    var author = await _authorUnitOfWork.AuthorDataLayer.GetAuthor(authorId);

                    if (author == null)
                    {
                        throw new AuthorNotFoundException($"Unable to find author with id [{authorId}]");
                    }

                    author.Books.Add(book);
                }

                foreach (int genreId in command.Genres)
                {
                    var genre = await _genreUnitOfWork.GenreDataLayer.GetGenre(genreId);

                    if (genre == null)
                    {
                        throw new GenreNotFoundException($"Unable to find genre with id [{genreId}]");
                    }

                    genre.Books.Add(book);
                }

                foreach (int seriesId in command.Series)
                {
                    var series = await _seriesUnitOfWork.SeriesDataLayer.GetSeries(seriesId);

                    if (series == null)
                    {
                        throw new SeriesNotFoundException($"Unable to find series with id [{seriesId}]");
                    }

                    series.Books.Add(book);
                }

                await _bookUnitOfWork.Save();
            }
            else
            {
                await _bookUnitOfWork.BookDataLayer.DeleteBookAuthorRelationships(book.BookId);
                await _bookUnitOfWork.BookDataLayer.DeleteBookGenreRelationships(book.BookId);
                await _bookUnitOfWork.BookDataLayer.DeleteBookSeriesRelationships(book.BookId);
                await _bookUnitOfWork.Save();

                foreach (int authorId in command.Authors)
                {
                    var author = await _authorUnitOfWork.AuthorDataLayer.GetAuthor(authorId);

                    if (author == null)
                    {
                        throw new AuthorNotFoundException($"Unable to find author with id [{authorId}]");
                    }

                    author.Books.Add(book);
                }

                foreach (int genreId in command.Genres)
                {
                    var genre = await _genreUnitOfWork.GenreDataLayer.GetGenre(genreId);

                    if (genre == null)
                    {
                        throw new GenreNotFoundException($"Unable to find genre with id [{genreId}]");
                    }

                    genre.Books.Add(book);
                }

                foreach (int seriesId in command.Series)
                {
                    var series = await _seriesUnitOfWork.SeriesDataLayer.GetSeries(seriesId);

                    if (series == null)
                    {
                        throw new SeriesNotFoundException($"Unable to find series with id [{seriesId}]");
                    }

                    series.Books.Add(book);
                }

                book.CoverImage = command.CoverImage;
                book.CreatedBy = _userService.GetUserId();
                book.CreatedDate = _dateTimeService.Now;
                book.Edition = command.Edition;
                book.EIsbn = command.EISBN;
                book.FictionTypeId = command.FictionTypeId;
                book.FormTypeId = command.FormTypeId;
                book.Isbn = command.ISBN;
                book.PublicationFormatId = command.PublicationFormatId;
                book.BusinessId = command.BusinessId;
                book.Subtitle = command.Subtitle;
                book.Title = command.Title;
                book.IsDeleted = false;
            }

            var bookRecord = new Domain.BookRecord()
            {
                BookId = book.BookId,
                BusinessId = book.BusinessId,
                CoverImage = book.CoverImage,
                CreatedBy = book.CreatedBy,
                CreatedDate = book.CreatedDate,
                Edition = book.Edition,
                EIsbn = book.EIsbn,
                FictionTypeId = book.FictionTypeId,
                FormTypeId = book.FormTypeId,
                Isbn = book.Isbn,
                PublicationFormatId = book.PublicationFormatId,
                Subtitle = book.Subtitle,
                Title = book.Title,
                IsDeleted = false,
            };

            await _bookUnitOfWork.BookDataLayer.AddBookRecord(bookRecord);
            await _bookUnitOfWork.Save();

            book.VersionId = bookRecord.BookRecordId;

            await _bookUnitOfWork.Save();
            await _bookUnitOfWork.Commit();

            response.BookId = book.BookId;


            return response;
        }
    }
}
