﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using FluentValidation.TestHelper;
using ApollosLibrary.Application.Author.Queries.GetAuthorRecordQuery;

namespace ApollosLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class GetAuthorRecordQueryTest : TestBase
    {
        private readonly GetAuthorRecordQueryValidator _validator;

        public GetAuthorRecordQueryTest(TestFixture fixture) : base(fixture)
        {
            _validator = new GetAuthorRecordQueryValidator();
        }

        [Fact]
        public void AuthorIdInvalidValue()
        {
            var query = new GetAuthorRecordQuery()
            {
                AuthorRecordId = 0,
            };

            var result = _validator.TestValidate(query);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.AuthorRecordId);
        }

        [Fact]
        public async Task GetAuthorRecord_NotFound()
        {
            var query = new GetAuthorRecordQuery()
            {
                AuthorRecordId = 1
            };

            var mockReferenceDataLayer = new Mock<IReferenceDataLayer>();
            var mockAuthorDataLayer = new Mock<IAuthorDataLayer>();

            var mockReferenceUOW = new Mock<IReferenceUnitOfWork>();
            mockReferenceUOW.Setup(u => u.ReferenceDataLayer).Returns(mockReferenceDataLayer.Object);

            var mockAuthorUow = new Mock<IAuthorUnitOfWork>();
            mockAuthorUow.Setup(u => u.AuthorDataLayer).Returns(mockAuthorDataLayer.Object);

            var mockUserService = new Mock<IUserService>();

            var mockDateTimeService = new Mock<IDateTimeService>();

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockReferenceUOW.Object;
            });

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockAuthorUow.Object;
            });

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockUserService.Object;
            });

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockDateTimeService.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(query);
            await act.Should().ThrowAsync<AuthorRecordNotFoundException>();
        }
    }
}
