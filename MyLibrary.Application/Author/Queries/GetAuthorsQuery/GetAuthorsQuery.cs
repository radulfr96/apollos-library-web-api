using MediatR;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Author.Queries.GetAuthorsQuery
{
    public class GetAuthorsQuery : IRequest<GetAuthorsQueryDto>
    {
    }

    public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, GetAuthorsQueryDto>
    {
        private readonly IAuthorUnitOfWork _authorUnitOfWork;

        public GetAuthorsQueryHandler(IAuthorUnitOfWork authorUnitOfWork)
        {
            _authorUnitOfWork = authorUnitOfWork;
        }

        public async Task<GetAuthorsQueryDto> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            var response = new GetAuthorsQueryDto();

            var authors = await _authorUnitOfWork.AuthorDataLayer.GetAuthors();

            response.Authors = authors.Select(a => new AuthorListItemDTO()
            {
                AuthorId = a.AuthorId,
                Country = a.Country.Name,
                Name = ($"{a.FirstName} {a.MiddleName} {a.LastName}").Trim()
            }).ToList();

            return response;
        }
    }
}
