using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Series.Commands.DeleteSeriesCommand
{
    public class DeleteSeriesCommand : IRequest<DeleteSeriesCommandDto>
    {
        public int SeriesId { get; set; }
    }

    public class DeleteSeriesCommandHandler : IRequestHandler<DeleteSeriesCommand, DeleteSeriesCommandDto>
    {
        public Task<DeleteSeriesCommandDto> Handle(DeleteSeriesCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
