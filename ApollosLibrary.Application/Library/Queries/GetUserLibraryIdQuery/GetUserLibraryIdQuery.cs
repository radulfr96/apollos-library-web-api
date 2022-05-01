using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Queries.GetUserLibraryIdQuery
{
    public class GetUserLibraryIdQuery : IRequest<GetUserLibraryIdQueryDto>
    {
    }

    public class GetUserLibraryIdQueryHandler : IRequestHandler<GetUserLibraryIdQuery,GetUserLibraryIdQueryDto>
    {
        private readonly ILibraryUnitOfWork _libraryUnitOfWork;
        private readonly IUserService _userService;

        public GetUserLibraryIdQueryHandler(ILibraryUnitOfWork libraryUnitOfWork, IUserService userService)
        {
            _libraryUnitOfWork = libraryUnitOfWork;
            _userService = userService;
        }

        public async Task<GetUserLibraryIdQueryDto> Handle(GetUserLibraryIdQuery request, CancellationToken cancellationToken)
        {
            var response = new GetUserLibraryIdQueryDto();

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

                response.LibraryId = newLibrary.LibraryId;

                return response;
            }

            response.LibraryId = library.Value;
            return response;
        }
    }
}
