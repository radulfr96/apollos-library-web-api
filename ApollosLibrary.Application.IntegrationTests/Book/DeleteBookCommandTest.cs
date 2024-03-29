﻿using MediatR;
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
using ApollosLibrary.Application.Book.Commands.DeleteBookCommand;
using FluentAssertions;
using NodaTime;

namespace ApollosLibrary.Application.IntegrationTests
{
    [Collection("IntegrationTestCollection")]
    public class DeleteBookCommandTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        public DeleteBookCommandTest(TestFixture fixture) : base(fixture)
        {
            var services = fixture.ServiceCollection;

            var mockDateTimeService = new Mock<IDateTimeService>();
            mockDateTimeService.Setup(d => d.Now).Returns(LocalDateTime.FromDateTime(new DateTime(2021, 02, 07)));
            services.AddSingleton(mockDateTimeService.Object);

            var provider = services.BuildServiceProvider();
            _mediatr = provider.GetRequiredService<IMediator>();
            _context = provider.GetRequiredService<ApollosLibraryContext>();
        }

        [Fact]
        public async Task DeleteBookCommand()
        {
            Thread.CurrentPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Sid, "1"),
            });

            var book = BookGenerator.GetGenericPhysicalBook(Guid.NewGuid());
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            var command = new DeleteBookCommand()
            {
                BookId = book.BookId,
            };

            var result = await _mediatr.Send(command);

            result.Should().NotBeNull();

            var deletedBook = _context.Books.FirstOrDefault(b => b.BookId == command.BookId);

            result.Should().NotBeNull();
        }
    }
}
