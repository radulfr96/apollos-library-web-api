using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Queries.GetLibraryEntryQuery
{
    public class GetLibraryEntryQuery : IRequest<GetLibraryEntryQueryDto>
    {
        public int EntryId { get; set; }
    }

    public class GetLibraryEntryQueryHandler : IRequestHandler<GetLibraryEntryQuery, GetLibraryEntryQueryDto>
    {

        private readonly ILibraryUnitOfWork _libraryUnitOfWork;
        private readonly IUserService _userService;

        public GetLibraryEntryQueryHandler(ILibraryUnitOfWork libraryUnitOfWork, IUserService userService)
        {
            _libraryUnitOfWork = libraryUnitOfWork;
            _userService = userService;
        }

        public async Task<GetLibraryEntryQueryDto> Handle(GetLibraryEntryQuery query, CancellationToken cancellationToken)
        {
            var libraryId = await _libraryUnitOfWork.LibraryDataLayer.GetLibraryIdByUserId(_userService.GetUserId());

            var entry = await _libraryUnitOfWork.LibraryDataLayer.GetLibraryEntry(query.EntryId);

            if (entry == null)
            {
                throw new LibraryEntryNotFoundException($"Unable to find entry with id [{query.EntryId}]");
            }

            if (entry.LibraryId != libraryId)
            {
                throw new UserCannotModifyLibraryException($"You are not authorized to view entry with id [{query.EntryId}]");
            }

            return new GetLibraryEntryQueryDto()
            {
                BookId = entry.BookId,
                EntryId = entry.EntryId,
                Quantity = entry.Quantity,
            };
        }
    }
}
