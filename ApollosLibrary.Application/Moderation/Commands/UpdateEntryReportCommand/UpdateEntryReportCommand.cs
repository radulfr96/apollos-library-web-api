using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Domain.Enums;
using ApollosLibrary.UnitOfWork.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Moderation.Commands.UpdateEntryReportCommand
{
    public class UpdateEntryReportCommand : IRequest<UpdateEntryReportCommandDto>
    {
        public int EntryReportId { get; set; }
        public EntryReportStatusEnum EntryReportStatus { get; set; }
    }

    public class UpdateReportEntryCommandHandler : IRequestHandler<UpdateEntryReportCommand, UpdateEntryReportCommandDto>
    {
        private readonly IModerationUnitOfWork _moderationUnitOfWork;

        public UpdateReportEntryCommandHandler(IModerationUnitOfWork moderationUnitOfWork)
        {
            _moderationUnitOfWork = moderationUnitOfWork;
        }

        public async Task<UpdateEntryReportCommandDto> Handle(UpdateEntryReportCommand command, CancellationToken cancellationToken)
        {
            var entryReport = await _moderationUnitOfWork.ModerationDataLayer.GetEntryReport(command.EntryReportId);

            if (entryReport == null)
            {
                throw new EntryReportNotFoundException($"Unable to find report with id [{command.EntryReportId}]");
            }

            entryReport.EntryReportStatusId = (int)command.EntryReportStatus;
            await _moderationUnitOfWork.Save();

            return new UpdateEntryReportCommandDto();
        }
    }
}
