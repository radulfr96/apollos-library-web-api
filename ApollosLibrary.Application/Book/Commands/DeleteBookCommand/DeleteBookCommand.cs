using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Book.Commands.DeleteBookCommand
{
    public class DeleteBookCommand : IRequest<DeleteBookCommandDto>
    {
        public int BookId { get; set; }
    }

    public class DeleteGenreCommandHandler : IRequestHandler<DeleteBookCommand, DeleteBookCommandDto>
    {
        private readonly IBookUnitOfWork _bookUnitOfWork;

        public DeleteGenreCommandHandler(IBookUnitOfWork bookUnitOfWork)
        {
            _bookUnitOfWork = bookUnitOfWork;
        }

        public async Task<DeleteBookCommandDto> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
        {
            var response = new DeleteBookCommandDto();

            var book = await _bookUnitOfWork.BookDataLayer.GetBook(command.BookId);
            if (book == null)
            {
                throw new BookNotFoundException($"Unable to find book with id {command.BookId}");
            }

            book.IsDeleted = true;
            book.Authors = new List<Domain.Author>();
            book.Genres = new List<Domain.Genre>();
            book.Series = new List<Domain.Series>();

            await _bookUnitOfWork.BookDataLayer.AddBookRecord(new Domain.BookRecord()
            {
                BookId = command.BookId,
                BusinessId = book.BusinessId,
                CoverImage = book.CoverImage,
                CreatedDate = book.CreatedDate,
                CreatedBy = book.CreatedBy,
                Edition = book.Edition,
                EIsbn = book.EIsbn,
                FictionTypeId = book.FictionTypeId,
                FormTypeId = book.FormTypeId,
                Isbn = book.Isbn,
                IsDeleted = true,
                PublicationFormatId = book.PublicationFormatId,
                Subtitle = book.Subtitle,
                Title = book.Title,
            });

            await _bookUnitOfWork.Save();

            return response;
        }
    }
}
