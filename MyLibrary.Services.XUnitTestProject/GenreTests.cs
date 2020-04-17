using Microsoft.Extensions.Configuration;
using Moq;
using MyLibrary.Common.DTOs;
using MyLibrary.Common.Requests;
using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace MyLibrary.Services.XUnitTestProject
{
    public class GenreTests
    {

        private readonly ClaimsPrincipal MockPrincipal;

        public GenreTests()
        {
            MockPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Sid, "1")
            });
        }

        [Fact]
        public void CreateGenreSuccess()
        {
            var request = new AddGenreRequest()
            {
                Name = "Test Genre"
            };

            var mockGenreDataLayer = new Mock<IGenreDataLayer>();

            mockGenreDataLayer.Setup(layer => layer.AddGenre(It.IsAny<Genre>())).Callback((Genre genre) =>
            {
                genre.GenreId = 1;
            });

            var unitOfWork = new Mock<IGenreUnitOfWork>();
            unitOfWork.Setup(u => u.GenreDataLayer).Returns(mockGenreDataLayer.Object);

            var service = new GenreService(unitOfWork.Object, MockPrincipal);

            var response = service.AddGenre(request);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(response.GenreID == 1);
        }

        [Fact]
        public void CreateGenreFail()
        {
            var request = new AddGenreRequest()
            {
                Name = null
            };

            var mockGenreDataLayer = new Mock<IGenreDataLayer>();

            var unitOfWork = new Mock<IGenreUnitOfWork>();
            unitOfWork.Setup(u => u.GenreDataLayer).Returns(mockGenreDataLayer.Object);

            var service = new GenreService(unitOfWork.Object, MockPrincipal);

            var response = service.AddGenre(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must supply a genre name");

            request.Name = "";

            response = service.AddGenre(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must supply a genre name");
        }

        [Fact]
        public void GetGenresNotFound()
        {
            var mockGenreDataLayer = new Mock<IGenreDataLayer>();

            mockGenreDataLayer.Setup(layer => layer.GetGenres()).Returns(new List<Genre>());

            var unitOfWork = new Mock<IGenreUnitOfWork>();
            unitOfWork.Setup(u => u.GenreDataLayer).Returns(mockGenreDataLayer.Object);

            var service = new GenreService(unitOfWork.Object, MockPrincipal);

            var response = service.GetGenres();

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void GetGenresSuccess()
        {
            var genre1 = new Genre()
            {
                GenreId = 1,
                CreatedDate = DateTime.Now,
                CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
                Name = "Test Genre"
            };

            var genre2 = new Genre()
            {
                GenreId = 2,
                CreatedDate = DateTime.Now,
                CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
                Name = "Test Genre 2"
            };

            var mockGenreDataLayer = new Mock<IGenreDataLayer>();

            mockGenreDataLayer.Setup(layer => layer.GetGenres()).Returns(new List<Genre>()
            {
                genre1,
                genre2
            });

            var unitOfWork = new Mock<IGenreUnitOfWork>();
            unitOfWork.Setup(u => u.GenreDataLayer).Returns(mockGenreDataLayer.Object);

            var service = new GenreService(unitOfWork.Object, MockPrincipal);

            var response = service.GetGenres();

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(response.Genres.Count == 2);
            Assert.True(response.Genres[0].GenreId == 1);
            Assert.True(response.Genres[1].GenreId == 2);
        }

        [Fact]
        public void GetGenreNotFound()
        {
            var mockGenreDataLayer = new Mock<IGenreDataLayer>();

            mockGenreDataLayer.Setup(layer => layer.GetGenre(It.IsAny<int>())).Returns(() =>
            {
                return null;
            });

            var unitOfWork = new Mock<IGenreUnitOfWork>();
            unitOfWork.Setup(u => u.GenreDataLayer).Returns(mockGenreDataLayer.Object);

            var service = new GenreService(unitOfWork.Object, MockPrincipal);

            var response = service.GetGenre(0);

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.Null(response.Genre);
        }


        [Fact]
        public void GetGenreSuccess()
        {
            var genre1 = new Genre()
            {
                GenreId = 1,
                CreatedDate = DateTime.Now,
                CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
                Name = "Test Genre"
            };

            var genre2 = new Genre()
            {
                GenreId = 2,
                CreatedDate = DateTime.Now,
                CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
                Name = "Test Genre 2"
            };

            var mockGenreDataLayer = new Mock<IGenreDataLayer>();

            mockGenreDataLayer.Setup(layer => layer.GetGenre(It.IsAny<int>())).Returns((int id) =>
            {
                return new List<Genre>()
                {
                    genre1,
                    genre2
                }.FirstOrDefault(g => g.GenreId == id);
            });

            var unitOfWork = new Mock<IGenreUnitOfWork>();
            unitOfWork.Setup(u => u.GenreDataLayer).Returns(mockGenreDataLayer.Object);

            var service = new GenreService(unitOfWork.Object, MockPrincipal);

            var response = service.GetGenre(2);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.NotNull(response.Genre);
            Assert.True(response.Genre.Name == genre2.Name);
        }

        [Fact]
        public void UpdateGenreSuccess()
        {
            var genre1 = new Genre()
            {
                GenreId = 1,
                CreatedDate = DateTime.Now,
                CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
                Name = "Test Genre"
            };

            var mockGenreDataLayer = new Mock<IGenreDataLayer>();

            mockGenreDataLayer.Setup(layer => layer.GetGenre(It.IsAny<int>())).Returns(genre1);

            var unitOfWork = new Mock<IGenreUnitOfWork>();
            unitOfWork.Setup(u => u.GenreDataLayer).Returns(mockGenreDataLayer.Object);

            var service = new GenreService(unitOfWork.Object, MockPrincipal);

            var request = new UpdateGenreRequest()
            {
                GenreID = 1,
                Name = "Test Genre 1",
            };

            var response = service.UpdateGenre(request);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.Equal(request.Name, genre1.Name);
            Assert.True(genre1.ModifiedBy == int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value));
        }

        [Fact]
        public void UpdateGenreNotFound()
        {
            var mockGenreDataLayer = new Mock<IGenreDataLayer>();

            mockGenreDataLayer.Setup(layer => layer.GetGenre(It.IsAny<int>())).Returns(() =>
            {
                return null;
            });

            var unitOfWork = new Mock<IGenreUnitOfWork>();
            unitOfWork.Setup(u => u.GenreDataLayer).Returns(mockGenreDataLayer.Object);

            var service = new GenreService(unitOfWork.Object, MockPrincipal);

            var request = new UpdateGenreRequest()
            {
                GenreID = 1,
                Name = "Test Genre 1",
            };

            var response = service.UpdateGenre(request);

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void UpdateGenreBadRequest()
        {
            var mockGenreDataLayer = new Mock<IGenreDataLayer>();
            var unitOfWork = new Mock<IGenreUnitOfWork>();
            unitOfWork.Setup(u => u.GenreDataLayer).Returns(mockGenreDataLayer.Object);

            var service = new GenreService(unitOfWork.Object, MockPrincipal);

            var request = new UpdateGenreRequest()
            {
                GenreID = 1,
                Name = null,
            };

            var response = service.UpdateGenre(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a genre name");

            request = new UpdateGenreRequest()
            {
                GenreID = 1,
                Name = "",
            };

            response = service.UpdateGenre(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a genre name");
        }

        [Fact]
        public void DeleteGenreSuccess()
        {
            var mockGenreDataLayer = new Mock<IGenreDataLayer>();
            mockGenreDataLayer.Setup(l => l.GetGenre(It.IsAny<int>())).Returns(() =>
            {
                return new Genre()
                {
                    CreatedDate = DateTime.Now,
                    CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
                    Name = "Test Genre",
                    GenreId = 1
                };
            });

            var unitOfWork = new Mock<IGenreUnitOfWork>();
            unitOfWork.Setup(u => u.GenreDataLayer).Returns(mockGenreDataLayer.Object);

            var service = new GenreService(unitOfWork.Object, MockPrincipal);

            var response = service.DeleteGenre(1);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public void DeleteGenreFail()
        {
            var mockGenreDataLayer = new Mock<IGenreDataLayer>();
            mockGenreDataLayer.Setup(l => l.GetGenre(It.IsAny<int>())).Returns(() =>
            {
                return null;
            });

            var unitOfWork = new Mock<IGenreUnitOfWork>();
            unitOfWork.Setup(u => u.GenreDataLayer).Returns(mockGenreDataLayer.Object);

            var service = new GenreService(unitOfWork.Object, MockPrincipal);

            var response = service.DeleteGenre(1);

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }
    }
}
