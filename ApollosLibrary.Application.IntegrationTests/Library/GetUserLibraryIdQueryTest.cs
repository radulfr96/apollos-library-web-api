﻿using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Library.Commands.CreateLibraryCommand;
using ApollosLibrary.Application.Library.Queries.GetUserLibraryIdQuery;
using ApollosLibrary.Domain;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApollosLibrary.Application.IntegrationTests.Library
{
    [Collection("IntegrationTestCollection")]
    public class GetUserLibraryIdQueryTest : TestBase
    {
        private readonly ApollosLibraryContext _context;
        private readonly IMediator _mediatr;
        private readonly IHttpContextAccessor _contextAccessor;

        public GetUserLibraryIdQueryTest(TestFixture fixture) : base(fixture)
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
        public async Task GetLibraryIdByUserCommand_CreatesNewLibrary()
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

            var command = new GetUserLibraryIdQuery();

            var result = await _mediatr.Send(command);

            var library = _context.Libraries.FirstOrDefault(l => l.LibraryId == result.LibraryId);

            library.Should().NotBeNull();
            library.Should().BeEquivalentTo(new Domain.Library()
            {
                LibraryId = result.LibraryId,
                UserId = userID,
            });
        }

        [Fact]
        public async Task GetLibraryIdByUserCommand_ExistingLibrary()
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

            var createCommand = new CreateLibraryCommand();

            var createResult = await _mediatr.Send(createCommand);

            var query = new GetUserLibraryIdQuery();

            var result = await _mediatr.Send(query);

            result.Should().NotBeNull();
            result.LibraryId.Should().BeGreaterThan(0);

            var library = _context.Libraries.FirstOrDefault(l => l.LibraryId == result.LibraryId);

            library.Should().NotBeNull();
            library.Should().BeEquivalentTo(new Domain.Library()
            {
                LibraryId = result.LibraryId,
                UserId = userID,
            });
        }
    }
}
