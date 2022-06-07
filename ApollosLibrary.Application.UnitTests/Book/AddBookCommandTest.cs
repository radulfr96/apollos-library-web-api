using Bogus;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Book.Commands.AddBookCommand;
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
using ApollosLibrary.Domain;
using FluentAssertions;
using ApollosLibrary.Domain.Enums;

namespace ApollosLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class AddBookCommandTest : TestBase
    {
        private readonly AddBookCommandValidator _validator;

        public AddBookCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new AddBookCommandValidator();
        }

        [Fact]
        public void TitleNotProvided()
        {
            var command = new AddBookCommand()
            {
                ISBN = "9780356501086",
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.Title);

            command.Title = "";

            result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.Title);
        }

        [Fact]
        public void TitleInvalidLength()
        {
            var command = new AddBookCommand()
            {
                ISBN = "9780356501086",
                Title = new Faker().Lorem.Random.String(201)
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.Title);
        }

        [Fact]
        public void SubtitleInvalidLength()
        {
            var command = new AddBookCommand()
            {
                ISBN = "9780356501086",
                Title = "Heir Of Novron",
                Subtitle = new Faker().Lorem.Random.String(201)
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.Subtitle);
        }

        [Fact]
        public void EditionInvalidValue()
        {
            var command = new AddBookCommand()
            {
                ISBN = "9780356501086",
                Title = "Heir Of Novron",
                Edition = 0,
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.Edition);
        }

        [Fact]
        public async Task PublicationFormatNotFound()
        {
            var command = new AddBookCommand()
            {
                ISBN = "9780356501086",
                Title = "Heir Of Novron",
                Edition = 0,
                PublicationFormatId = 1,
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

            var referenceDataLayer = new Mock<IReferenceDataLayer>();

            var referenceUnitOfWork = new Mock<IReferenceUnitOfWork>();
            referenceUnitOfWork.Setup(r => r.ReferenceDataLayer).Returns(referenceDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return referenceUnitOfWork.Object;
            });

            var BusinessDataLayer = new Mock<IBusinessDataLayer>();

            var BusinessUnitOfWork = new Mock<IBusinessUnitOfWork>();
            BusinessUnitOfWork.Setup(r => r.BusinessDataLayer).Returns(BusinessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return BusinessUnitOfWork.Object;
            });

            var bookDataLayer = new Mock<IBookDataLayer>();
            bookDataLayer.Setup(d => d.GetSeries(It.IsAny<int>())).Returns(Task.FromResult((Domain.Series)null));

            var bookUnitOfWork = new Mock<IBookUnitOfWork>();
            bookUnitOfWork.Setup(b => b.BookDataLayer).Returns(bookDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return bookUnitOfWork.Object;
            });

            var authorUnitOfWork = new Mock<IAuthorUnitOfWork>();

            var authorDataLayer = new Mock<IAuthorDataLayer>();
            authorUnitOfWork.Setup(r => r.AuthorDataLayer).Returns(authorDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return authorUnitOfWork.Object;
            });

            var genreUnitOfWork = new Mock<IGenreUnitOfWork>();

            var genreDataLayer = new Mock<IGenreDataLayer>();

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return genreUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = async () => await mediator.Send(command);
            await act.Should().ThrowAsync<PublicationFormatNotFoundException>();
        }

        [Fact]
        public async Task FictionTypeNotFound()
        {
            var command = new AddBookCommand()
            {
                ISBN = "9780356501086",
                Title = "Heir Of Novron",
                Edition = 0,
                PublicationFormatId = 1,
                FictionTypeId = 1,
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

            var referenceDataLayer = new Mock<IReferenceDataLayer>();
            referenceDataLayer.Setup(d => d.GetPublicationFormat(It.IsAny<int>())).Returns(Task.FromResult(new PublicationFormat()));

            var referenceUnitOfWork = new Mock<IReferenceUnitOfWork>();
            referenceUnitOfWork.Setup(r => r.ReferenceDataLayer).Returns(referenceDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return referenceUnitOfWork.Object;
            });

            var BusinessDataLayer = new Mock<IBusinessDataLayer>();

            var BusinessUnitOfWork = new Mock<IBusinessUnitOfWork>();
            BusinessUnitOfWork.Setup(r => r.BusinessDataLayer).Returns(BusinessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return BusinessUnitOfWork.Object;
            });

            var bookDataLayer = new Mock<IBookDataLayer>();
            bookDataLayer.Setup(d => d.GetSeries(It.IsAny<int>())).Returns(Task.FromResult((Domain.Series)null));

            var bookUnitOfWork = new Mock<IBookUnitOfWork>();
            bookUnitOfWork.Setup(b => b.BookDataLayer).Returns(bookDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return bookUnitOfWork.Object;
            });

            var authorUnitOfWork = new Mock<IAuthorUnitOfWork>();

            var authorDataLayer = new Mock<IAuthorDataLayer>();
            authorUnitOfWork.Setup(r => r.AuthorDataLayer).Returns(authorDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return authorUnitOfWork.Object;
            });

            var genreUnitOfWork = new Mock<IGenreUnitOfWork>();

            var genreDataLayer = new Mock<IGenreDataLayer>();

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return genreUnitOfWork.Object;
            });

            var seriesUnitOfWork = new Mock<ISeriesUnitOfWork>();

            var seriesDataLayer = new Mock<ISeriesDataLayer>();
            seriesUnitOfWork.Setup(r => r.SeriesDataLayer).Returns(seriesDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return seriesUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = async () => await mediator.Send(command);
            await act.Should().ThrowAsync<FictionTypeNotFoundException>();
        }

        [Fact]
        public async Task FormTypeNotFound()
        {
            var command = new AddBookCommand()
            {
                ISBN = "9780356501086",
                Title = "Heir Of Novron",
                Edition = 0,
                PublicationFormatId = 1,
                FictionTypeId = 1,
                FormTypeId = 1,
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

            var referenceDataLayer = new Mock<IReferenceDataLayer>();
            referenceDataLayer.Setup(d => d.GetPublicationFormat(It.IsAny<int>())).Returns(Task.FromResult(new PublicationFormat()));
            referenceDataLayer.Setup(d => d.GetFictionType(It.IsAny<int>())).Returns(Task.FromResult(new FictionType()));

            var referenceUnitOfWork = new Mock<IReferenceUnitOfWork>();
            referenceUnitOfWork.Setup(r => r.ReferenceDataLayer).Returns(referenceDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return referenceUnitOfWork.Object;
            });

            var BusinessDataLayer = new Mock<IBusinessDataLayer>();

            var BusinessUnitOfWork = new Mock<IBusinessUnitOfWork>();
            BusinessUnitOfWork.Setup(r => r.BusinessDataLayer).Returns(BusinessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return BusinessUnitOfWork.Object;
            });

            var bookDataLayer = new Mock<IBookDataLayer>();
            bookDataLayer.Setup(d => d.GetSeries(It.IsAny<int>())).Returns(Task.FromResult((Domain.Series)null));

            var bookUnitOfWork = new Mock<IBookUnitOfWork>();
            bookUnitOfWork.Setup(b => b.BookDataLayer).Returns(bookDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return bookUnitOfWork.Object;
            });

            var authorUnitOfWork = new Mock<IAuthorUnitOfWork>();

            var authorDataLayer = new Mock<IAuthorDataLayer>();
            authorUnitOfWork.Setup(r => r.AuthorDataLayer).Returns(authorDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return authorUnitOfWork.Object;
            });

            var genreUnitOfWork = new Mock<IGenreUnitOfWork>();

            var genreDataLayer = new Mock<IGenreDataLayer>();

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return genreUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<FormTypeNotFoundException>();
        }

        [Fact]
        public async Task BusinessNotFound()
        {
            var command = new AddBookCommand()
            {
                ISBN = "9780356501086",
                Title = "Heir Of Novron",
                Edition = 0,
                PublicationFormatId = 1,
                FictionTypeId = 1,
                FormTypeId = 1,
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

            var referenceDataLayer = new Mock<IReferenceDataLayer>();
            referenceDataLayer.Setup(d => d.GetPublicationFormat(It.IsAny<int>())).Returns(Task.FromResult(new PublicationFormat()));
            referenceDataLayer.Setup(d => d.GetFictionType(It.IsAny<int>())).Returns(Task.FromResult(new FictionType()));
            referenceDataLayer.Setup(d => d.GetFormType(It.IsAny<int>())).Returns(Task.FromResult(new FormType()));

            var referenceUnitOfWork = new Mock<IReferenceUnitOfWork>();
            referenceUnitOfWork.Setup(r => r.ReferenceDataLayer).Returns(referenceDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return referenceUnitOfWork.Object;
            });

            var BusinessDataLayer = new Mock<IBusinessDataLayer>();

            var BusinessUnitOfWork = new Mock<IBusinessUnitOfWork>();
            BusinessUnitOfWork.Setup(r => r.BusinessDataLayer).Returns(BusinessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return BusinessUnitOfWork.Object;
            });

            var bookDataLayer = new Mock<IBookDataLayer>();
            bookDataLayer.Setup(d => d.GetSeries(It.IsAny<int>())).Returns(Task.FromResult((Domain.Series)null));

            var bookUnitOfWork = new Mock<IBookUnitOfWork>();
            bookUnitOfWork.Setup(b => b.BookDataLayer).Returns(bookDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return bookUnitOfWork.Object;
            });

            var authorUnitOfWork = new Mock<IAuthorUnitOfWork>();

            var authorDataLayer = new Mock<IAuthorDataLayer>();
            authorUnitOfWork.Setup(r => r.AuthorDataLayer).Returns(authorDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return authorUnitOfWork.Object;
            });

            var genreUnitOfWork = new Mock<IGenreUnitOfWork>();

            var genreDataLayer = new Mock<IGenreDataLayer>();

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return genreUnitOfWork.Object;
            });

            var seriesUnitOfWork = new Mock<ISeriesUnitOfWork>();

            var seriesDataLayer = new Mock<ISeriesDataLayer>();
            seriesUnitOfWork.Setup(r => r.SeriesDataLayer).Returns(seriesDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return seriesUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<BusinessNotFoundException>();
        }

        [Fact]
        public async Task BusinessIsNotPublisher()
        {
            var command = new AddBookCommand()
            {
                ISBN = "9780356501086",
                Title = "Heir Of Novron",
                Edition = 0,
                PublicationFormatId = 1,
                FictionTypeId = 1,
                FormTypeId = 1,
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

            var referenceDataLayer = new Mock<IReferenceDataLayer>();
            referenceDataLayer.Setup(d => d.GetPublicationFormat(It.IsAny<int>())).Returns(Task.FromResult(new PublicationFormat()));
            referenceDataLayer.Setup(d => d.GetFictionType(It.IsAny<int>())).Returns(Task.FromResult(new FictionType()));
            referenceDataLayer.Setup(d => d.GetFormType(It.IsAny<int>())).Returns(Task.FromResult(new FormType()));

            var referenceUnitOfWork = new Mock<IReferenceUnitOfWork>();
            referenceUnitOfWork.Setup(r => r.ReferenceDataLayer).Returns(referenceDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return referenceUnitOfWork.Object;
            });

            var businessDataLayer = new Mock<IBusinessDataLayer>();
            businessDataLayer.Setup(b => b.GetBusiness(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Business()
            {
                BusinessId = 1,
                BusinessTypeId = (int)BusinessTypeEnum.Bookshop,
            }));

            var businessUnitOfWork = new Mock<IBusinessUnitOfWork>();
            businessUnitOfWork.Setup(r => r.BusinessDataLayer).Returns(businessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return businessUnitOfWork.Object;
            });

            var bookDataLayer = new Mock<IBookDataLayer>();
            bookDataLayer.Setup(d => d.GetSeries(It.IsAny<int>())).Returns(Task.FromResult((Domain.Series)null));

            var bookUnitOfWork = new Mock<IBookUnitOfWork>();
            bookUnitOfWork.Setup(b => b.BookDataLayer).Returns(bookDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return bookUnitOfWork.Object;
            });

            var authorUnitOfWork = new Mock<IAuthorUnitOfWork>();

            var authorDataLayer = new Mock<IAuthorDataLayer>();
            authorUnitOfWork.Setup(r => r.AuthorDataLayer).Returns(authorDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return authorUnitOfWork.Object;
            });

            var genreUnitOfWork = new Mock<IGenreUnitOfWork>();

            var genreDataLayer = new Mock<IGenreDataLayer>();

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return genreUnitOfWork.Object;
            });

            var seriesUnitOfWork = new Mock<ISeriesUnitOfWork>();

            var seriesDataLayer = new Mock<ISeriesDataLayer>();
            seriesUnitOfWork.Setup(r => r.SeriesDataLayer).Returns(seriesDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return seriesUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<BusinessIsNotPublisherException>();
        }

        [Fact]
        public async Task AuthorNotFound()
        {
            var command = new AddBookCommand()
            {
                ISBN = "9780356501086",
                Title = "Heir Of Novron",
                Edition = 0,
                PublicationFormatId = 1,
                FictionTypeId = 1,
                FormTypeId = 1,
                BusinessId = 1,
                Authors = new List<int>()
                {
                    1,
                }
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

            var referenceDataLayer = new Mock<IReferenceDataLayer>();
            referenceDataLayer.Setup(d => d.GetPublicationFormat(It.IsAny<int>())).Returns(Task.FromResult(new PublicationFormat()));
            referenceDataLayer.Setup(d => d.GetFictionType(It.IsAny<int>())).Returns(Task.FromResult(new FictionType()));
            referenceDataLayer.Setup(d => d.GetFormType(It.IsAny<int>())).Returns(Task.FromResult(new FormType()));

            var referenceUnitOfWork = new Mock<IReferenceUnitOfWork>();
            referenceUnitOfWork.Setup(r => r.ReferenceDataLayer).Returns(referenceDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return referenceUnitOfWork.Object;
            });

            var BusinessDataLayer = new Mock<IBusinessDataLayer>();
            BusinessDataLayer.Setup(d => d.GetBusiness(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Business()
            {
                BusinessTypeId = (int)BusinessTypeEnum.Publisher,
            }));

            var BusinessUnitOfWork = new Mock<IBusinessUnitOfWork>();
            BusinessUnitOfWork.Setup(r => r.BusinessDataLayer).Returns(BusinessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return BusinessUnitOfWork.Object;
            });

            var bookDataLayer = new Mock<IBookDataLayer>();

            var bookUnitOfWork = new Mock<IBookUnitOfWork>();
            bookUnitOfWork.Setup(b => b.BookDataLayer).Returns(bookDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return bookUnitOfWork.Object;
            });

            var authorUnitOfWork = new Mock<IAuthorUnitOfWork>();

            var authorDataLayer = new Mock<IAuthorDataLayer>();
            authorUnitOfWork.Setup(r => r.AuthorDataLayer).Returns(authorDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return authorUnitOfWork.Object;
            });

            var seriesUnitOfWork = new Mock<ISeriesUnitOfWork>();

            var seriesDataLayer = new Mock<ISeriesDataLayer>();
            seriesUnitOfWork.Setup(r => r.SeriesDataLayer).Returns(seriesDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return seriesUnitOfWork.Object;
            });

            var genreUnitOfWork = new Mock<IGenreUnitOfWork>();

            var genreDataLayer = new Mock<IGenreDataLayer>();

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return genreUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = async () => await mediator.Send(command);
            await act.Should().ThrowAsync<AuthorNotFoundException>();
        }

        [Fact]
        public async Task GenreNotFound()
        {
            var command = new AddBookCommand()
            {
                ISBN = "9780356501086",
                Title = "Heir Of Novron",
                Edition = 0,
                PublicationFormatId = 1,
                FictionTypeId = 1,
                FormTypeId = 1,
                BusinessId = 1,
                Genres = new List<int>()
                {
                    1,
                }
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

            var referenceDataLayer = new Mock<IReferenceDataLayer>();
            referenceDataLayer.Setup(d => d.GetPublicationFormat(It.IsAny<int>())).Returns(Task.FromResult(new PublicationFormat()));
            referenceDataLayer.Setup(d => d.GetFictionType(It.IsAny<int>())).Returns(Task.FromResult(new FictionType()));
            referenceDataLayer.Setup(d => d.GetFormType(It.IsAny<int>())).Returns(Task.FromResult(new FormType()));

            var referenceUnitOfWork = new Mock<IReferenceUnitOfWork>();
            referenceUnitOfWork.Setup(r => r.ReferenceDataLayer).Returns(referenceDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return referenceUnitOfWork.Object;
            });

            var BusinessDataLayer = new Mock<IBusinessDataLayer>();
            BusinessDataLayer.Setup(d => d.GetBusiness(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Business()
            {
                BusinessTypeId = (int)BusinessTypeEnum.Publisher,
            }));

            var BusinessUnitOfWork = new Mock<IBusinessUnitOfWork>();
            BusinessUnitOfWork.Setup(r => r.BusinessDataLayer).Returns(BusinessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return BusinessUnitOfWork.Object;
            });

            var bookDataLayer = new Mock<IBookDataLayer>();

            var bookUnitOfWork = new Mock<IBookUnitOfWork>();
            bookUnitOfWork.Setup(b => b.BookDataLayer).Returns(bookDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return bookUnitOfWork.Object;
            });

            var authorUnitOfWork = new Mock<IAuthorUnitOfWork>();

            var authorDataLayer = new Mock<IAuthorDataLayer>();
            authorUnitOfWork.Setup(r => r.AuthorDataLayer).Returns(authorDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return authorUnitOfWork.Object;
            });

            var seriesUnitOfWork = new Mock<ISeriesUnitOfWork>();

            var seriesDataLayer = new Mock<ISeriesDataLayer>();
            seriesUnitOfWork.Setup(r => r.SeriesDataLayer).Returns(seriesDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return seriesUnitOfWork.Object;
            });

            var genreUnitOfWork = new Mock<IGenreUnitOfWork>();

            var genreDataLayer = new Mock<IGenreDataLayer>();
            genreUnitOfWork.Setup(s => s.GenreDataLayer).Returns(genreDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return genreUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<GenreNotFoundException>();
        }

        [Fact]
        public async Task SeriesNotFound()
        {
            var command = new AddBookCommand()
            {
                ISBN = "9780356501086",
                Title = "Heir Of Novron",
                Edition = 0,
                PublicationFormatId = 1,
                FictionTypeId = 1,
                FormTypeId = 1,
                BusinessId = 1,
                Series = new List<int>()
                {
                    1,
                }
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

            var referenceDataLayer = new Mock<IReferenceDataLayer>();
            referenceDataLayer.Setup(d => d.GetPublicationFormat(It.IsAny<int>())).Returns(Task.FromResult(new PublicationFormat()));
            referenceDataLayer.Setup(d => d.GetFictionType(It.IsAny<int>())).Returns(Task.FromResult(new FictionType()));
            referenceDataLayer.Setup(d => d.GetFormType(It.IsAny<int>())).Returns(Task.FromResult(new FormType()));

            var referenceUnitOfWork = new Mock<IReferenceUnitOfWork>();
            referenceUnitOfWork.Setup(r => r.ReferenceDataLayer).Returns(referenceDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return referenceUnitOfWork.Object;
            });

            var BusinessDataLayer = new Mock<IBusinessDataLayer>();
            BusinessDataLayer.Setup(d => d.GetBusiness(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Business()
            {
                BusinessTypeId = (int)BusinessTypeEnum.Publisher,
            }));

            var BusinessUnitOfWork = new Mock<IBusinessUnitOfWork>();
            BusinessUnitOfWork.Setup(r => r.BusinessDataLayer).Returns(BusinessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return BusinessUnitOfWork.Object;
            });

            var bookDataLayer = new Mock<IBookDataLayer>();

            var bookUnitOfWork = new Mock<IBookUnitOfWork>();
            bookUnitOfWork.Setup(b => b.BookDataLayer).Returns(bookDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return bookUnitOfWork.Object;
            });

            var authorUnitOfWork = new Mock<IAuthorUnitOfWork>();

            var authorDataLayer = new Mock<IAuthorDataLayer>();
            authorUnitOfWork.Setup(r => r.AuthorDataLayer).Returns(authorDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return authorUnitOfWork.Object;
            });

            var genreUnitOfWork = new Mock<IGenreUnitOfWork>();

            var genreDataLayer = new Mock<IGenreDataLayer>();
            genreUnitOfWork.Setup(s => s.GenreDataLayer).Returns(genreDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return genreUnitOfWork.Object;
            });

            var seriesUnitOfWork = new Mock<ISeriesUnitOfWork>();

            var seriesDataLayer = new Mock<ISeriesDataLayer>();
            seriesUnitOfWork.Setup(s => s.SeriesDataLayer).Returns(seriesDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return seriesUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<SeriesNotFoundException>();
        }
    }
}
