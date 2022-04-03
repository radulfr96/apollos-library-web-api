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

            await _bookUnitOfWork.BookDataLayer.DeleteBook(command.BookId);
            await _bookUnitOfWork.Save();

            return response;
        }
    }
}
