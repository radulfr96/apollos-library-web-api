using MediatR;
using MyLibrary.Application.Interfaces;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Author.Commands.UpdateAuthorCommand
{
    public class UpdateAuthorCommand : IRequest<UpdateAuthorCommandDto>
    {
        public int AuthorID { get; set; }
        public string Firstname { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CountryID { get; set; }
        public string Description { get; set; }
    }

    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, UpdateAuthorCommandDto>
    {
        private readonly IAuthorUnitOfWork _authorUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public UpdateAuthorCommandHandler(IAuthorUnitOfWork authorUnitOfWork, IUserService userService, IDateTimeService dateTimeService)
        {
            _authorUnitOfWork = authorUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
        }

        public async Task<UpdateAuthorCommandDto> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
        {
            var response = new UpdateAuthorCommandDto();

            var author = await _authorUnitOfWork.AuthorDataLayer.GetAuthor(command.AuthorID);

            author.FirstName = command.Firstname;
            author.MiddleName = command.MiddleName;
            author.LastName = command.LastName;
            author.CountryId = command.CountryID;
            author.Description = command.Description;
            author.ModifiedBy = _userService.GetUserId();
            author.ModifiedDate = _dateTimeService.Now;

            await _authorUnitOfWork.Save();

            return response;
        }
    }
}
