using MediatR;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Author.Commands.DeleteAuthorCommand
{
    public class DeleteAuthorCommand : IRequest<DeleteAuthorCommandDto>
    {
        public int AuthorId { get; set; }
    }

    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, DeleteAuthorCommandDto>
    {
        private readonly IAuthorUnitOfWork _authorUnitOfWork;

        public DeleteAuthorCommandHandler(IAuthorUnitOfWork authorUnitOfWork)
        {
            _authorUnitOfWork = authorUnitOfWork;
        }

        public async Task<DeleteAuthorCommandDto> Handle(DeleteAuthorCommand query, CancellationToken cancellationToken)
        {
            var response = new DeleteAuthorCommandDto();

            var author = await _authorUnitOfWork.AuthorDataLayer.GetAuthor(query.AuthorId);

            if (author == null)
            {
                throw new AuthorNotFoundException($"Unable to find author with id [{query.AuthorId}].");
            }

            await _authorUnitOfWork.Begin();

            var record = new Domain.AuthorRecord()
            {
                AuthorId = author.AuthorId,
                CountryId = author.CountryId,
                CreatedBy = author.CreatedBy,
                CreatedDate = author.CreatedDate,
                Description = author.Description,
                FirstName = author.FirstName,
                IsDeleted = true,
                LastName = author.LastName,
                MiddleName = author.MiddleName,
                ReportedVersion = false,
            };

            await _authorUnitOfWork.AuthorDataLayer.AddAuthorRecord(record);
            await _authorUnitOfWork.Save();

            author.IsDeleted = true;
            author.VersionId = record.AuthorRecordId;
            author.Books = new List<Domain.Book>();

            await _authorUnitOfWork.Save();
            await _authorUnitOfWork.Commit();

            return response;
        }
    }
}
