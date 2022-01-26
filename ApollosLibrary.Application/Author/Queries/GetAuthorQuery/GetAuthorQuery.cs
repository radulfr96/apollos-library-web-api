using MediatR;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Author.Queries.GetAuthorQuery
{
    public class GetAuthorQuery : IRequest<GetAuthorQueryDto>
    {
        public int AuthorId { get; set; }
    }

    public class GetAuthorQueryHandler : IRequestHandler<GetAuthorQuery, GetAuthorQueryDto>
    {
        private readonly IAuthorUnitOfWork _authorUnitOfWork;

        public GetAuthorQueryHandler(IAuthorUnitOfWork authorUnitOfWork)
        {
            _authorUnitOfWork = authorUnitOfWork;
        }

        public async Task<GetAuthorQueryDto> Handle(GetAuthorQuery query, CancellationToken cancellationToken)
        {
            var response = new GetAuthorQueryDto();

            var author = await _authorUnitOfWork.AuthorDataLayer.GetAuthor(query.AuthorId);

            if (author == null)
            {
                throw new AuthorNotFoundException($"Unable to find author with id [{query.AuthorId}]");
            }

            response.AuthorID = author.AuthorId;
            response.Firstname = author.FirstName;
            response.Description = author.Description;
            response.CountryID = author.CountryId;
            response.Lastname = author.LastName;
            response.Middlename = author.MiddleName;

            return response;
        }
    }
}
