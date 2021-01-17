using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Genre.Commands.UpdateGenreCommand
{
    public class UpdateGenreCommand : IRequest<UpdateGenreCommandDto>
    {
        public int GenreID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must provide a genre name")]
        public string Name { get; set; }
    }

    public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, UpdateGenreCommandDto>
    {
        public Task<UpdateGenreCommandDto> Handle(UpdateGenreCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
