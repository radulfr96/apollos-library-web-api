using MediatR;
using MyLibrary.Application.Common.Exceptions;
using MyLibrary.Application.Interfaces;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Genre.Commands.UpdateGenreCommand
{
    public class UpdateGenreCommand : IRequest<UpdateGenreCommandDto>
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
    }

    public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, UpdateGenreCommandDto>
    {
        private readonly IGenreUnitOfWork _genreUnitOfWork;
        private readonly IUserService _userService;
        private readonly IDateTimeService _dateTimeService;

        public UpdateGenreCommandHandler(IGenreUnitOfWork genreUnitOfWork, IUserService userService, IDateTimeService dateTimeService)
        {
            _genreUnitOfWork = genreUnitOfWork;
            _userService = userService;
            _dateTimeService = dateTimeService;
        }

        public async Task<UpdateGenreCommandDto> Handle(UpdateGenreCommand command, CancellationToken cancellationToken)
        {
            var response = new UpdateGenreCommandDto();

            var genre = await _genreUnitOfWork.GenreDataLayer.GetGenre(command.GenreId);

            if (genre == null)
            {
                throw new GenreNotFoundException($"Unable to find genre with id {command.GenreId}");
            }

            genre.Name = command.Name;
            genre.ModifiedBy = _userService.GetUserId();
            genre.ModifiedDate = _dateTimeService.Now;

            await _genreUnitOfWork.Save();

            return response;
        }
    }
}
