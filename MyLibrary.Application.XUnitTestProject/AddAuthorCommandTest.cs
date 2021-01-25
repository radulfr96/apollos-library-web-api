using Bogus;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Moq;
using MyLibrary.Application.Author.Commands.AddAuthorCommand;
using MyLibrary.Application.Common.Enums;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace MyLibrary.Application.XUnitTestProject
{
    public class AddAuthorCommandTest
    {
        //private readonly ClaimsPrincipal MockPrincipal;

        private readonly AddAuthorCommandValidator _validatior;
        private readonly Faker _faker;

        //public AuthorTests()
        //{
        //    MockPrincipal = new TestPrincipal(new Claim[]
        //    {
        //        new Claim(ClaimTypes.Name, "Test User"),
        //        new Claim(ClaimTypes.Sid, "1")
        //    });
        //}

        public AddAuthorCommandTest()
        {
            _validatior = new AddAuthorCommandValidator();
            _faker = new Faker();
        }

        [Fact]
        public void CreateAuthorFirstnameNotProvided()
        {
            var command = new AddAuthorCommand();

            var result = _validatior.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.FirstnameNotProvided.ToString()).Any());

            command.Firstname = "";

            result = _validatior.TestValidate(command);

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
        public void CreateAuthorFirstnameInvalidFormat(string name)
        {
            var command = new AddAuthorCommand()
            {
                Firstname = name,
            };

            var result = _validatior.TestValidate(command);

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

            var result = _validatior.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.LastnameNotProvided.ToString()).Any());

            command.Lastname = "";

            result = _validatior.TestValidate(command);

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
        public void CreateAuthorLastnameInvalidFormat(string name)
        {
            var command = new AddAuthorCommand()
            {
                Firstname = _faker.Name.FirstName(),
                Lastname = name,
            };

            var result = _validatior.TestValidate(command);

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
        public void CreateAuthorMiddlenameInvalidFormat(string name)
        {
            var command = new AddAuthorCommand()
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
        public void CreateAuthorNoCountryProvided()
        {
            var command = new AddAuthorCommand()
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

            var result = _validatior.TestValidate(command);

            Assert.False(result.IsValid);
            Assert.True(result.Errors.Select(e => e.ErrorCode).Where(e => e == ErrorCodeEnum.DecriptionInvalidLength.ToString()).Any());
        }

        //[Fact]
        //public void GetAuthorsNotFound()
        //{
        //    var mockAuthorDataLayer = new Mock<IAuthorDataLayer>();

        //    mockAuthorDataLayer.Setup(layer => layer.GetAuthors()).Returns(new List<Author>());

        //    var unitOfWork = new Mock<IAuthorUnitOfWork>();
        //    unitOfWork.Setup(u => u.AuthorDataLayer).Returns(mockAuthorDataLayer.Object);

        //    var service = new AuthorService(unitOfWork.Object, MockPrincipal);

        //    var response = service.GetAuthors();

        //    Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        //}

        //[Fact]
        //public void GetAuthorsSuccess()
        //{
        //    var author1 = new Author()
        //    {
        //        AuthorId = 1,
        //        CreatedDate = DateTime.Now,
        //        CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
        //        FirstName = "Test Author 1",
        //        CountryId = "AU",
        //        Country = new Country()
        //        {
        //            CountryId = "AU",
        //            Name = "Australia"
        //        }
        //    };

        //    var author2 = new Author()
        //    {
        //        AuthorId = 2,
        //        CreatedDate = DateTime.Now,
        //        CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
        //        FirstName = "Test Author 2",
        //        CountryId = "AU",
        //        Country = new Country()
        //        {
        //            CountryId = "AU",
        //            Name = "Australia"
        //        }
        //    };

        //    var mockAuthorDataLayer = new Mock<IAuthorDataLayer>();

        //    mockAuthorDataLayer.Setup(layer => layer.GetAuthors()).Returns(new List<Author>()
        //    {
        //        author1,
        //        author2
        //    });

        //    var unitOfWork = new Mock<IAuthorUnitOfWork>();
        //    unitOfWork.Setup(u => u.AuthorDataLayer).Returns(mockAuthorDataLayer.Object);

        //    var service = new AuthorService(unitOfWork.Object, MockPrincipal);

        //    var response = service.GetAuthors();

        //    Assert.True(response.StatusCode == HttpStatusCode.OK);
        //    Assert.True(response.Authors.Count == 2);
        //    Assert.True(response.Authors[0].AuthorId == 1);
        //    Assert.True(response.Authors[1].AuthorId == 2);
        //}

        //[Fact]
        //public void GetAuthorNotFound()
        //{
        //    var mockAuthorDataLayer = new Mock<IAuthorDataLayer>();

        //    mockAuthorDataLayer.Setup(layer => layer.GetAuthor(It.IsAny<int>())).Returns(() =>
        //    {
        //        return null;
        //    });

        //    var unitOfWork = new Mock<IAuthorUnitOfWork>();
        //    unitOfWork.Setup(u => u.AuthorDataLayer).Returns(mockAuthorDataLayer.Object);

        //    var service = new AuthorService(unitOfWork.Object, MockPrincipal);

        //    var response = service.GetAuthor(0);

        //    Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        //    Assert.Null(response.Author);
        //}

        //[Fact]
        //public void GetAuthorSuccess()
        //{
        //    var author1 = new Author()
        //    {
        //        AuthorId = 1,
        //        CreatedDate = DateTime.Now,
        //        CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
        //        FirstName = "Wade",
        //        CountryId = "AU",
        //        Country = new Country()
        //        {
        //            CountryId = "AU",
        //            Name = "Australia",
        //        }
        //    };

        //    var author2 = new Author()
        //    {
        //        AuthorId = 2,
        //        CreatedDate = DateTime.Now,
        //        CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
        //        FirstName = "Wade",
        //        CountryId = "AU",
        //        Country = new Country()
        //        {
        //            CountryId = "AU",
        //            Name = "Australia",
        //        }
        //    };

        //    var mockAuthorDataLayer = new Mock<IAuthorDataLayer>();

        //    mockAuthorDataLayer.Setup(layer => layer.GetAuthor(It.IsAny<int>())).Returns((int id) =>
        //    {
        //        return new List<Author>()
        //        {
        //            author1,
        //            author2
        //        }.FirstOrDefault(g => g.AuthorId == id);
        //    });

        //    var unitOfWork = new Mock<IAuthorUnitOfWork>();
        //    unitOfWork.Setup(u => u.AuthorDataLayer).Returns(mockAuthorDataLayer.Object);

        //    var service = new AuthorService(unitOfWork.Object, MockPrincipal);

        //    var response = service.GetAuthor(2);

        //    Assert.True(response.StatusCode == HttpStatusCode.OK);
        //    Assert.NotNull(response.Author);
        //    Assert.True(response.Author.Firstname == author2.FirstName);
        //}

        //[Fact]
        //public void UpdateAuthorSuccess()
        //{
        //    var author1 = new Author()
        //    {
        //        AuthorId = 1,
        //        CreatedDate = DateTime.Now,
        //        CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
        //        FirstName = "Wde",
        //        Country = new Country()
        //        {
        //            CountryId = "AS",
        //            Name = "Austria",
        //        }
        //    };

        //    var mockAuthorDataLayer = new Mock<IAuthorDataLayer>();

        //    mockAuthorDataLayer.Setup(layer => layer.GetAuthor(It.IsAny<int>())).Returns(author1);

        //    var unitOfWork = new Mock<IAuthorUnitOfWork>();
        //    unitOfWork.Setup(u => u.AuthorDataLayer).Returns(mockAuthorDataLayer.Object);

        //    var service = new AuthorService(unitOfWork.Object, MockPrincipal);

        //    var request = new UpdateAuthorRequest()
        //    {
        //        AuthorID = 1,
        //        FirstName = "Wade",
        //        CountryID = "AU"
        //    };

        //    var response = service.UpdateAuthor(request);

        //    Assert.True(response.StatusCode == HttpStatusCode.OK);
        //    Assert.Equal(request.FirstName, author1.FirstName);
        //    Assert.Equal(request.CountryID, author1.CountryId);
        //    Assert.True(author1.ModifiedBy == int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value));
        //}

        //[Fact]
        //public void UpdateAuthorNotFound()
        //{
        //    var mockAuthorDataLayer = new Mock<IAuthorDataLayer>();

        //    mockAuthorDataLayer.Setup(layer => layer.GetAuthor(It.IsAny<int>())).Returns(() =>
        //    {
        //        return null;
        //    });

        //    var unitOfWork = new Mock<IAuthorUnitOfWork>();
        //    unitOfWork.Setup(u => u.AuthorDataLayer).Returns(mockAuthorDataLayer.Object);

        //    var service = new AuthorService(unitOfWork.Object, MockPrincipal);

        //    var request = new UpdateAuthorRequest()
        //    {
        //        AuthorID = 1,
        //        FirstName = "Wade",
        //        CountryID = "AU"
        //    };

        //    var response = service.UpdateAuthor(request);

        //    Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        //}

        //[Fact]
        //public void UpdateAuhorBadRequestNoName()
        //{
        //    var mockAuthorDataLayer = new Mock<IAuthorDataLayer>();
        //    var unitOfWork = new Mock<IAuthorUnitOfWork>();
        //    unitOfWork.Setup(u => u.AuthorDataLayer).Returns(mockAuthorDataLayer.Object);

        //    var service = new AuthorService(unitOfWork.Object, MockPrincipal);

        //    var request = new UpdateAuthorRequest()
        //    {
        //        AuthorID = 1,
        //        FirstName = null,
        //        CountryID = "AU"
        //    };

        //    var response = service.UpdateAuthor(request);

        //    Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        //    Assert.True(response.Messages[0] == "You must supply a first name or an alias");

        //    request = new UpdateAuthorRequest()
        //    {
        //        AuthorID = 1,
        //        FirstName = "",
        //        CountryID = "AU"
        //    };

        //    response = service.UpdateAuthor(request);

        //    Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        //    Assert.True(response.Messages[0] == "You must supply a first name or an alias");
        //}

        //[Fact]
        //public void UpdateAuhorBadRequestNoCountry()
        //{
        //    var mockAuthorDataLayer = new Mock<IAuthorDataLayer>();
        //    var unitOfWork = new Mock<IAuthorUnitOfWork>();
        //    unitOfWork.Setup(u => u.AuthorDataLayer).Returns(mockAuthorDataLayer.Object);

        //    var service = new AuthorService(unitOfWork.Object, MockPrincipal);

        //    var request = new UpdateAuthorRequest()
        //    {
        //        AuthorID = 1,
        //        FirstName = "Wade",
        //        CountryID = null
        //    };

        //    var response = service.UpdateAuthor(request);

        //    Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        //    Assert.True(response.Messages[0] == "You must select a country of origin");

        //    request = new UpdateAuthorRequest()
        //    {
        //        AuthorID = 1,
        //        FirstName = "Wade",
        //        CountryID = ""
        //    };

        //    response = service.UpdateAuthor(request);

        //    Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        //    Assert.True(response.Messages[0] == "You must select a country of origin");
        //}

        //[Fact]
        //public void DeleteAuthorSuccess()
        //{
        //    var mockAuthorDataLayer = new Mock<IAuthorDataLayer>();
        //    mockAuthorDataLayer.Setup(l => l.GetAuthor(It.IsAny<int>())).Returns(() =>
        //    {
        //        return new Author()
        //        {
        //            CreatedDate = DateTime.Now,
        //            CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
        //            FirstName = "Wade",
        //            AuthorId = 1
        //        };
        //    });

        //    var unitOfWork = new Mock<IAuthorUnitOfWork>();
        //    unitOfWork.Setup(u => u.AuthorDataLayer).Returns(mockAuthorDataLayer.Object);

        //    var service = new AuthorService(unitOfWork.Object, MockPrincipal);

        //    var response = service.DeleteAuthor(1);

        //    Assert.True(response.StatusCode == HttpStatusCode.OK);
        //}

        //[Fact]
        //public void DeleteGenreFail()
        //{
        //    var mockAuthorDataLayer = new Mock<IAuthorDataLayer>();
        //    mockAuthorDataLayer.Setup(l => l.GetAuthor(It.IsAny<int>())).Returns(() =>
        //    {
        //        return null;
        //    });

        //    var unitOfWork = new Mock<IAuthorUnitOfWork>();
        //    unitOfWork.Setup(u => u.AuthorDataLayer).Returns(mockAuthorDataLayer.Object);

        //    var service = new AuthorService(unitOfWork.Object, MockPrincipal);

        //    var response = service.DeleteAuthor(1);

        //    Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        //}

        //[Fact]
        //public void CreateAuthorAllDetailsSuccess()
        //{
        //    var request = new AddAuthorRequest()
        //    {
        //        Firstname = "Wade",
        //        Middlename = "James",
        //        Lastname = "Russell",
        //        CountryID = "AU",
        //        Description = "This is a test author"
        //    };

        //    var mockAuthorDataLayer = new Mock<IAuthorDataLayer>();

        //    mockAuthorDataLayer.Setup(layer => layer.AddAuthor(It.IsAny<Author>())).Callback((Author author) =>
        //    {
        //        author.AuthorId = 1;
        //    });

        //    var unitOfWork = new Mock<IAuthorUnitOfWork>();
        //    unitOfWork.Setup(u => u.AuthorDataLayer).Returns(mockAuthorDataLayer.Object);

        //    var service = new AuthorService(unitOfWork.Object, MockPrincipal);

        //    var response = service.AddAuthor(request);

        //    Assert.True(response.StatusCode == HttpStatusCode.OK);
        //    Assert.True(response.AuthorID == 1);
        //}

        //[Fact]
        //public void CreateAuthorSuccess()
        //{
        //    var request = new AddAuthorRequest()
        //    {
        //        Firstname = "Wade",
        //        CountryID = "AU",
        //    };

        //    var mockAuthorDataLayer = new Mock<IAuthorDataLayer>();

        //    mockAuthorDataLayer.Setup(layer => layer.AddAuthor(It.IsAny<Author>())).Callback((Author author) =>
        //    {
        //        author.AuthorId = 1;
        //    });

        //    var unitOfWork = new Mock<IAuthorUnitOfWork>();
        //    unitOfWork.Setup(u => u.AuthorDataLayer).Returns(mockAuthorDataLayer.Object);

        //    var service = new AuthorService(unitOfWork.Object, MockPrincipal);

        //    var response = service.AddAuthor(request);

        //    Assert.True(response.StatusCode == HttpStatusCode.OK);
        //    Assert.True(response.AuthorID == 1);
        //}
    }
}
