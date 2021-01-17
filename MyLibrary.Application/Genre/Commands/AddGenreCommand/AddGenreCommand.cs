using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Genre.Commands.AddGenreCommand
{
    public class AddGenreCommand : IRequest<AddGenreCommandDto>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must supply a genre name")]
        public string Name { get; set; }
    }

    public class AddGenreCommandHandler : IRequestHandler<AddGenreCommand, AddGenreCommandDto>
    {
        public Task<AddGenreCommandDto> Handle(AddGenreCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
