using MediatR;
using MyLibrary.Application.Common.Exceptions;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Genre.Commands.DeleteGenreCommand
{
    public class DeleteGenreCommand : IRequest<DeleteGenreCommandDto>
    {
        public int GenreId { get; set; }
    }

    public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, DeleteGenreCommandDto>
    {
        private readonly IGenreUnitOfWork _genreUnitOfWork;

        public DeleteGenreCommandHandler(IGenreUnitOfWork genreUnitOfWork)
        {
            _genreUnitOfWork = genreUnitOfWork;
        }

        public async Task<DeleteGenreCommandDto> Handle(DeleteGenreCommand command, CancellationToken cancellationToken)
        {
            var response = new DeleteGenreCommandDto();

            var genre = _genreUnitOfWork.GenreDataLayer.GetGenre(command.GenreId);
            if (genre == null)
            {
                throw new GenreNotFoundException($"Unable to find genre with id {command.GenreId}");
            }

            await _genreUnitOfWork.GenreDataLayer.DeleteGenre(command.GenreId);
            await _genreUnitOfWork.Save();

            return response;
        }
    }
}
