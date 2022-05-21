using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Queries.GetLibraryEntriesQuery
{
    public class GetLibraryEntriesQuery : IRequest<GetLibraryEntriesQueryDto>
    {
    }

    public class GetLibraryEntriesQueryHandler : IRequestHandler<GetLibraryEntriesQuery, GetLibraryEntriesQueryDto>
    {
        private readonly IUserService _userService;
        private readonly ILibraryUnitOfWork _libraryUnitOfWork;

        public GetLibraryEntriesQueryHandler(IUserService userService, ILibraryUnitOfWork libraryUnitOfWork)
        {
            _userService = userService;
            _libraryUnitOfWork = libraryUnitOfWork;
        }

        public async Task<GetLibraryEntriesQueryDto> Handle(GetLibraryEntriesQuery request, CancellationToken cancellationToken)
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

            var response = new GetLibraryEntriesQueryDto
            {
                LibraryEntries = (await _libraryUnitOfWork.LibraryDataLayer.GetLibraryEntriesByUserId(_userService.GetUserId()))
                                .Select(e => new LibraryEntryListItemDTO()
                                {
                                    EntryId = e.EntryId,
                                    Title = e.Book.Title,
                                    Quantity = e.Quantity,
                                    Author = e.Book.Authors.Count > 1
                                            ? $"{GetAuthorCredit(e.Book.Authors.First())} et al."
                                            : $"{GetAuthorCredit(e.Book.Authors.First())}",

                                }).ToList()
            };

            return response;
        }

        private string GetAuthorCredit(Domain.Author author)
        {
            return string.IsNullOrEmpty(author.LastName) ? author.FirstName : author.LastName;
        }
    }
}
