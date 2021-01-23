using MediatR;
using MyLibrary.Application.Interfaces;
using MyLibrary.Persistence.Model;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Author.Commands.AddAuthorCommand
{
    public class AddAuthorCommand : IRequest<AddAuthorCommandDto>
    {
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string CountryID { get; set; }
        public string Description { get; set; }
    }

    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, AddAuthorCommandDto>
    {
        private readonly IAuthorUnitOfWork _authorUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public AddAuthorCommandHandler(IAuthorUnitOfWork authorUnitOfWork, IUserService userService, IDateTimeService dateTimeService)
        {
            _authorUnitOfWork = authorUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
        }

        public async Task<AddAuthorCommandDto> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            var response = new AddAuthorCommandDto();

            var author = new Persistence.Model.Author()
            {
                FirstName = request.Firstname,
                MiddleName = request.Middlename,
                LastName = request.Lastname,
                CountryId = request.CountryID,
                Description = request.Description,
                CreatedDate = _dateTimeService.Now,
                CreatedBy = _userService.GetUserId(),
            };

            await _authorUnitOfWork.AuthorDataLayer.AddAuthor(author);
            await _authorUnitOfWork.Save();

            response.AuthorID = author.AuthorId;

            return response;
        }
    }
}
