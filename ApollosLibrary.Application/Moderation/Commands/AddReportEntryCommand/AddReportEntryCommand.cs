using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Domain.Enums;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Commands.AddReportEntryCommand
{
    public class AddReportEntryCommand : IRequest<AddReportEntryCommandDto>
    {
        public int EntryId { get; set; }
        public EntryTypeEnum EntryType { get; set; }
        public Guid CreatedBy { get; set; }
    }

    public class ReportEntryCommandHandler : IRequestHandler<AddReportEntryCommand, AddReportEntryCommandDto>
    {
        private readonly IModerationUnitOfWork _moderationUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public ReportEntryCommandHandler(
            IModerationUnitOfWork moderationUnitOfWork
            , IUserService userService
            , IDateTimeService dateTimeService)
        {
            _moderationUnitOfWork = moderationUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
        }

        public async Task<AddReportEntryCommandDto> Handle(AddReportEntryCommand request, CancellationToken cancellationToken)
        {
            var reportEntry = new Domain.EntryReport()
            {
                CreatedBy = request.CreatedBy,
                EntryId = request.EntryId,
                EntryType = request.EntryType,
                ReportedBy = _userService.GetUserId(),
                ReportedDate = _dateTimeService.Now,
            };

            await _moderationUnitOfWork.ModerationDataLayer.AddEntryReport(reportEntry);
            await _moderationUnitOfWork.Save();

            return new AddReportEntryCommandDto()
            {
                ReportEntryId = reportEntry.EntryReportId,
            };
        }
    }
}
