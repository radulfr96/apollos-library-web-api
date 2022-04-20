using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Commands.CreateLibraryCommand
{
    public class CreateLibraryCommand : IRequest<CreateLibraryCommandDto>
    {
        public Guid UserId { get; set; }
    }

    public class CreateLibraryCommandHandler : IRequestHandler<CreateLibraryCommand, CreateLibraryCommandDto>
    {
        public CreateLibraryCommandHandler()
        {

        }

        public Task<CreateLibraryCommandDto> Handle(CreateLibraryCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
