using Bogus;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLibrary.Application.Author.Commands.UpdateAuthorCommand;
using MyLibrary.Application.Common.Enums;
using MyLibrary.Application.Common.Exceptions;
using MyLibrary.Application.Interfaces;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.Persistence.Model;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyLibrary.Application.XUnitTestProject
{
    [Collection("UnitTestCollection")]
    public class UpdateAuthorCommandTest : TestBase
    {
        private readonly UpdateAuthorCommandValidator _validatior;
        private readonly Faker _faker;
        private readonly TestFixture _fixture1;

        public UpdateAuthorCommandTest(TestFixture fixture) : base(fixture)
        {
            _validatior = new UpdateAuthorCommandValidator();
            _faker = new Faker();
            _fixture = fixture;
        }

        [Fact]
        public void UpdateAuthorFirstnameNotProvided()
        {
            var command = new UpdateAuthorCommand();

            var result = _validatior.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.FirstnameNotProvided.ToString()).Any());

            command.Firstname = "";

            result = _validatior.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.FirstnameNotProvided.ToString()).Any());
        }

        [Fact]
        public void UpdateAuthorFirstnameInvalidLength()
        {
            var command = new UpdateAuthorCommand()
            {
                Firstname = _faker.Random.String(51),
            };

            var result = _validatior.TestValidate(command);

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
        public void UpdateAuthorFirstnameInvalidFormat(string name)
        {
            var command = new UpdateAuthorCommand()
            {
                Firstname = name,
            };

            var result = _validatior.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.FirstnameInvalidFormat.ToString()).Any());
        }

        [Fact]
        public void UpdateAuthorLastnameNotProvided()
        {
            var command = new UpdateAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = null,
            };

            var result = _validatior.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.LastnameNotProvided.ToString()).Any());

            command.Lastname = "";

            result = _validatior.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.LastnameNotProvided.ToString()).Any());
        }

        [Fact]
        public void UpdateAuthorLastnameInvalidLength()
        {
            var command = new UpdateAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Random.String(51),
            };

            var result = _validatior.TestValidate(command);

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
        public void UpdateAuthorLastnameInvalidFormat(string name)
        {
            var command = new UpdateAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = name,
            };

            var result = _validatior.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.LastnameInvalidFormat.ToString()).Any());
        }

        [Fact]
        public void UpdateAuthorMiddlenameInvalidLength()
        {
            var command = new UpdateAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Name.LastName(),
                Middlename = _faker.Random.String(51)
            };

            var result = _validatior.TestValidate(command);

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
        public void UpdateAuthorMiddlenameInvalidFormat(string name)
        {
            var command = new UpdateAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Name.LastName(),
                Middlename = name,
            };

            var result = _validatior.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.MiddlenameInvalidFormat.ToString()).Any());
        }

        [Fact]
        public void UpdateAuthorNoCountryProvided()
        {
            var command = new UpdateAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Name.LastName(),
                Middlename = _faker.Name.FirstName(),
                Description = _faker.Random.String(2001)
            };

            var result = _validatior.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.CountryNotProvided.ToString()).Any());

            command.CountryID = "";

            result = _validatior.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.CountryNotProvided.ToString()).Any());
        }


        [Fact]
        public void UpdateAuthorDescriptionInvalidLength()
        {
            var command = new UpdateAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Name.LastName(),
                Middlename = _faker.Name.FirstName(),
                CountryID = "AU",
                Description = _faker.Random.String(2001)
            };

            var result = _validatior.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.DecriptionInvalidLength.ToString()).Any());
        }

        [Fact]
        public async Task UpdateAuthorCountryInvalidValue()
        {
            var command = new UpdateAuthorCommand()
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

            var mockAuthorDataLayer = new Mock<IAuthorDataLayer>();
            mockAuthorDataLayer.Setup(a => a.GetAuthor(It.IsAny<int>())).Returns(Task.FromResult(new Persistence.Model.Author()));
            mockAuthorUow.Setup(d => d.AuthorDataLayer).Returns(mockAuthorDataLayer.Object);

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

        [Fact]
        public async Task UpdateAuthorAuthorNotFound()
        {
            var command = new UpdateAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = _faker.Name.LastName(),
                Middlename = _faker.Name.FirstName(),
                CountryID = "AU",
                Description = _faker.Random.String(2001)
            };

            var mockReferenceDataLayer = new Mock<IReferenceDataLayer>();
            var mockAuthorDataLayer = new Mock<IAuthorDataLayer>();

            mockReferenceDataLayer.Setup(r => r.GetCountries()).Returns(Task.FromResult(new List<Country>()
            {
                new Country() { CountryId = "UK", Name = "United Kingdom"},
                new Country() { CountryId = "US", Name = "United States" },
                new Country() { CountryId = "AU", Name = "Australia"}
            }));

            mockAuthorDataLayer.Setup(s => s.GetAuthor(It.IsAny<int>())).Returns(Task.FromResult((Persistence.Model.Author)null));

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

            await Assert.ThrowsAsync<AuthorNotFoundException>(() => mediator.Send(command));
        }
    }
}
