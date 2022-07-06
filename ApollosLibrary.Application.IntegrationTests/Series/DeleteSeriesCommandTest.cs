using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.IntegrationTests.Generators;
using ApollosLibrary.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using ApollosLibrary.Domain;
using ApollosLibrary.Application.Series.Commands.DeleteSeriesCommand;
using Microsoft.AspNetCore.Http;
using ApollosLibrary.Application.Book.Commands.AddBookCommand;
using NodaTime;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class DeleteSeriesCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public DeleteSeriesCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(LocalDateTime.FromDateTime(new DateTime(2021, 02, 07)));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task DeleteSeries()
        {
            var userID = Guid.NewGuid();

            var httpContext = new TestHttpContext
            {
                User = new TestPrincipal(new Claim[]
                {
                    new Claim("userid", userID.ToString()),
                })
            };

            _contextAccessor.HttpContext = httpContext;

            var bookGenerated = BookGenerator.GetGenericPhysicalBook(userID);

            var bookCommand = new AddBookCommand()
            {
                CoverImage = bookGenerated.CoverImage,
                Edition = bookGenerated.Edition,
                EISBN = bookGenerated.EIsbn,
                ISBN = bookGenerated.Isbn,
                FictionTypeId = bookGenerated.FictionTypeId,
                FormTypeId = bookGenerated.FormTypeId,
                PublicationFormatId = bookGenerated.PublicationFormatId,
                Subtitle = bookGenerated.Subtitle,
                Title = bookGenerated.Title,
            };

            var bookResult = await _mediatr.Send(bookCommand);

            var seriesGenerated = SeriesGenerator.GetSeriesNoOrders(new Guid());

            _context.Series.Add(seriesGenerated);

            _context.SaveChanges();

            var command = new DeleteSeriesCommand()
            {
                SeriesId = seriesGenerated.SeriesId,
            };

            await _mediatr.Send(command);

            var series = _context.Series.FirstOrDefault(p => p.SeriesId == command.SeriesId);

            series.Should().NotBeNull();
            series.IsDeleted.Should().BeTrue();
        }
    }
}
