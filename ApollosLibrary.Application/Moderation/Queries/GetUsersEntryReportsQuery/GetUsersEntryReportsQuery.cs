using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Queries.GetUsersEntryReportsQuery
{
    public class GetUsersEntryReportsQuery : IRequest<GetUsersEntryReportsQueryDto>
    {
        public Guid UserId { get; set; }
    }

    public class GetUsersEntryReportsQueryHandler : IRequestHandler<GetUsersEntryReportsQuery, GetUsersEntryReportsQueryDto>
    {
        public readonly IModerationUnitOfWork _moderationUnitOfWork;

        public GetUsersEntryReportsQueryHandler(IModerationUnitOfWork moderationUnitOfWork)
        {
            _moderationUnitOfWork = moderationUnitOfWork;
        }

        public async Task<GetUsersEntryReportsQueryDto> Handle(GetUsersEntryReportsQuery request, CancellationToken cancellationToken)
        {
            var reports = await _moderationUnitOfWork.ModerationDataLayer.GetReportsOfEntriesByUser(request.UserId);

            return new GetUsersEntryReportsQueryDto()
            {
                EntryReports = reports.Select(r => new EntryReportListItem()
                {
                    CreatedBy = r.CreatedBy,
                    CreatedDate = r.CreatedDate,
                    EntryId = r.EntryId,
                    EntryType = r.EntryType,
                    ReportedBy = r.ReportedBy,
                    ReportedDate = r.ReportedDate,
                    ReportId = r.EntryReportId,
                })
                .ToList(),
            };
        }
    }
}
