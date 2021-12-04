using MediatR;
using MyLibrary.Application.Common.Exceptions;
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
        private readonly IReferenceUnitOfWork _referenceUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public AddAuthorCommandHandler(IAuthorUnitOfWork authorUnitOfWork, IReferenceUnitOfWork referenceUnitOfWork, IUserService userService, IDateTimeService dateTimeService)
        {
            _authorUnitOfWork = authorUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
            _referenceUnitOfWork = referenceUnitOfWork;
        }

        public async Task<AddAuthorCommandDto> Handle(AddAuthorCommand command, CancellationToken cancellationToken)
        {
            var countries = (await _referenceUnitOfWork.ReferenceDataLayer.GetCountries()).Select(c => c.CountryId).ToList();

            if (!countries.Contains(command.CountryID))
            {
                throw new CountryInvalidValueException($"Unable to find country with code [{command.CountryID}]");
            }

            var response = new AddAuthorCommandDto();

            var author = new Persistence.Model.Author()
            {
                FirstName = command.Firstname,
                MiddleName = command.Middlename,
                LastName = command.Lastname,
                CountryId = command.CountryID,
                Description = command.Description,
                CreatedDate = _dateTimeService.Now,
                CreatedBy = _userService.GetUserID(),
            };

            await _authorUnitOfWork.AuthorDataLayer.AddAuthor(author);
            await _authorUnitOfWork.Save();

            response.AuthorId = author.AuthorId;

            return response;
        }
    }
}
