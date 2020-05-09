using Moq;
using MyLibrary.Data.Model;
using MyLibrary.DataLayer.Contracts;
using MyLibrary.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace MyLibrary.Services.XUnitTestProject
{
    public class ReferenceTests
    {
        private readonly ClaimsPrincipal MockPrincipal;

        public ReferenceTests()
        {
            MockPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Sid, "1")
            });
        }

        [Fact]
        public void GetCountriesNotFound()
        {
            var mockDataLayer = new Mock<IReferenceDataLayer>();

            mockDataLayer.Setup(layer => layer.GetCountries()).Returns(new List<Country>());

            var unitOfWork = new Mock<IReferenceUnitOfWork>();
            unitOfWork.Setup(u => u.ReferenceDataLayer).Returns(mockDataLayer.Object);

            var service = new ReferenceService(unitOfWork.Object, MockPrincipal);

            var response = service.GetCountries();

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void GetCountriesSuccess()
        {
            var country1 = new Country()
            {
                CountryId = "AU",
                Name = "Australia"
            };

            var country2 = new Country()
            {
                CountryId = "US",
                Name = "United States of America"
            };

            var mockDataLayer = new Mock<IReferenceDataLayer>();

            mockDataLayer.Setup(layer => layer.GetCountries()).Returns(new List<Country>()
            {
                country1,
                country2
            });

            var unitOfWork = new Mock<IReferenceUnitOfWork>();
            unitOfWork.Setup(u => u.ReferenceDataLayer).Returns(mockDataLayer.Object);

            var service = new ReferenceService(unitOfWork.Object, MockPrincipal);

            var response = service.GetCountries();

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(response.Countries.Count == 2);
            Assert.True(response.Countries[0].CountryID == "AU");
            Assert.True(response.Countries[1].CountryID == "US");
        }
    }
}
