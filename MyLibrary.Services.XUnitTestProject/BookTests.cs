using Moq;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.UnitOfWork;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace MyLibrary.Services .XUnitTestProject
{
    public class BookTests
    {
        private readonly ClaimsPrincipal MockPrincipal;

        public BookTests()
        {
            MockPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Sid, "1")
            });
        }

        [Fact]
        public void GetBookFailNotFound()
        {
            var dataLayer = new Mock<IBookDataLayer>();
            var uow = new Mock<IBookUnitOfWork>();

            uow.Setup(b => b.BookDataLayer).Returns(dataLayer.Object);

            var service = new BookService(uow.Object, MockPrincipal);

            var response = service.GetBook(1);

            // Test github action test
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
