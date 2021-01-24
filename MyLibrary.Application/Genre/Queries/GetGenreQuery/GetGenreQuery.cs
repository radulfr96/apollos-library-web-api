using MediatR;
using MyLibrary.Application.Common.Exceptions;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Genre.Queries.GetGenreQuery
{
    public class GetGenreQuery : IRequest<GetGenreQueryDto>
    {
        public int GenreId { get; set; }
    }

    public class GetGenreQueryHandler : IRequestHandler<GetGenreQuery, GetGenreQueryDto>
    {
        private readonly IGenreUnitOfWork _genreUnitOfWork;

        public GetGenreQueryHandler(IGenreUnitOfWork genreUnitOfWork)
        {
            _genreUnitOfWork = genreUnitOfWork;
        }

        public async Task<GetGenreQueryDto> Handle(GetGenreQuery query, CancellationToken cancellationToken)
        {
            var response = new GetGenreQueryDto();

            var genre = await _genreUnitOfWork.GenreDataLayer.GetGenre(query.GenreId);

            if (genre == null)
            {
                throw new GenreNotFoundException($"Unable to find genre with id {query.GenreId}");
            }

            response.GenreId = genre.GenreId;
            response.Name = genre.Name;
            return response;
        }
    }
}
