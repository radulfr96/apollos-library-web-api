using ApollosLibrary.Domain.Enums;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Queries.GetEntryReportsByEntryUserQuery
{
    public class GetEntryReportsByEntryUserQuery : IRequest<GetEntryReportsByEntryUserQueryDto>
    {
        public Guid UserId { get; set; }
    }

    public class GetReportsByUserQueryHandler : IRequestHandler<GetEntryReportsByEntryUserQuery, GetEntryReportsByEntryUserQueryDto>
    {
        public readonly IModerationUnitOfWork _moderationUnitOfWork;

        public GetReportsByUserQueryHandler(IModerationUnitOfWork moderationUnitOfWork)
        {
            _moderationUnitOfWork = moderationUnitOfWork;
        }

        public async Task<GetEntryReportsByEntryUserQueryDto> Handle(GetEntryReportsByEntryUserQuery request, CancellationToken cancellationToken)
        {
            var reports = await _moderationUnitOfWork.ModerationDataLayer.GetReportsOfEntriesByUser(request.UserId);

            return new GetEntryReportsByEntryUserQueryDto()
            {
                EntryReports = reports.Select(r => new EntryReportListItem()
                {
                    CreatedBy = r.CreatedBy,
                    CreatedDate = r.CreatedDate,
                    EntryRecordId = r.EntryRecordId,
                    EntryTypeId = r.EntryTypeId,
                    EntryType = r.EntryType.Name,
                    EntryStatusId = r.EntryReportStatusId,
                    EntryStatus = r.EntryReportStatus.Name,
                    ReportedBy = r.ReportedBy,
                    ReportedDate = r.ReportedDate,
                    ReportId = r.EntryReportId,
                })
                .ToList(),
            };
        }
    }
}
