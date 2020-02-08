using Microsoft.Extensions.Configuration;
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
        private IConfiguration Configuration { get; set; }

        public UserTests()
        {

            var configBuilder = new ConfigurationBuilder().AddInMemoryCollection(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("TokenKey", "TestKjKAFOJPF\\466484dsvsfhiuehefhoipjejfopkepojfOPJFAEFJLEAJFMLJ3PR0-OFEikrokdey1"),
            });

            Configuration = configBuilder.Build();
        }

        [Fact]
        public void GetUsersSuccess()
        {

            var role = new Role()
            {
                RoleId = 1,
                Name = "TestRole",
            };

            var userRoles = new List<UserRole>()
            {
                new UserRole()
                {
                    UserId = 1,
                    RoleId = 1,
                    Role = role
                }
            };

            var users = new List<User>() {
                new User()
                {
                    CreatedBy = "Unit Test",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = "TestHash",
                    Salter = "TestSalt",
                    UserId = 1,
                    Username = "TestUser",
                    UserRole = userRoles
                }
            };

            var mockUserDataLayer = new UserMockDataLayer(users);
            var unitOfWork = new UserUnitOfWork(mockUserDataLayer);
            var service = new UserService(unitOfWork, Configuration);

            var response = service.GetUsers();

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(response.Users.Count > 0);
            Assert.True(response.Users[0].Username == "TestUser");
            Assert.True(response.Users[0].Roles.Count > 0);
            Assert.True(response.Users[0].Roles[0].Name == "TestRole");
        }

        [Fact]
        public void GetUsersNotFound()
        {
            var mockDataLayer = new UserMockDataLayer(new List<User>());
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
            var response = service.GetUsers();

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.True(response.Users.Count == 0);
        }

        [Fact]
        public void CheckUsernamrExists()
        {
            var mockDataLayer = new UserMockDataLayer(
                new List<User>()
                {
                    new User()
                    {
                        CreatedBy = "Unit Test",
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        Password = "TestHash",
                        Salter = "TestSalt",
                        UserId = 1,
                        Username = "TestUser",
                    }
                }
            );

            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
            var response = service.UsernameCheck("TestUser");

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(response.Result);
        }

        [Fact]
        public void CheckUserByUsernameNotExistsNoUsers()
        {
            var mockDataLayer = new UserMockDataLayer(new List<User>());
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
            var response = service.GetUsers();

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.True(response.Users.Count == 0);
        }

        [Fact]
        public void CheckUsernameUnknownUser()
        {
            var mockDataLayer = new UserMockDataLayer(
                new List<User>()
                {
                    new User()
                    {
                        CreatedBy = "Unit Test",
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        Password = "TestHash",
                        Salter = "TestSalt",
                        UserId = 1,
                        Username = "TestUser",
                    }
                }
            );

            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
            var response = service.UsernameCheck("TeUser");

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.False(response.Result);
        }

        [Fact]
        public void GetUserByIdSuccess()
        {
            var mockDataLayer = new UserMockDataLayer(
                new List<User>()
                {
                    new User()
                    {
                        CreatedBy = "Unit Test",
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        Password = "pkfpaejfoijaoi",
                        Salter = "wkfqokfpokp",
                        UserId = 1,
                        Username = "TestUser",
                    }
                }
            );
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
            var response = service.GetUserById(1);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(response.User.Username == "TestUser");
        }

        [Fact]
        public void GetUserByIdNotFoundNoUsers()
        {
            var mockDataLayer = new UserMockDataLayer(new List<User>());
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
            var response = service.GetUserById(-1);

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.True(response.User == null);
        }

        [Fact]
        public void GetUserByIdNotFound()
        {
            var mockDataLayer = new UserMockDataLayer(
                new List<User>()
                {
                    new User()
                    {
                        CreatedBy = "Unit Test",
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        Password = "pkfpaejfoijaoi",
                        Salter = "wkfqokfpokp",
                        UserId = 1,
                        Username = "Test User",
                    }
                }
            );

            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
            var response = service.GetUserById(2);

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.True(response.User == null);
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

                UserId = 1,
                Username = "TestUser"
            };

            var mockDataLayer = new UserMockDataLayer(new List<User>() { user });
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
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
                UserId = 1,
                Username = "TestUser"
            };

            var mockDataLayer = new UserMockDataLayer(new List<User>() { user });
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
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
                UserId = 1,
                Username = "TestUser"
            };

            var mockDataLayer = new UserMockDataLayer(new List<User>() { user });
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
            var response = service.Login(new LoginRequest()
            {
                Username = "TestUse",
                Password = "TestPassword1"
            });

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrEmpty(response.Token));
        }

        [Fact]
        public void LoginUserFailMissingUsername()
        {

            var mockDataLayer = new UserMockDataLayer(new List<User>());
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
            var response = service.Login(new LoginRequest()
            {
                Username = "",
                Password = "TestPassword1"
            });

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrEmpty(response.Token));
            Assert.True(response.Messages[0] == "Please provide your username to login.");
        }

        [Fact]
        public void LoginUserFailMissingPassword()
        {

            var mockDataLayer = new UserMockDataLayer(new List<User>());
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
            var response = service.Login(new LoginRequest()
            {
                Username = "TestUsername1",
                Password = ""
            });

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrEmpty(response.Token));
            Assert.True(response.Messages[0] == "Please provide your password to login.");
        }

        [Fact]
        public void RegsiterUserFailUsernameTaken()
        {
            var user = new User()
            {
                CreatedBy = "Users Unit Test",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Password = "U5Suy6J/faf/RV0K/kaknxGiHt8xVNzD9s7w=",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 1,
                Username = "TestUser"
            };

            var mockDataLayer = new UserMockDataLayer(new List<User>() { user });
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
            var response = service.Register(new RegisterUserRequest()
            {
                ConfirmPassword = "TestPassword1",
                Username = "TestUser",
                Password = "TestPassword1",
            });

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrEmpty(response.Token));
            Assert.True(response.Messages[0] == "Registration unsuccessful user already exists");
        }

        [Fact]
        public void RegsiterUserFailUsernameMissing()
        {
            var mockDataLayer = new UserMockDataLayer(new List<User>());
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
            var response = service.Register(new RegisterUserRequest()
            {
                ConfirmPassword = "TestPaword1",
                Username = "",
                Password = "TestPassword1",
            });

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrEmpty(response.Token));
            Assert.True(response.Messages[0] == "Please provide a username");
        }

        [Fact]
        public void RegsiterUserFailPasswordMissing()
        {
            var mockDataLayer = new UserMockDataLayer(new List<User>());
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
            var response = service.Register(new RegisterUserRequest()
            {
                ConfirmPassword = "TestPaword1",
                Username = "TestUser",
                Password = "",
            });

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrEmpty(response.Token));
            Assert.True(response.Messages[0] == "Please provide a password");
        }

        [Fact]
        public void RegsiterUserFailConfirmPasswordMissing()
        {
            var mockDataLayer = new UserMockDataLayer(new List<User>());
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
            var response = service.Register(new RegisterUserRequest()
            {
                ConfirmPassword = "",
                Username = "TestUser",
                Password = "TestPassword1",
            });

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrEmpty(response.Token));
            Assert.True(response.Messages[0] == "Please provide your confirmation password");
        }

        [Fact]
        public void RegsiterUserFailPasswordMissmatch()
        {
            var mockDataLayer = new UserMockDataLayer(new List<User>());
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
            var response = service.Register(new RegisterUserRequest()
            {
                ConfirmPassword = "TestPaword1",
                Username = "TestUser",
                Password = "TestPassword1",
            });

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrEmpty(response.Token));
            Assert.True(response.Messages[0] == "Password do not match");
        }

        [Fact]
        public void RegsiterUserSuccess()
        {
            var mockDataLayer = new UserMockDataLayer(new List<User>());
            var unitOfWork = new UserUnitOfWork(mockDataLayer);
            var service = new UserService(unitOfWork, Configuration);
            var response = service.Register(new RegisterUserRequest()
            {
                ConfirmPassword = "TestPassword1",
                Username = "TestUser",
                Password = "TestPassword1",
            });

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.False(string.IsNullOrEmpty(response.Token));
        }
    }
}
