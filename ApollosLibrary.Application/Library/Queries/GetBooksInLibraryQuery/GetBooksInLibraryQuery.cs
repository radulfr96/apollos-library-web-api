using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Queries.GetBooksInLibraryQuery
{
    public class GetBooksInLibraryQuery : IRequest<GetBooksLibraryInQueryDto>
    {
    }

    public class GetBooksInLibraryQueryHandler : IRequestHandler<GetBooksInLibraryQuery, GetBooksLibraryInQueryDto>
    {
        private readonly IUserService _userService;
        private readonly ILibraryUnitOfWork _libraryUnitOfWork;
        private readonly IBookUnitOfWork _bookUnitOfWork;

        public GetBooksInLibraryQueryHandler(
            IUserService userService
            , IBookUnitOfWork bookUnitOfWork
            , ILibraryUnitOfWork libraryUnitOfWork
            )
        {
            _userService = userService;
            _bookUnitOfWork = bookUnitOfWork;
            _libraryUnitOfWork = libraryUnitOfWork;
        }

        public async Task<GetBooksLibraryInQueryDto> Handle(GetBooksInLibraryQuery query, CancellationToken cancellationToken)
        {
            var userId = _userService.GetUserId();
            var library = await _libraryUnitOfWork.LibraryDataLayer.GetLibraryIdByUserId(userId);

            if (!library.HasValue)
            {
                var newLibrary = new Domain.Library()
                {
                    UserId = userId,
                };

                await _libraryUnitOfWork.LibraryDataLayer.AddLibrary(newLibrary);
                await _libraryUnitOfWork.Save();

                library = newLibrary.LibraryId;
            }

            var books = await _libraryUnitOfWork.LibraryDataLayer.GetLibraryBooks(library.Value);

            return new GetBooksLibraryInQueryDto()
            {
                LibraryBooks = books.Select(b => new LibraryBook()
                {
                    BookId = b.BookId,
                    eISBN = b.EIsbn,
                    ISBN = b.Isbn,
                    Title = b.Title,
                    Author = b.Authors.Count > 1
                                            ? $"{GetAuthorCredit(b.Authors.First())} et al."
                                            : $"{GetAuthorCredit(b.Authors.First())}",
                }).ToList(),
            };
        }

        private string GetAuthorCredit(Domain.Author author)
        {
            return string.IsNullOrEmpty(author.LastName) ? author.FirstName : author.LastName;
        }
    }
}
