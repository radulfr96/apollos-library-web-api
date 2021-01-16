using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Author.Commands
{
    public class AddAuthorCommand : IRequest<AddAuthorCommandDto>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must supply a first name or an alias")]
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "You must select a country of origin")]
        public string CountryID { get; set; }
        public string Description { get; set; }
    }

    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, AddAuthorCommandDto>
    {
        public Task<AddAuthorCommandDto> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
