using MyLibrary.Data.Model;
using MyLibrary.Mock.DataLayers;
using MyLibrary.UnitOfWork;
using System;
using System.Collections.Generic;
using Xunit;

namespace MyLibrary.Services.XUnitTestProject
{
    public class UserTests
    {
        [Fact]
        public void GetUsersSuccess()
        {
            var users = new List<User>() { 
                new User() 
                { 
                    CreatedBy = "Unit Test",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = "TestHash",
                    Salter = "TestSalt",
                    SetPassword = false,
                    UserId = 1,
                    Username = "TestUser",
                } 
            };

            var mockUserDataLayer = new UserMockDataLayer(users);
            var unitOfWork = new UserUnitOfWork(mockUserDataLayer);

            var service = new UserService(unitOfWork);

            var response = service.GetUsers();

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(response.Users.Count > 0);
            Assert.True(response.Users[0].Username == "TestUser");
        }

        [Fact]
        public void GetUsersNotFound()
        {
            var mockDataLayer = new UserMockDataLayer(new List<User>());
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork);
            var response = service.GetUsers();

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.NotFound);
            Assert.True(response.Users.Count == 0);
        }
    }
}
