﻿using Bogus;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ApollosLibrary.Application.Book.Commands.UpdateBookCommand;

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

namespace ApollosLibrary.Application.UnitTests
{
    [Collection("UnitTestCollection")]
    public class UpdateBookCommandTest : TestBase
    {
        private readonly UpdateBookCommandValidator _validator;
        private readonly Faker _faker;

        public UpdateBookCommandTest(TestFixture fixture) : base(fixture)
        {
            _validator = new UpdateBookCommandValidator();
            _faker = new Faker();
        }

        [Fact]
        public void TitleNotProvided()
        {
            var command = new UpdateBookCommand()
            {
                ISBN = "9780356501086",
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.ISBN);

            command.Title = "";

            result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.ISBN);
        }

        [Fact]
        public void TitleInvalidLength()
        {
            var command = new UpdateBookCommand()
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
            var command = new UpdateBookCommand()
            {
                ISBN = "9780356501086",
                Title = "Heir Of Novron",
                Subtitle = _faker.Lorem.Random.String(201)
            };

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor(f => f.Subtitle);
        }

        [Fact]
        public void EditionInvalidValue()
        {
            var command = new UpdateBookCommand()
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
        public async Task BookNotFound()
        {
            var command = new UpdateBookCommand()
            {
                ISBN = "9780356501086",
                Title = "Heir Of Novron",
                Edition = 0,
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
            bookDataLayer.Setup(d => d.GetBookByISBN(It.IsAny<string>())).Returns(Task.FromResult(new Domain.Book()));

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
            genreUnitOfWork.Setup(r => r.GenreDataLayer).Returns(genreDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return genreUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<BookNotFoundException>();
        }

        [Fact]
        public async Task ISBNDuplicate()
        {
            var command = new UpdateBookCommand()
            {
                ISBN = "9780356501086",
                Title = "Heir Of Novron",
                Edition = 0,
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
            bookDataLayer.Setup(d => d.GetBook(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Book()));
            bookDataLayer.Setup(d => d.GetBookByISBN(It.IsAny<string>())).Returns(Task.FromResult(new Domain.Book()));

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
            genreUnitOfWork.Setup(r => r.GenreDataLayer).Returns(genreDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return genreUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<ISBNAlreadyAddedException>();
        }

        [Fact]
        public async Task EISBNDuplicate()
        {
            var command = new UpdateBookCommand()
            {
                EISBN = "9780356501086",
                Title = "Heir Of Novron",
                Edition = 0,
            };

            var commandBook2 = new UpdateBookCommand()
            {
                EISBN = "9780356501090",
                Title = "Test Book",
                Edition = 0,
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
            bookDataLayer.Setup(d => d.GetBook(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Book()));
            bookDataLayer.Setup(d => d.GetBookByeISBN(It.IsAny<string>())).Returns(Task.FromResult(new Domain.Book()));

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
            genreUnitOfWork.Setup(r => r.GenreDataLayer).Returns(genreDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return genreUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<ISBNAlreadyAddedException>();
        }

        [Fact]
        public async Task WhenEISBNIsSame_DuplicateErrorNotThrown()
        {
            var command = new UpdateBookCommand()
            {
                EISBN = "9780356501086",
                Title = "Heir Of Novron",
                Edition = 0,
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
            bookDataLayer.Setup(d => d.GetBook(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Book()));
            bookDataLayer.Setup(d => d.GetBookByeISBN(It.IsAny<string>())).Returns(Task.FromResult(new Domain.Book()));

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
            genreUnitOfWork.Setup(r => r.GenreDataLayer).Returns(genreDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return genreUnitOfWork.Object;
            });

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<ISBNAlreadyAddedException>();
        }

        [Fact]
        public async Task PublicationFormatNotFound()
        {
            var command = new UpdateBookCommand()
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
            bookDataLayer.Setup(d => d.GetBook(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Book()));

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
            await act.Should().ThrowAsync<PublicationFormatNotFoundException>();
        }

        [Fact]
        public async Task FictionTypeNotFound()
        {
            var command = new UpdateBookCommand()
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
            bookDataLayer.Setup(d => d.GetBook(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Book()));

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
            await act.Should().ThrowAsync<FictionTypeNotFoundException>();
        }

        [Fact]
        public async Task FormTypeNotFound()
        {
            var command = new UpdateBookCommand()
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
            bookDataLayer.Setup(d => d.GetBook(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Book()));

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
            var command = new UpdateBookCommand()
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
            BusinessUnitOfWork.Setup(u => u.BusinessDataLayer).Returns(BusinessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return BusinessUnitOfWork.Object;
            });

            var bookDataLayer = new Mock<IBookDataLayer>();
            bookDataLayer.Setup(d => d.GetBook(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Book()));

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
            await act.Should().ThrowAsync<BusinessNotFoundException>();
        }

        [Fact]
        public async Task AuthorNotFound()
        {
            var command = new UpdateBookCommand()
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
            BusinessDataLayer.Setup(d => d.GetBusiness(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Business()));

            var BusinessUnitOfWork = new Mock<IBusinessUnitOfWork>();
            BusinessUnitOfWork.Setup(r => r.BusinessDataLayer).Returns(BusinessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return BusinessUnitOfWork.Object;
            });

            var bookDataLayer = new Mock<IBookDataLayer>();
            bookDataLayer.Setup(d => d.GetBook(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Book()));

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
            await act.Should().ThrowAsync<AuthorNotFoundException>();
        }

        [Fact]
        public async Task GenreNotFound()
        {
            var command = new UpdateBookCommand()
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
            BusinessDataLayer.Setup(d => d.GetBusiness(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Business()));

            var BusinessUnitOfWork = new Mock<IBusinessUnitOfWork>();
            BusinessUnitOfWork.Setup(r => r.BusinessDataLayer).Returns(BusinessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return BusinessUnitOfWork.Object;
            });

            var bookDataLayer = new Mock<IBookDataLayer>();
            bookDataLayer.Setup(d => d.GetBook(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Book()));

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

            var provider = _fixture.ServiceCollection.BuildServiceProvider();
            var mediator = provider.GetRequiredService<IMediator>();

            Func<Task> act = () => mediator.Send(command);
            await act.Should().ThrowAsync<GenreNotFoundException>();
        }

        [Fact]
        public async Task SeriesNotFound()
        {
            var command = new UpdateBookCommand()
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
            BusinessDataLayer.Setup(d => d.GetBusiness(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Business()));

            var BusinessUnitOfWork = new Mock<IBusinessUnitOfWork>();
            BusinessUnitOfWork.Setup(r => r.BusinessDataLayer).Returns(BusinessDataLayer.Object);

            _fixture.ServiceCollection.AddTransient(services =>
            {
                return BusinessUnitOfWork.Object;
            });

            var bookDataLayer = new Mock<IBookDataLayer>();
            bookDataLayer.Setup(d => d.GetBook(It.IsAny<int>())).Returns(Task.FromResult(new Domain.Book()));

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
