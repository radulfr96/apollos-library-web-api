using ApollosLibrary.Domain.Enums;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Queries.GetEntryReportsQuery
{
    public class GetEntryReportsQuery : IRequest<GetEntryReportsQueryDto>
    {
    }

    public class GetReportedEntryListQueryHandler : IRequestHandler<GetEntryReportsQuery, GetEntryReportsQueryDto>
    {
        private readonly IModerationUnitOfWork _moderationUnitOfWork;

        public GetReportedEntryListQueryHandler(IModerationUnitOfWork moderationUnitOfWork)
        {
            _moderationUnitOfWork = moderationUnitOfWork;
        }

        public async Task<GetEntryReportsQueryDto> Handle(GetEntryReportsQuery request, CancellationToken cancellationToken)
        {
            var reports = await _moderationUnitOfWork.ModerationDataLayer.GetEntryReports();

            return new GetEntryReportsQueryDto()
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
