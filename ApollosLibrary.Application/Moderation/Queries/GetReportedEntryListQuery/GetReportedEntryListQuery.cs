using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Queries.GetReportedEntryListQuery
{
    public class GetReportedEntryListQuery : IRequest<GetReportedEntryListQueryDto>
    {
    }

    public class GetReportedEntryListQueryHandler : IRequestHandler<GetReportedEntryListQuery, GetReportedEntryListQueryDto>
    {
        private readonly IModerationUnitOfWork _moderationUnitOfWork;

        public GetReportedEntryListQueryHandler(IModerationUnitOfWork moderationUnitOfWork)
        {
            _moderationUnitOfWork = moderationUnitOfWork;
        }

        public async Task<GetReportedEntryListQueryDto> Handle(GetReportedEntryListQuery request, CancellationToken cancellationToken)
        {
            var reports = await _moderationUnitOfWork.ModerationDataLayer.GetEntryReports();

            return new GetReportedEntryListQueryDto()
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
