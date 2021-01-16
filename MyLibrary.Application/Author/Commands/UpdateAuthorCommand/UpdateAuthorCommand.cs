using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Author.Commands.UpdateAuthorCommand
{
    public class UpdateAuthorCommand : IRequest<UpdateAuthorCommandDto>
    {
        public int AuthorID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must supply a first name or an alias")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must select a country of origin")]
        public string CountryID { get; set; }
        public string Description { get; set; }
    }

    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, UpdateAuthorCommandDto>
    {
        public Task<UpdateAuthorCommandDto> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
