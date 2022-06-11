using ApollosLibrary.Domain.Enums;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Queries.GetReportsOfEntriesByUserQuery
{
    public class GetReportsOfEntriesByUserQuery : IRequest<GetReportsOfEntriesByUserQueryDto>
    {
        public Guid UserId { get; set; }
    }

    public class GetReportsByUserQueryHandler : IRequestHandler<GetReportsOfEntriesByUserQuery, GetReportsOfEntriesByUserQueryDto>
    {
        public readonly IModerationUnitOfWork _moderationUnitOfWork;

        public GetReportsByUserQueryHandler(IModerationUnitOfWork moderationUnitOfWork)
        {
            _moderationUnitOfWork = moderationUnitOfWork;
        }

        public async Task<GetReportsOfEntriesByUserQueryDto> Handle(GetReportsOfEntriesByUserQuery request, CancellationToken cancellationToken)
        {
            var reports = await _moderationUnitOfWork.ModerationDataLayer.GetReportsOfEntriesByUser(request.UserId);

            return new GetReportsOfEntriesByUserQueryDto()
            {
                EntryReports = reports.Select(r => new EntryReportListItem()
                {
                    CreatedBy = r.CreatedBy,
                    CreatedDate = r.CreatedDate,
                    EntryId = r.EntryId,
                    EntryTypeId = r.EntryTypeId,
                    ReportedBy = r.ReportedBy,
                    ReportedDate = r.ReportedDate,
                    ReportId = r.EntryReportId,
                })
                .ToList(),
            };
        }
    }
}
