using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Domain;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Commands.AddLibraryEntryCommand
{
    public class AddLibraryEntryCommand : IRequest<AddLibraryEntryCommandDto>
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }

    public class AddLibraryEntryCommandHandler : IRequestHandler<AddLibraryEntryCommand, AddLibraryEntryCommandDto>
    {
        private readonly ILibraryUnitOfWork _libraryUnitOfWork;
        private readonly IBookUnitOfWork _bookUnitOfWork;
        private readonly IUserService _userService;

        public AddLibraryEntryCommandHandler(
            ILibraryUnitOfWork libraryUnitOfWork
            , IBookUnitOfWork bookUnitOfWork
            , IUserService userService
            )
        {
            _bookUnitOfWork = bookUnitOfWork;
            _libraryUnitOfWork = libraryUnitOfWork;
            _userService = userService;
        }

        public async Task<AddLibraryEntryCommandDto> Handle(AddLibraryEntryCommand command, CancellationToken cancellationToken)
        {
            var response = new AddLibraryEntryCommandDto();

            var libraryId = await _libraryUnitOfWork.LibraryDataLayer.GetLibraryIdByUserId(_userService.GetUserId());

            if (libraryId == null)
            {
                var library = new Domain.Library()
                {
                    UserId = _userService.GetUserId(),
                };

                await _libraryUnitOfWork.LibraryDataLayer.AddLibrary(library);
                await _libraryUnitOfWork.Save();
                libraryId = library.LibraryId;
            }

            var book = await _bookUnitOfWork.BookDataLayer.GetBook(command.BookId);

            if (book == null)
            {
                throw new BookNotFoundException($"Unable to find book with id of [{command.BookId}]");
            }

            var existingEntry = await _libraryUnitOfWork.LibraryDataLayer.GetLibraryEntry(libraryId.Value, book.BookId);

            if (existingEntry == null)
            {
                var entry = new LibraryEntry()
                {
                    BookId = command.BookId,
                    LibraryId = libraryId.Value,
                    Quantity = command.Quantity,
                };

                await _libraryUnitOfWork.LibraryDataLayer.AddLibraryEntry(entry);
                await _libraryUnitOfWork.Save();

                response.LibraryEntryId = entry.EntryId;
            }
            else
            {
                existingEntry.Quantity = command.Quantity;
                response.LibraryEntryId = existingEntry.EntryId;
                await _libraryUnitOfWork.Save();
            }

            return response;
        }
    }
}
