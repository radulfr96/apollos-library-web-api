using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Book.Queries.GetBookRecordQuery
{
    public class GetBookRecordQuery : IRequest<GetBookRecordQueryDto>
    {
        public int BookRecordId { get; set; }
    }

    public class GetBookByRecordIdQueryHandler : IRequestHandler<GetBookRecordQuery, GetBookRecordQueryDto>
    {
        private readonly IBookUnitOfWork _bookUnitOfWork;

        public GetBookByRecordIdQueryHandler(IBookUnitOfWork bookUnitOfWork)
        {
            _bookUnitOfWork = bookUnitOfWork;
        }

        public async Task<GetBookRecordQueryDto> Handle(GetBookRecordQuery request, CancellationToken cancellationToken)
        {
            var bookRecord = await _bookUnitOfWork.BookDataLayer.GetBookRecord(request.BookRecordId);

            if (bookRecord == null)
            {
                throw new BookRecordNotFoundException($"Unable to find book record with id of [{request.BookRecordId}]");
            }

            return new GetBookRecordQueryDto()
            {
                BookId = bookRecord.BookId,
                CoverImage = bookRecord.CoverImage,
                EISBN = bookRecord.EIsbn,
                ISBN = bookRecord.Isbn,
                Subtitle = bookRecord.Subtitle,
                Title = bookRecord.Title,
            };
        }
    }
}
