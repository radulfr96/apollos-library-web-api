using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Queries.GetEntryReportQuery
{
    public class GetEntryReportQuery : IRequest<GetEntryReportQueryDto>
    {
        public int ReportEntryId { get; set; }
    }

    public class GetEntryReportQueryHandler : IRequestHandler<GetEntryReportQuery, GetEntryReportQueryDto>
    {
        private readonly IModerationUnitOfWork _moderationUnitOfWork;

        public GetEntryReportQueryHandler(IModerationUnitOfWork moderationUnitOfWork)
        {
            _moderationUnitOfWork = moderationUnitOfWork;
        }

        public async Task<GetEntryReportQueryDto> Handle(GetEntryReportQuery request, CancellationToken cancellationToken)
        {
            var report = await _moderationUnitOfWork.ModerationDataLayer.GetEntryReport(request.ReportEntryId);
        
            if (report == null)
            {
                throw new EntryReportNotFoundException($"Unable to find entry report with id [{request.ReportEntryId}]");
            }

            return new GetEntryReportQueryDto()
            {
                CreatedDate = report.CreatedDate,
                CreatedBy = report.CreatedBy,
                EntryId = report.EntryId,
                EntryType = report.EntryType,
                ReportedBy = report.ReportedBy,
                ReportedDate = report.ReportedDate,
            };
        }
    }
}
