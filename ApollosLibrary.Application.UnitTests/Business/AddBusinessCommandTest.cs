
using Bogus;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Common.Enums;
using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Business.Commands.AddBusinessCommand;
using ApollosLibrary.DataLayer.Contracts;

using ApollosLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ApollosLibrary.Domain;
using FluentAssertions;

namespace ApollosLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class AddBusinessCommandTest : TestBase
    {
        private readonly AddBusinessCommandValidator _validator;

        public AddBusinessCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new AddBusinessCommandValidator();
        }

        [Fact]
        public void PubliherNameNotProvided()
        {
            var command = new AddBusinessCommand();

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.BusinessNameNotProvided.ToString()).Any().Should().BeTrue();

            command.Name = "";

            result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.BusinessNameNotProvided.ToString()).Any().Should().BeTrue();
        }

        [Fact]
        public void PubliherNameInvalid()
        {
            var command = new AddBusinessCommand()
            { 
                Name = new Faker().Random.AlphaNumeric(201),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.BusinessNameInvalidLength.ToString()).Any().Should().BeTrue();
        }

        [Fact]
        public void PubliherWebsiteInvalidLength()
        {
            var command = new AddBusinessCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                Website = new Faker().Random.AlphaNumeric(201)
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.WebsiteInvalidLength.ToString()).Any().Should().BeTrue();
        }

        [Fact]
        public void PubliherAddressNotProvided()
        {
            var command = new AddBusinessCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.InvalidAddressProvided.ToString()).Any().Should().BeTrue();
        }

        [Fact]
        public void PubliherAddressInvalidValueNoStreet()
        {
            var command = new AddBusinessCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                City = new Faker().Address.City(),
                Postcode = new Faker().Address.ZipCode(),
                State = new Faker().Address.State(),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.InvalidAddressProvided.ToString()).Any().Should().BeTrue();
        }

        [Fact]
        public void PubliherAddressInvalidValueNoCity()
        {
            var command = new AddBusinessCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                StreetAddress = new Faker().Address.StreetAddress(),
                Postcode = new Faker().Address.ZipCode(),
                State = new Faker().Address.State(),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.InvalidAddressProvided.ToString()).Any().Should().BeTrue();
        }

        [Fact]
        public void PubliherAddressInvalidValueNoPostcode()
        {
            var command = new AddBusinessCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                StreetAddress = new Faker().Address.StreetAddress(),
                City = new Faker().Address.City(),
                State = new Faker().Address.State(),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.InvalidAddressProvided.ToString()).Any().Should().BeTrue();
        }

        [Fact]
        public void PubliherAddressValidValueNoState()
        {
            var command = new AddBusinessCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                StreetAddress = new Faker().Address.StreetAddress(),
                City = new Faker().Address.City(),
                Postcode = new Faker().Address.ZipCode(),
                CountryID = "AU"
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeTrue();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.InvalidAddressProvided.ToString()).Any().Should().BeFalse();
        }

        [Fact]
        public void BusinessCountryNotProvided()
        {
            var command = new AddBusinessCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                StreetAddress = new Faker().Address.StreetAddress(),
                City = new Faker().Address.City(),
                Postcode = new Faker().Address.ZipCode(),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.CountryNotProvided.ToString()).Any().Should().BeTrue();
        }

        [Fact]
        public async Task BusinessCountryInvalidValue()
        {
            var command = new AddBusinessCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                StreetAddress = new Faker().Address.StreetAddress(),
                City = new Faker().Address.City(),
                Postcode = new Faker().Address.ZipCode(),
                CountryID = "TS",
            };

            var mockUserService = new Mock<IUserService>();
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockUserService.Object;
            });

            var mockDateTimeService = new Mock<IDateTimeService>();
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockDateTimeService.Object;
            });

            var mockReferenceDataLayer = new Mock<IReferenceDataLayer>();

            mockReferenceDataLayer.Setup(r => r.GetCountries()).Returns(Task.FromResult(new List<Country>()
            {
                new Country() { CountryId = "UK", Name = "United Kingdom"},
                new Country() { CountryId = "US", Name = "United States" },
                new Country() { CountryId = "AU", Name = "Australia"}
            }));

            var mockReferenceUOW = new Mock<IReferenceUnitOfWork>();
            mockReferenceUOW.Setup(u => u.ReferenceDataLayer).Returns(mockReferenceDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockReferenceUOW.Object;
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

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<CountryInvalidValueException>();
        }
    }
}
