using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
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
using ApollosLibrary.Application.Series.Commands.AddSeriesCommand;
using ApollosLibrary.Application.Book.Commands.AddBookCommand;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class AddSeriesCommandTest : TestBase
    {
        private readonly IDateTimeService _dateTime;
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public AddSeriesCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(new DateTime(2021, 02, 07));
            _dateTime = mockDateTimeService.Object;
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
            _contextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
        }

        [Fact]
        public async Task AddSeriesCommand()
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

            var seriesGenerated = SeriesGenerator.GetSeries(userID);

            var command = new AddSeriesCommand()
            {
                Name = seriesGenerated.Name,
                SeriesOrder = new Dictionary<int, int>(),
            };

            command.SeriesOrder.Add(bookResult.BookId, seriesGenerated.SeriesOrders.First().Number);

            var result = await _mediatr.Send(command);

            var series = _context.Series.FirstOrDefault(p => p.SeriesId == result.SeriesId);

            series.Should().BeEquivalentTo(new Domain.Series()
            {
               SeriesId = result.SeriesId,
               CreatedBy = series.CreatedBy,
               CreatedDate = series.CreatedDate,
               Name = series.Name,
            }, opt => opt.Excluding(f => f.SeriesOrders));

            series.SeriesOrders.Should().HaveCount(1);
            series.SeriesOrders.First().Should().BeEquivalentTo(new SeriesOrder()
            {
                BookId = bookResult.BookId,
                SeriesId = result.SeriesId,
                Number = series.SeriesOrders.First().Number,
            }, opt => opt.Excluding(f => f.Series).Excluding(f => f.Book).Excluding(f => f.OrderId));
        }
    }
}
