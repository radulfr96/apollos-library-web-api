using MediatR;
using MyLibrary.UnitOfWork.Contracts;
using MyLibrary.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyLibrary.Application.Interfaces;

namespace MyLibrary.Application.Genre.Commands.AddGenreCommand
{
    public class AddGenreCommand : IRequest<AddGenreCommandDto>
    {
        public string Name { get; set; }
    }

    public class AddGenreCommandHandler : IRequestHandler<AddGenreCommand, AddGenreCommandDto>
    {
        private readonly IGenreUnitOfWork _genreUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public AddGenreCommandHandler(IGenreUnitOfWork genreUnitOfWork, IUserService userService, IDateTimeService dateTimeService)
        {
            _genreUnitOfWork = genreUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;

        }

        public async Task<AddGenreCommandDto> Handle(AddGenreCommand command, CancellationToken cancellationToken)
        {
            var response = new AddGenreCommandDto();

            var genre = new Persistence.Model.Genre()
            {
                Name = command.Name,
                CreatedDate = _dateTimeService.Now,
                CreatedBy = _userService.GetUserId(),
            };

            await _genreUnitOfWork.GenreDataLayer.AddGenre(genre);
            await _genreUnitOfWork.Save();

            response.GenreID = genre.GenreId;

            return response;
        }
    }
}
