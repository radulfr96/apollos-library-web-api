using MediatR;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Book.Queries.GetBooksQuery
{
    public class GetBooksQuery : IRequest<GetBooksQueryDto> { }

    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, GetBooksQueryDto>
    {
        private readonly IBookUnitOfWork _bookUnitOfWork;

        public GetBooksQueryHandler(IBookUnitOfWork bookUnitOfWork)
        {
            _bookUnitOfWork = bookUnitOfWork;
        }

        public async Task<GetBooksQueryDto> Handle(GetBooksQuery query, CancellationToken cancellationToken)
        {
            var response = new GetBooksQueryDto();

            var books = await _bookUnitOfWork.BookDataLayer.GetBooks();

            if (books == null || books.Count == 0)
            {
                return response;
            }

            response.Books = books.Select(b => new BookListItemDTO()
            {
                BookID = b.BookId,
                eISBN = b.EIsbn,
                FictionType = b.FictionType.Name,
                ISBN = b.Isbn,
                FormatType = b.FormType.Name,
                Title = b.Title
            }).ToList();

            return response;
        }
    }
}
