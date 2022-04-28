using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Library.Commands.AddLibraryEntryCommand
{
    public class AddLibraryEntryCommand : IRequest<AddLibraryEntryCommandDto>
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}
