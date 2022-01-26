using MediatR;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApollosLibrary.Application.Author.Commands.UpdateAuthorCommand
{
    public class UpdateAuthorCommand : IRequest<UpdateAuthorCommandDto>
    {
        public int AuthorId { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string CountryID { get; set; }
        public string Description { get; set; }
    }

    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, UpdateAuthorCommandDto>
    {
        private readonly IAuthorUnitOfWork _authorUnitOfWork;
        private readonly IReferenceUnitOfWork _referenceUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public UpdateAuthorCommandHandler(IAuthorUnitOfWork authorUnitOfWork, IReferenceUnitOfWork referenceUnitOfWork, IUserService userService, IDateTimeService dateTimeService)
        {
            _authorUnitOfWork = authorUnitOfWork;
            _referenceUnitOfWork = referenceUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
        }

        public async Task<UpdateAuthorCommandDto> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
        {
            var response = new UpdateAuthorCommandDto();

            var author = await _authorUnitOfWork.AuthorDataLayer.GetAuthor(command.AuthorId);

            if (author == null)
            {
                throw new AuthorNotFoundException($"Unable to find book with id [{command.AuthorId}]");
            }

            var countries = (await _referenceUnitOfWork.ReferenceDataLayer.GetCountries()).Select(c => c.CountryId).ToList();

            if (!countries.Contains(command.CountryID))
            {
                throw new CountryInvalidValueException($"Unable to find country with code [{command.CountryID}]");
            }

            author.FirstName = command.Firstname;
            author.MiddleName = command.Middlename;
            author.LastName = command.Lastname;
            author.CountryId = command.CountryID;
            author.Description = command.Description;
            author.ModifiedBy = _userService.GetUserId();
            author.ModifiedDate = _dateTimeService.Now;

            await _authorUnitOfWork.Save();

            return response;
        }
    }
}
