using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Author.Queries.GetAuthorRecordQuery
{
    public class GetAuthorRecordQuery : IRequest<GetAuthorRecordQueryDto>
    {
        public int AuthorRecordId { get; set; }
    }

    public class GetAuthorRecordQueryHandler : IRequestHandler<GetAuthorRecordQuery, GetAuthorRecordQueryDto>
    {
        private readonly IAuthorUnitOfWork _authorUnitOfWork;

        public GetAuthorRecordQueryHandler(IAuthorUnitOfWork authorUnitOfWork)
        {
            _authorUnitOfWork = authorUnitOfWork;
        }

        public async Task<GetAuthorRecordQueryDto> Handle(GetAuthorRecordQuery request, CancellationToken cancellationToken)
        {
            var authorRecord = await _authorUnitOfWork.AuthorDataLayer.GetAuthorRecord(request.AuthorRecordId);

            if (authorRecord == null)
            {
                throw new AuthorRecordNotFoundException($"Unable to find author record with id of [{request.AuthorRecordId}]");
            }

            return new GetAuthorRecordQueryDto()
            {
                AuthorId = authorRecord.AuthorId,
                Description = authorRecord.Description,
                FirstName = authorRecord.FirstName,
                LastName = authorRecord.LastName,
                MiddleName = authorRecord.MiddleName,
            };
        }
    }
}
