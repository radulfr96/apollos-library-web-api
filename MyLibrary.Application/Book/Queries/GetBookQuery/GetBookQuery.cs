using MediatR;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Book.Queries.GetBookQuery
{
    public class GetBookQuery : IRequest<GetBookQueryDto>
    {
        public int BookId { get; set; }
    }

    public class GetBookQueryHandler : IRequestHandler<GetBookQuery, GetBookQueryDto>
    {
        public IBookUnitOfWork _bookUnitOfWork { get; set; }

        public GetBookQueryHandler(IBookUnitOfWork bookUnitOfWork)
        {
            _bookUnitOfWork = bookUnitOfWork;
        }

        public async Task<GetBookQueryDto> Handle(GetBookQuery query, CancellationToken cancellationToken)
        {
            var response = new GetBookQueryDto();

            var book = await _bookUnitOfWork.BookDataLayer.GetBook(query.BookId);

            if (book == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            response.BookID = book.BookId;
            response.CoverImage = book.CoverImage == null ? null : Encoding.ASCII.GetBytes(book.CoverImage);
            response.Edition = book.Edition;
            response.eISBN = book.EIsbn;
            response.FictionType = book.FictionTypeId;
            response.FormType = book.FormTypeId;
            response.ISBN = book.Isbn;
            response.NumberInSeries = book.NumberInSeries;
            response.PublicationFormat = book.PublicationFormatId;
            response.Publisher = book.PublisherId;
            response.SeriesID = book.SeriesId;
            response.Subtitle = book.Subtitle;
            response.Title = book.Title;
            response.Authors = book.BookAuthor.Select(ba => ba.AuthorId).ToList();
            response.Genres = book.BookGenre.Select(bg => bg.GenreId).ToList();


            return response;
        }
    }
}
