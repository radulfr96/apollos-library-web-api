using Bogus;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

using ApollosLibrary.Application.Common.Exceptions;
using ApollosLibrary.Application.Interfaces;
using ApollosLibrary.Application.Business.Commands.UpdateBusinessCommand;
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
    public class UpdateBusinessCommandTest : TestBase
    {
        private readonly UpdateBusinessCommandValidator _validator;

        public UpdateBusinessCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new UpdateBusinessCommandValidator();
        }

        [Fact]
        public void PubliherNameNotProvided()
        {
            var command = new UpdateBusinessCommand();

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.Name);

            command.Name = "";

            result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.Name);
        }

        [Fact]
        public void PubliherNameInvalid()
        {
            var command = new UpdateBusinessCommand()
            {
                Name = new Faker().Random.AlphaNumeric(201),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.Name);
        }

        [Fact]
        public void PubliherWebsiteInvalidLength()
        {
            var command = new UpdateBusinessCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                Website = new Faker().Random.AlphaNumeric(201)
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.Website);
        }

        [Fact]
        public void PubliherAddressNotProvided()
        {
            var command = new UpdateBusinessCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f);
        }

        [Fact]
        public void PubliherAddressInvalidValueNoStreet()
        {
            var command = new UpdateBusinessCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                City = new Faker().Address.City(),
                Postcode = new Faker().Address.ZipCode(),
                State = new Faker().Address.State(),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f);
        }

        [Fact]
        public void PubliherAddressInvalidValueNoCity()
        {
            var command = new UpdateBusinessCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                StreetAddress = new Faker().Address.StreetAddress(),
                Postcode = new Faker().Address.ZipCode(),
                State = new Faker().Address.State(),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f);
        }

        [Fact]
        public void PubliherAddressInvalidValueNoPostcode()
        {
            var command = new UpdateBusinessCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                StreetAddress = new Faker().Address.StreetAddress(),
                City = new Faker().Address.City(),
                State = new Faker().Address.State(),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f);
        }

        [Fact]
        public void BusinessCountryNotProvided()
        {
            var command = new UpdateBusinessCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                StreetAddress = new Faker().Address.StreetAddress(),
                City = new Faker().Address.City(),
                Postcode = new Faker().Address.ZipCode(),
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.CountryID);
        }

        [Fact]
        public async Task BusinessNotFound()
        {
            var command = new UpdateBusinessCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                StreetAddress = new Faker().Address.StreetAddress(),
                City = new Faker().Address.City(),
                Postcode = new Faker().Address.ZipCode(),
                CountryID = "TS",
                BusinessId = 1,
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

            var mockBusinessService = new Mock<IBusinessUnitOfWork>();
            mockBusinessService.Setup(p => p.BusinessDataLayer).Returns(new Mock<IBusinessDataLayer>().Object);
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockBusinessService.Object;
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
            await act.Should().ThrowAsync<BusinessNotFoundException>();
        }

        [Fact]
        public async Task BusinessCountryInvalidValue()
        {
            var Business = new Domain.Business()
            {
                CreatedDate = new DateTime(2021, 02, 07),
                IsDeleted = false,
                City = "Melbourne",
                CountryId = "AU",
                CreatedBy = new Guid(),
                Name = "Test Pub",
                Postcode = "3000",
                State = "Victoria",
                StreetAddress = "123 Fake Street",
                Website = "www.example.com",
                BusinessId = 1,
            };

            var command = new UpdateBusinessCommand()
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

            var mockBusinessService = new Mock<IBusinessUnitOfWork>();
            var businessDataLayer = new Mock<IBusinessDataLayer>();
            businessDataLayer.Setup(p => p.GetBusiness(It.IsAny<int>())).Returns(Task.FromResult(Business));
            mockBusinessService.Setup(p => p.BusinessDataLayer).Returns(businessDataLayer.Object);
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockBusinessService.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<CountryInvalidValueException>();
        }
    }
}
