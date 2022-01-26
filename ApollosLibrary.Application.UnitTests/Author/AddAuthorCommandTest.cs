using Bogus;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using ApollosLibrary.Persistence.Model;
using Moq;
using ApollosLibrary.Application.Author.Commands.AddAuthorCommand;
using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.DataLayer.Contracts;
using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using Xunit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Common.DTOs;
using ApollosLibrary.Application.Interfaces;

namespace ApollosLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class AddAuthorCommandTest : TestBase
    {
        private readonly AddAuthorCommandValidator _validator;
        private readonly Faker _faker;

        public AddAuthorCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new AddAuthorCommandValidator();
            _faker = new Faker();
        }

        [Fact]
        public void CreateAuthorFirstnameNotProvided()
        {
            var command = new AddAuthorCommand();

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.FirstnameNotProvided.ToString()).Any());

            command.Firstname = "";

            result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.FirstnameNotProvided.ToString()).Any());
        }

        [Fact]
        public void CreateAuthorFirstnameInvalidLength()
        {
            var command = new AddAuthorCommand()
            {
                Firstname = _faker.Random.String(51),
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.FirstnameInvalidLength.ToString()).Any());
        }

        [Theory]
        [InlineData("Tester0")]
        [InlineData("Tester1")]
        [InlineData("Tester2")]
        [InlineData("Tester3")]
        [InlineData("Tester4")]
        [InlineData("Tester5")]
        [InlineData("Tester6")]
        [InlineData("Tester7")]
        [InlineData("Tester8")]
        [InlineData("Tester9")]
        [InlineData("Tester!")]
        [InlineData("Tester@")]
        [InlineData("Tester#")]
        [InlineData("Tester$")]
        [InlineData("Tester%")]
        [InlineData("Tester^")]
        [InlineData("Tester&")]
        [InlineData("Tester*")]
        [InlineData("Tester(")]
        [InlineData("Tester)")]
        [InlineData("Tester`")]
        [InlineData("Tester~")]
        [InlineData("Tester_")]
        [InlineData("Tester+")]
        [InlineData("Tester=")]
        [InlineData("Tester[")]
        [InlineData("Tester]")]
        [InlineData("Tester{")]
        [InlineData("Tester}")]
        [InlineData("Tester|")]
        [InlineData("Tester\\")]
        [InlineData("Tester")]
        [InlineData("Tester?")]
        [InlineData("Tester<")]
        [InlineData("Tester>")]
        [InlineData("Tester,")]
        [InlineData("Tester.")]
        public void CreateAuthorFirstnameInvalidFormat(string name)
        {
            var command = new AddAuthorCommand()
            {
                Firstname = name,
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.FirstnameInvalidFormat.ToString()).Any());
        }

        [Fact]
        public void CreateAuthorLastnameNotProvided()
        {
            var command = new AddAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = null,
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.LastnameNotProvided.ToString()).Any());

            command.Lastname = "";

            result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.LastnameNotProvided.ToString()).Any());
        }

        [Fact]
        public void CreateAuthorLastnameInvalidLength()
        {
            var command = new AddAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Random.String(51),
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.LastnameInvalidLength.ToString()).Any());
        }

        [Theory]
        [InlineData("Tester0")]
        [InlineData("Tester1")]
        [InlineData("Tester2")]
        [InlineData("Tester3")]
        [InlineData("Tester4")]
        [InlineData("Tester5")]
        [InlineData("Tester6")]
        [InlineData("Tester7")]
        [InlineData("Tester8")]
        [InlineData("Tester9")]
        [InlineData("Tester!")]
        [InlineData("Tester@")]
        [InlineData("Tester#")]
        [InlineData("Tester$")]
        [InlineData("Tester%")]
        [InlineData("Tester^")]
        [InlineData("Tester&")]
        [InlineData("Tester*")]
        [InlineData("Tester(")]
        [InlineData("Tester)")]
        [InlineData("Tester`")]
        [InlineData("Tester~")]
        [InlineData("Tester_")]
        [InlineData("Tester+")]
        [InlineData("Tester=")]
        [InlineData("Tester[")]
        [InlineData("Tester]")]
        [InlineData("Tester{")]
        [InlineData("Tester}")]
        [InlineData("Tester|")]
        [InlineData("Tester\\")]
        [InlineData("Tester")]
        [InlineData("Tester?")]
        [InlineData("Tester<")]
        [InlineData("Tester>")]
        [InlineData("Tester,")]
        [InlineData("Tester.")]
        public void CreateAuthorLastnameInvalidFormat(string name)
        {
            var command = new AddAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = name,
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.LastnameInvalidFormat.ToString()).Any());
        }

        [Fact]
        public void CreateAuthorMiddlenameInvalidLength()
        {
            var command = new AddAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Name.LastName(),
                Middlename = _faker.Random.String(51)
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.MiddlenameInvalidLength.ToString()).Any());
        }

        [Theory]
        [InlineData("Tester0")]
        [InlineData("Tester1")]
        [InlineData("Tester2")]
        [InlineData("Tester3")]
        [InlineData("Tester4")]
        [InlineData("Tester5")]
        [InlineData("Tester6")]
        [InlineData("Tester7")]
        [InlineData("Tester8")]
        [InlineData("Tester9")]
        [InlineData("Tester!")]
        [InlineData("Tester@")]
        [InlineData("Tester#")]
        [InlineData("Tester$")]
        [InlineData("Tester%")]
        [InlineData("Tester^")]
        [InlineData("Tester&")]
        [InlineData("Tester*")]
        [InlineData("Tester(")]
        [InlineData("Tester)")]
        [InlineData("Tester`")]
        [InlineData("Tester~")]
        [InlineData("Tester_")]
        [InlineData("Tester+")]
        [InlineData("Tester=")]
        [InlineData("Tester[")]
        [InlineData("Tester]")]
        [InlineData("Tester{")]
        [InlineData("Tester}")]
        [InlineData("Tester|")]
        [InlineData("Tester\\")]
        [InlineData("Tester")]
        [InlineData("Tester?")]
        [InlineData("Tester<")]
        [InlineData("Tester>")]
        [InlineData("Tester,")]
        [InlineData("Tester.")]
        public void CreateAuthorMiddlenameInvalidFormat(string name)
        {
            var command = new AddAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Name.LastName(),
                Middlename = name,
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.MiddlenameInvalidFormat.ToString()).Any());
        }

        [Fact]
        public void CreateAuthorNoCountryProvided()
        {
            var command = new AddAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Name.LastName(),
                Middlename = _faker.Name.FirstName(),
                Description = _faker.Random.String(2001)
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.CountryNotProvided.ToString()).Any());

            command.CountryID = "";

            result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.CountryNotProvided.ToString()).Any());
        }


        [Fact]
        public void CreateAuthorDescriptionInvalidLength()
        {
            var command = new AddAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Name.LastName(),
                Middlename = _faker.Name.FirstName(),
                CountryID = "AU",
                Description = _faker.Random.String(2001)
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.DecriptionInvalidLength.ToString()).Any());
        }

        [Fact]
        public async Task CreateAuthorCountryInvalidValue()
        {
            var command = new AddAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Name.LastName(),
                Middlename = _faker.Name.FirstName(),
                CountryID = "TS",
                Description = _faker.Random.String(2001)
            };

            var mockReferenceDataLayer = new Mock<IReferenceDataLayer>();

            mockReferenceDataLayer.Setup(r => r.GetCountries()).Returns(Task.FromResult(new List<Country>()
            {
                new Country() { CountryId = "UK", Name = "United Kingdom"},
                new Country() { CountryId = "US", Name = "United States" },
                new Country() { CountryId = "AU", Name = "Australia"}
            }));

            var mockReferenceUOW = new Mock<IReferenceUnitOfWork>();
            mockReferenceUOW.Setup(u => u.ReferenceDataLayer).Returns(mockReferenceDataLayer.Object);

            var mockAuthorUow = new Mock<IAuthorUnitOfWork>();

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

            await Assert.ThrowsAsync<CountryInvalidValueException>(() => mediator.Send(command));
        }
    }
}
