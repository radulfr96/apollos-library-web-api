using MyLibrary.Common.Requests;
using MyLibrary.Data.Model;
using MyLibrary.Mock.DataLayers;
using MyLibrary.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Net;
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

            Assert.True(response.StatusCode == HttpStatusCode.OK);
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

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.True(response.Users.Count == 0);
        }

        [Fact]
        public void LoginUserSuccess()
        {
            var user = new User()
            {
                CreatedBy = "Users Unit Test",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Password = "U5Suy6JmLuYeztykx//RV0K/kaknxGiHt8xVNzD9s7w=",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                SetPassword = false,
                UserId = 1,
                Username = "TestUser"
            };

            var mockDataLayer = new UserMockDataLayer(new List<User>() { user });
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork);
            var response = service.Login(new LoginRequest()
            {
                Username = "TestUser",
                Password = "TestPassword1"
            });

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.False(string.IsNullOrEmpty(response.Token));
        }

        [Fact]
        public void LoginUserFailBadUsername()
        {

            var user = new User()
            {
                CreatedBy = "Users Unit Test",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Password = "U5Suy6JmLuYeztykx//RV0K/kaknxGiHt8xVNzD9s7w=",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                SetPassword = false,
                UserId = 1,
                Username = "TestUser"
            };

            var mockDataLayer = new UserMockDataLayer(new List<User>() { user });
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork);
            var response = service.Login(new LoginRequest()
            {
                Username = "TestUse",
                Password = "TestPassword1"
            });

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrEmpty(response.Token));
        }

        [Fact]
        public void LoginUserFailBadPassword()
        {

            var user = new User()
            {
                CreatedBy = "Users Unit Test",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Password = "U5Suy6J/faf/RV0K/kaknxGiHt8xVNzD9s7w=",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                SetPassword = false,
                UserId = 1,
                Username = "TestUser"
            };

            var mockDataLayer = new UserMockDataLayer(new List<User>() { user });
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork);
            var response = service.Login(new LoginRequest()
            {
                Username = "TestUse",
                Password = "TestPassword1"
            });

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrEmpty(response.Token));
        }
    }
}
