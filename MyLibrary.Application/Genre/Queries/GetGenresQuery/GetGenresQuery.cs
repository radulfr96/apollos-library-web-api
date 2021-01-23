using MediatR;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Application.Genre.Queries.GetGenresQuery
{
    public class GetGenresQuery : IRequest<GetGenresQueryDto>
    {
    }

    public class GetGenresQueryHandler : IRequestHandler<GetGenresQuery, GetGenresQueryDto>
    {
        private readonly IGenreUnitOfWork _genreUnitOfWork;

        public GetGenresQueryHandler(IGenreUnitOfWork genreUnitOfWork)
        {
            _genreUnitOfWork = genreUnitOfWork;
        }

        public async Task<GetGenresQueryDto> Handle(GetGenresQuery query, CancellationToken cancellationToken)
        {
            var response = new GetGenresQueryDto();

            var genres = await _genreUnitOfWork.GenreDataLayer.GetGenres();

            if (genres.Count == 0)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }

            response.Genres = genres.Select(g => new GenreDto()
            {
                GenreId = g.GenreId,
                Name = g.Name
            }).ToList();

            response.StatusCode = HttpStatusCode.OK;

            return response;
        }
    }
}
