using Moq;
using MyLibrary.Common.Requests;
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
    public class PublisherUnitTests
    {
        private readonly ClaimsPrincipal MockPrincipal;

        public PublisherUnitTests()
        {
            MockPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Sid, "1")
            });
        }

        [Fact]
        public void CreatePublisherSuccessAllInfo ()
        {
            var request = new AddPublisherRequest()
            {
                Name = "Test Publisher",
                City = "Melbourne",
                CountryID = "AU",
                Postcode = "3000",
                State = "Victoria",
                StreetAddress = "123 Fake Street",
                Website = "http://www.example.com/"
            };

            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();

            mockPublisherDataLayer.Setup(layer => layer.AddPublisher(It.IsAny<Publisher>()));

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var response = service.AddPublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public void CreatePublisherSuccessWebsiteOnly()
        {
            var request = new AddPublisherRequest()
            {
                Name = "Test Publisher",
                Website = "http://www.example.com/"
            };

            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();

            mockPublisherDataLayer.Setup(layer => layer.AddPublisher(It.IsAny<Publisher>()));

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var response = service.AddPublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public void CreatePublisherSuccessAddressOnly()
        {
            var request = new AddPublisherRequest()
            {
                Name = "Test Publisher",
                City = "Melbourne",
                CountryID = "AU",
                Postcode = "3000",
                State = "Victoria",
                StreetAddress = "123 Fake Street",
            };

            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();

            mockPublisherDataLayer.Setup(layer => layer.AddPublisher(It.IsAny<Publisher>()));

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var response = service.AddPublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public void CreatePublisherFailNoName()
        {
            var request = new AddPublisherRequest()
            {
                Name = null,
                City = "Melbourne",
                CountryID = "AU",
                Postcode = "3000",
                State = "Victoria",
                StreetAddress = "123 Fake Street",
                Website = "http://www.example.com/"
            };

            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var response = service.AddPublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a publisher name");

            request.Name = "";

            response = service.AddPublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a publisher name");
        }

        [Fact]
        public void CreatePublisherFailNoAddressOrWebsite()
        {
            var request = new AddPublisherRequest()
            {
                Name = "Test Publisher",
                City = null,
                CountryID = null,
                Postcode = null,
                State = null,
                StreetAddress = null,
                Website = null
            };

            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var response = service.AddPublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide the publisher address or website.");

            request.City = "";
            request.CountryID = "";
            request.Postcode = "";
            request.State = "";
            request.StreetAddress = "";
            request.Website = "";

            response = service.AddPublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide the publisher address or website.");
        }

        [Fact]
        public void CreatePublisherFailBadAddressOnlyCity()
        {
            var request = new AddPublisherRequest()
            {
                Name = "Test Publisher",
                City = "Melbourne",
                CountryID = null,
                Postcode = null,
                State = null,
                StreetAddress = null,
                Website = "https://www.example.com"
            };

            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var response = service.AddPublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a full address.");

            request.CountryID = "";
            request.Postcode = "";
            request.State = "";
            request.StreetAddress = "";

            response = service.AddPublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a full address.");
        }

        [Fact]
        public void CreatePublisherFailBadAddressCityAndCountry()
        {
            var request = new AddPublisherRequest()
            {
                Name = "Test Publisher",
                City = "Melbourne",
                CountryID = "AU",
                Postcode = null,
                State = null,
                StreetAddress = null,
                Website = "https://www.example.com"
            };

            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var response = service.AddPublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a full address.");

            request.Postcode = "";
            request.State = "";
            request.StreetAddress = "";

            response = service.AddPublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a full address.");
        }

        [Fact]
        public void CreatePublisherFailBadAddressCityCountryPostcode()
        {
            var request = new AddPublisherRequest()
            {
                Name = "Test Publisher",
                City = "Melbourne",
                CountryID = "AU",
                Postcode = "3000",
                State = null,
                StreetAddress = null,
                Website = "https://www.example.com"
            };

            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var response = service.AddPublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a full address.");

            request.State = "";
            request.StreetAddress = "";

            response = service.AddPublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a full address.");
        }

        [Fact]
        public void CreatePublisherFailBadAddressNoStreetAddress()
        {
            var request = new AddPublisherRequest()
            {
                Name = "Test Publisher",
                City = "Melbourne",
                CountryID = "AU",
                Postcode = "3000",
                State = "Victoria",
                StreetAddress = null,
                Website = "https://www.example.com"
            };

            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var response = service.AddPublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a full address.");

            request.StreetAddress = "";

            response = service.AddPublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a full address.");
        }

        [Fact]
        public void GetPublishersNotFound()
        {
            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();

            mockPublisherDataLayer.Setup(layer => layer.GetPublishers()).Returns(new List<Publisher>());

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var response = service.GetPublishers();

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void GetPublishersSuccess()
        {
            var publisher1 = new Publisher()
            {
                PublisherId = 1,
                CreateDate = DateTime.Now,
                CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
                Name = "Test Publisher 1",
                City = "Melbourne",
                State = "Victoria",
                CountryId = "AU",
                Country = new Country()
                {
                    CountryId = "AU",
                    Name = "Australia",
                },
                IsDeleted = false,
                Postcode = "3000",
                StreetAddress = "123 Fake Street",
                Website = "https://www.example.com",
            };

            var publisher2 = new Publisher()
            {
                PublisherId = 2,
                CreateDate = DateTime.Now,
                CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
                Name = "Test Publisher 2",
                City = "Central Brooklyn",
                State = "New York",
                CountryId = "US",
                Country = new Country()
                {
                    CountryId = "US",
                    Name = "United States of America",
                },
                IsDeleted = false,
                Postcode = "11212",
                StreetAddress = "123 Fake Street",
                Website = "https://www.example.com",
            };

            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();

            mockPublisherDataLayer.Setup(layer => layer.GetPublishers()).Returns(new List<Publisher>()
            {
                publisher1,
                publisher2
            });

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var response = service.GetPublishers();

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(response.Publishers.Count == 2);
            Assert.True(response.Publishers[0].PublisherID == 1);
            Assert.True(response.Publishers[1].PublisherID == 2);
        }

        [Fact]
        public void GetPublisherNotFound()
        {
            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();

            mockPublisherDataLayer.Setup(layer => layer.GetPublisher(It.IsAny<int>())).Returns(() =>
            {
                return null;
            });

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var response = service.GetPublisher(0);

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.Null(response.Publisher);
        }


        [Fact]
        public void GetPublisherSuccess()
        {
            var publisher1 = new Publisher()
            {
                PublisherId = 1,
                CreateDate = DateTime.Now,
                CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
                Name = "Test Publisher 1",
                City = "Melbourne",
                State = "Victoria",
                CountryId = "AU",
                Country = new Country()
                {
                    CountryId = "AU",
                    Name = "Australia",
                },
                IsDeleted = false,
                Postcode = "3000",
                StreetAddress = "123 Fake Street",
                Website = "https://www.example.com",
            };

            var publisher2 = new Publisher()
            {
                PublisherId = 2,
                CreateDate = DateTime.Now,
                CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
                Name = "Test Publisher 2",
                City = "Central Brooklyn",
                State = "New York",
                CountryId = "US",
                Country = new Country()
                {
                    CountryId = "US",
                    Name = "United States of America",
                },
                IsDeleted = false,
                Postcode = "11212",
                StreetAddress = "123 Fake Street",
                Website = "https://www.example.com",
            };

            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();

            mockPublisherDataLayer.Setup(layer => layer.GetPublisher(It.IsAny<int>())).Returns((int id) =>
            {
                return new List<Publisher>()
                {
                    publisher1,
                    publisher2
                }.FirstOrDefault(g => g.PublisherId == id);
            });

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var response = service.GetPublisher(2);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.NotNull(response.Publisher);
            Assert.True(response.Publisher.Name == publisher2.Name);
            Assert.True(response.Publisher.City == publisher2.City);
            Assert.True(response.Publisher.Country.CountryID == publisher2.CountryId);
            Assert.True(response.Publisher.Postcode == publisher2.Postcode);
            Assert.True(response.Publisher.State == publisher2.State);
            Assert.True(response.Publisher.StreetAddress == publisher2.StreetAddress);
            Assert.True(response.Publisher.Website == publisher2.Website);
        }

        [Fact]
        public void UpdatePublisherSuccess()
        {
            var publisher1 = new Publisher()
            {
                PublisherId = 1,
                CreateDate = DateTime.Now,
                CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
                Name = "Test Publishr 1",
                City = "Melbourn",
                State = "Victori",
                CountryId = "AS",
                Country = new Country()
                {
                    CountryId = "AS",
                    Name = "Austria",
                },
                IsDeleted = false,
                Postcode = "300",
                StreetAddress = "123 Fake Stret",
                Website = "https://www.example.co",
            };

            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();

            mockPublisherDataLayer.Setup(layer => layer.GetPublisher(It.IsAny<int>())).Returns(publisher1);

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var request = new UpdatePublisherRequest()
            {
                PublisherID = 1,
                Name = "Test Publisher 1",
                City = "Melbourne",
                State = "Victoria",
                CountryID = "AU",
                Postcode = "3000",
                StreetAddress = "123 Fake Street",
                Website = "https://www.example.com",
            };

            var response = service.UpdatePublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(request.Name == publisher1.Name);
            Assert.True(request.City == publisher1.City);
            Assert.True(request.CountryID == publisher1.CountryId);
            Assert.True(request.Postcode == publisher1.Postcode);
            Assert.True(request.State == publisher1.State);
            Assert.True(request.StreetAddress == publisher1.StreetAddress);
            Assert.True(request.Website == publisher1.Website);
            Assert.True(publisher1.ModifiedBy == int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value));
        }

        [Fact]
        public void UpdatePublisherNotFound()
        {
            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();

            mockPublisherDataLayer.Setup(layer => layer.GetPublisher(It.IsAny<int>())).Returns(() =>
            {
                return null;
            });

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var request = new UpdatePublisherRequest()
            {
                PublisherID = 1,
                Name = "Test Genre 1",
                Website = "https://www.example.com"
            };

            var response = service.UpdatePublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void UpdatePublisherBadRequestInvalidName()
        {
            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();
            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var request = new UpdatePublisherRequest()
            {
                PublisherID = 1,
                Name = null,
                Website = "https://www.example.com"
            };

            var response = service.UpdatePublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a publisher name");

            request = new UpdatePublisherRequest()
            {
                PublisherID = 1,
                Name = "",
                Website = "https://www.example.com"
            };

            response = service.UpdatePublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a publisher name");
        }

        [Fact]
        public void UpdatePublisherBadRequestNoAddressOrWebsite()
        {
            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();
            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var request = new UpdatePublisherRequest()
            {
                PublisherID = 1,
                Name = "TestPublisher",
                City = null,
                CountryID = null,
                Postcode = null,
                State = null,
                StreetAddress = null,
                Website = null
            };

            var response = service.UpdatePublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide the publisher address or website.");

            request = new UpdatePublisherRequest()
            {
                PublisherID = 1,
                Name = "TestPublisher",
                City = "",
                CountryID = "",
                Postcode = "",
                State = "",
                StreetAddress = "",
                Website = ""
            };

            response = service.UpdatePublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide the publisher address or website.");
        }

        [Fact]
        public void UpdatePublisherBadRequestBadAddressCityOnly()
        {
            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();
            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var request = new UpdatePublisherRequest()
            {
                PublisherID = 1,
                Name = "TestPublisher",
                City = "Melbourne",
                CountryID = null,
                Postcode = null,
                State = null,
                StreetAddress = null,
                Website = null
            };

            var response = service.UpdatePublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a full address.");

            request = new UpdatePublisherRequest()
            {
                PublisherID = 1,
                Name = "TestPublisher",
                City = "Melbourne",
                CountryID = "",
                Postcode = "",
                State = "",
                StreetAddress = "",
                Website = ""
            };

            response = service.UpdatePublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a full address.");
        }

        [Fact]
        public void UpdatePublisherBadRequestBadAddressCityCountry()
        {
            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();
            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var request = new UpdatePublisherRequest()
            {
                PublisherID = 1,
                Name = "TestPublisher",
                City = "Melbourne",
                CountryID = "AU",
                Postcode = null,
                State = null,
                StreetAddress = null,
                Website = null
            };

            var response = service.UpdatePublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a full address.");

            request = new UpdatePublisherRequest()
            {
                PublisherID = 1,
                Name = "TestPublisher",
                City = "Melbourne",
                CountryID = "AU",
                Postcode = "",
                State = "",
                StreetAddress = "",
                Website = ""
            };

            response = service.UpdatePublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a full address.");
        }

        [Fact]
        public void UpdatePublisherBadRequestBadAddressCityCountryPost()
        {
            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();
            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var request = new UpdatePublisherRequest()
            {
                PublisherID = 1,
                Name = "TestPublisher",
                City = "Melbourne",
                CountryID = "AU",
                Postcode = "3000",
                State = null,
                StreetAddress = null,
                Website = null
            };

            var response = service.UpdatePublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a full address.");

            request = new UpdatePublisherRequest()
            {
                PublisherID = 1,
                Name = "TestPublisher",
                City = "Melbourne",
                CountryID = "AU",
                Postcode = "3000",
                State = "",
                StreetAddress = "",
                Website = ""
            };

            response = service.UpdatePublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a full address.");
        }

        [Fact]
        public void UpdatePublisherBadRequestBadAddressNoStreetAddress()
        {
            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();
            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var request = new UpdatePublisherRequest()
            {
                PublisherID = 1,
                Name = "TestPublisher",
                City = "Melbourne",
                CountryID = "AU",
                Postcode = "3000",
                State = "Victoria",
                StreetAddress = null,
                Website = null
            };

            var response = service.UpdatePublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a full address.");

            request = new UpdatePublisherRequest()
            {
                PublisherID = 1,
                Name = "TestPublisher",
                City = "Melbourne",
                CountryID = "AU",
                Postcode = "3000",
                State = "Victoria",
                StreetAddress = "",
                Website = ""
            };

            response = service.UpdatePublisher(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a full address.");
        }

        [Fact]
        public void DeletePublisherSuccess()
        {
            var publisher = new Publisher()
            {
                CreateDate = DateTime.Now,
                CreatedBy = int.Parse(MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value),
                Name = "Test Genre",
                PublisherId = 1,
                City = "Central Brooklyn",
                CountryId = "US",
                IsDeleted = false,
                Postcode = "11212",
                State = "New York",
                StreetAddress = "123 FakeStreet",
                Website = "https://example.com/"
            };

            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();
            mockPublisherDataLayer.Setup(l => l.GetPublisher(It.IsAny<int>())).Returns(() =>
            {
                return publisher;
            });

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var response = service.DeletePublisher(1);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(publisher.City == "Deleted");
            Assert.True(publisher.CountryId == "AU");
            Assert.True(publisher.IsDeleted == true);
            Assert.True(publisher.Name == "Deleted");
            Assert.True(publisher.Postcode == "0000");
            Assert.True(publisher.State == "Deleted");
            Assert.True(publisher.StreetAddress == "Deleted");
            Assert.True(publisher.Website == "");
        }

        [Fact]
        public void DeletePublisherNotFound()
        {
            var mockPublisherDataLayer = new Mock<IPublisherDataLayer>();
            mockPublisherDataLayer.Setup(l => l.GetPublisher(It.IsAny<int>())).Returns(() =>
            {
                return null;
            });

            var unitOfWork = new Mock<IPublisherUnitOfWork>();
            unitOfWork.Setup(u => u.PublisherDataLayer).Returns(mockPublisherDataLayer.Object);

            var service = new PublisherService(unitOfWork.Object, MockPrincipal);

            var response = service.DeletePublisher(1);

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }
    }
}
