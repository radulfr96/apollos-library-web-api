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
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class AddSeriesCommandTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public AddSeriesCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(LocalDateTime.FromDateTime(new DateTime(2021, 02, 07)));
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
            };

            var result = await _mediatr.Send(command);

            var series = _context.Series.Include(s => s.SeriesRecords).FirstOrDefault(p => p.SeriesId == result.SeriesId);

            series.Should().BeEquivalentTo(new Domain.Series()
            {
               SeriesId = result.SeriesId,
               CreatedBy = series.CreatedBy,
               CreatedDate = series.CreatedDate,
               Name = series.Name,
               VersionId = series.SeriesRecords.Last().SeriesRecordId
            }, opt => opt.Excluding(f => f.SeriesRecords));

            series.SeriesRecords.Last().Should().BeEquivalentTo(new SeriesRecord()
            {
                SeriesId = series.SeriesId,
                SeriesRecordId = series.SeriesRecords.Last().SeriesRecordId,
                CreatedBy = userID,
                CreatedDate = series.CreatedDate,
                IsDeleted = false,
                Name = command.Name,
                ReportedVersion = false,
            }, opt => opt.Excluding(f => f.Series));
        }
    }
}
