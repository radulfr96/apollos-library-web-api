using MediatR;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Author.Commands.DeleteAuthorCommand
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

            var author = _authorUnitOfWork.AuthorDataLayer.GetAuthor(query.AuthorId);

            await _authorUnitOfWork.AuthorDataLayer.DeleteAuthor(query.AuthorId);
            await _authorUnitOfWork.Save();

            return response;
        }
    }
}
