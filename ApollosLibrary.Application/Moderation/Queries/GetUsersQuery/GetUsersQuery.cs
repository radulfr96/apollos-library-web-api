using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Queries.GetUsersQuery
{
    public class GetUsersQuery : IRequest<GetUsersQueryDto>
    {
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetUsersQueryDto>
    {
        private IModerationUnitOfWork _moderationUnitOfWork;

        public GetUsersQueryHandler(IModerationUnitOfWork moderationUnitOfWork)
        {
            _moderationUnitOfWork = moderationUnitOfWork;
        }

        public async Task<GetUsersQueryDto> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return new GetUsersQueryDto()
            {
                Users = await _moderationUnitOfWork.ModerationDataLayer.GetUsers(),
            };
        }
    }
}
