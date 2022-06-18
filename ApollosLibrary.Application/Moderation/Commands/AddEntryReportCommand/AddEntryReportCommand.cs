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

namespace ApollosLibrary.Application.Moderation.Commands.AddEntryReportCommand
{
    public class AddEntryReportCommand : IRequest<AddEntryReportCommandDto>
    {
        public int EntryRecordId { get; set; }
        public EntryTypeEnum EntryType { get; set; }
        public Guid CreatedBy { get; set; }
    }

    public class ReportEntryCommandHandler : IRequestHandler<AddEntryReportCommand, AddEntryReportCommandDto>
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

        public async Task<AddEntryReportCommandDto> Handle(AddEntryReportCommand request, CancellationToken cancellationToken)
        {
            var reportEntry = new Domain.EntryReport()
            {
                CreatedBy = request.CreatedBy,
                EntryRecordId = request.EntryRecordId,
                EntryTypeId = (int)request.EntryType,
                EntryReportStatusId = (int)EntryReportStatusEnum.Open,
                ReportedBy = _userService.GetUserId(),
                ReportedDate = _dateTimeService.Now,
            };

            await _moderationUnitOfWork.ModerationDataLayer.AddEntryReport(reportEntry);
            await _moderationUnitOfWork.Save();

            return new AddEntryReportCommandDto()
            {
                ReportEntryId = reportEntry.EntryReportId,
            };
        }
    }
}
