using Bogus;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyLibrary.Application.Common.Enums;
using MyLibrary.Application.Common.Exceptions;
using MyLibrary.Application.Interfaces;
using MyLibrary.Application.Publisher.Commands.UpdatePublisherCommand;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.Persistence.Model;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class UpdatePublisherCommandTest : TestBase
    {
        private readonly UpdatePublisherCommandValidator _validator;

        public UpdatePublisherCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new UpdatePublisherCommandValidator();
        }

        [Fact]
        public void PubliherNameNotProvided()
        {
            var command = new UpdatePublisherCommand();

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.PublisherNameNotProvided.ToString()).Any());

            command.Name = "";

            result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.PublisherNameNotProvided.ToString()).Any());
        }

        [Fact]
        public void PubliherNameInvalid()
        {
            var command = new UpdatePublisherCommand()
            {
                Name = new Faker().Random.AlphaNumeric(201),
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.PublisherNameInvalidLength.ToString()).Any());
        }

        [Fact]
        public void PubliherWebsiteInvalidLength()
        {
            var command = new UpdatePublisherCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                Website = new Faker().Random.AlphaNumeric(201)
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.WebsiteInvalidLength.ToString()).Any());
        }

        [Fact]
        public void PubliherAddressNotProvided()
        {
            var command = new UpdatePublisherCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.InvalidAddressProvided.ToString()).Any());
        }

        [Fact]
        public void PubliherAddressInvalidValueNoStreet()
        {
            var command = new UpdatePublisherCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                City = new Faker().Address.City(),
                Postcode = new Faker().Address.ZipCode(),
                State = new Faker().Address.State(),
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.InvalidAddressProvided.ToString()).Any());
        }

        [Fact]
        public void PubliherAddressInvalidValueNoCity()
        {
            var command = new UpdatePublisherCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                StreetAddress = new Faker().Address.StreetAddress(),
                Postcode = new Faker().Address.ZipCode(),
                State = new Faker().Address.State(),
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.InvalidAddressProvided.ToString()).Any());
        }

        [Fact]
        public void PubliherAddressInvalidValueNoPostcode()
        {
            var command = new UpdatePublisherCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                StreetAddress = new Faker().Address.StreetAddress(),
                City = new Faker().Address.City(),
                State = new Faker().Address.State(),
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.InvalidAddressProvided.ToString()).Any());
        }

        [Fact]
        public void PubliherAddressValidValueNoState()
        {
            var command = new UpdatePublisherCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                StreetAddress = new Faker().Address.StreetAddress(),
                City = new Faker().Address.City(),
                Postcode = new Faker().Address.ZipCode(),
                CountryID = "AU",
            };

            var result = _validator.TestValidate(command);

            Assert.True(result.IsValid);
            Assert.True(result.Errors.Count() == 0);
        }

        [Fact]
        public void PublisherCountryNotProvided()
        {
            var command = new UpdatePublisherCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                StreetAddress = new Faker().Address.StreetAddress(),
                City = new Faker().Address.City(),
                Postcode = new Faker().Address.ZipCode(),
            };

            var result = _validator.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.CountryNotProvided.ToString()).Any());
        }

        [Fact]
        public async Task PublisherNotFound()
        {
            var command = new UpdatePublisherCommand()
            {
                Name = new Faker().Random.AlphaNumeric(50),
                StreetAddress = new Faker().Address.StreetAddress(),
                City = new Faker().Address.City(),
                Postcode = new Faker().Address.ZipCode(),
                CountryID = "TS",
                PublisherId = 1,
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

            var mockPublisherService = new Mock<IPublisherUnitOfWork>();
            mockPublisherService.Setup(p => p.PublisherDataLayer).Returns(new Mock<IPublisherDataLayer>().Object);
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockPublisherService.Object;
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

            await Assert.ThrowsAsync<PublisherNotFoundException>(() => mediator.Send(command));
        }

        [Fact]
        public async Task PublisherCountryInvalidValue()
        {
            var publisher = new Persistence.Model.Publisher()
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
                PublisherId = 1,
            };

            var command = new UpdatePublisherCommand()
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

            var mockPublisherService = new Mock<IPublisherUnitOfWork>();

            var publisherDataLayer = new Mock<IPublisherDataLayer>();
            publisherDataLayer.Setup(p => p.GetPublisher(It.IsAny<int>())).Returns(Task.FromResult(publisher));
            mockPublisherService.Setup(p => p.PublisherDataLayer).Returns(publisherDataLayer.Object);
            _fixture.ServiceCollection.AddTransient(services =>
            {
                return mockPublisherService.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            await Assert.ThrowsAsync<CountryInvalidValueException>(() => mediator.Send(command));
        }
    }
}
