using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using MyLibrary.Common.Requests;
using MyLibrary.Data.Model;
using MyLibrary.Services.XUnitTestProject.MockClasses;
using MyLibrary.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Xunit;

namespace MyLibrary.Services.XUnitTestProject
{
    public class UserTests
    {
        private IConfiguration Configuration { get; set; }

        private ClaimsPrincipal MockPrincipal;

        public UserTests()
        {

            var configBuilder = new ConfigurationBuilder().AddInMemoryCollection(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("TokenKey", "TestKjKAFOJPF466484dsvsfhiuehefhoipjejfopkepojfOPJFAEFJLEAJFMLJ3PR0-OFEikrokdey1"),
            });

            Configuration = configBuilder.Build();

            MockPrincipal = new TestPrincipal(new Claim[]
            {
                new Claim(ClaimTypes.Name, "Test User"),
            });
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

            var dataLayer = new MockUserDataLayer();
            dataLayer.Users = new List<User>()
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
                    UserRole = userRoles
                }
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = dataLayer;
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);

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
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer();
            var response = service.GetUsers();

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.True(response.Users.Count == 0);
        }

        [Fact]
        public void CheckUsernamrExists()
        {
            var mockUserDataLayer = new MockUserDataLayer();
            mockUserDataLayer.Users = new List<User>()
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
                };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = mockUserDataLayer;


            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.UsernameCheck("TestUser");

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(response.Result);
        }

        [Fact]
        public void CheckUserByUsernameNotExistsNoUsers()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.GetUsers();

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.True(response.Users.Count == 0);
        }

        [Fact]
        public void CheckUsernameUnknownUser()
        {
            var mockUserDataLayer = new MockUserDataLayer();
            mockUserDataLayer.Users = new List<User>()
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
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = mockUserDataLayer;

            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.UsernameCheck("TeUser");

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.False(response.Result);
        }

        [Fact]
        public void GetUserByIdSuccess()
        {
            var userDataLayer = new MockUserDataLayer();
            var roleDataLayer = new MockRoleDataLayer();
            userDataLayer.Users = new List<User>()
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
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = userDataLayer;
            mockUserUnitOfWork.MockRoleDataLayer = roleDataLayer;

            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.GetUserById(1);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(response.User.Username == "TestUser");
        }

        [Fact]
        public void GetUserByIdNotFoundNoUsers()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.GetUserById(-1);

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.True(response.User == null);
        }

        [Fact]
        public void GetUserByIdNotFound()
        {
            var userDataLayer = new MockUserDataLayer();
            userDataLayer.Users = new List<User>()
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
            };
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = userDataLayer;

            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.GetUserById(2);

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.True(response.User == null);
        }

        [Fact]
        public void LoginUserSuccessActive()
        {
            var userDataLayer = new MockUserDataLayer()
            {
                Users = new List<User>()
                {
                    new User()
                    {
                        CreatedBy = "Users Unit Test",
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        Password = "U5Suy6JmLuYeztykx//RV0K/kaknxGiHt8xVNzD9s7w=",
                        Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                        UserId = 1,
                        Username = "TestUser"
                    }
                }
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = userDataLayer;

            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.Login(new LoginRequest()
            {
                Username = "TestUser",
                Password = "TestPassword1"
            });

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.False(string.IsNullOrEmpty(response.Token));
        }

        [Fact]
        public void LoginUserSuccessReActivate()
        {
            var userDataLayer = new MockUserDataLayer()
            {
                Users = new List<User>()
                {
                    new User()
                    {
                        CreatedBy = "Users Unit Test",
                        CreatedDate = DateTime.Now,
                        IsActive = false,
                        Password = "U5Suy6JmLuYeztykx//RV0K/kaknxGiHt8xVNzD9s7w=",
                        Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                        UserId = 1,
                        Username = "TestUser"
                    }
                }
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = userDataLayer;

            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.Login(new LoginRequest()
            {
                Username = "TestUser",
                Password = "TestPassword1"
            });

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(userDataLayer.Users[0].IsActive);
            Assert.False(string.IsNullOrEmpty(response.Token));
        }

        [Fact]
        public void LoginUserFailBadUsername()
        {

            var dataLayer = new MockUserDataLayer();
            dataLayer.Users = new List<User>() {
                new User()
                {
                    CreatedBy = "Users Unit Test",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = "U5Suy6JmLuYeztykx//RV0K/kaknxGiHt8xVNzD9s7w=",
                    Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                    UserId = 1,
                    Username = "TestUser"
                }
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = dataLayer;
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
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
            var dataLayer = new MockUserDataLayer();
            dataLayer.Users = new List<User>()
            {
                new User
                {
                    CreatedBy = "Users Unit Test",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = "U5Suy6J/faf/RV0K/kaknxGiHt8xVNzD9s7w=",
                    Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                    UserId = 1,
                    Username = "TestUser"
                }
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = dataLayer;

            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
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
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
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
            var userDataLayer = new MockUserDataLayer();
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = userDataLayer;
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
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
        public void RegsiterUserSuccess()
        {
            var userDataLayer = new MockUserDataLayer();
            var roleDataLayer = new MockRoleDataLayer();
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = userDataLayer;
            mockUserUnitOfWork.MockRoleDataLayer = roleDataLayer;
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.Register(new RegisterUserRequest()
            {
                ConfirmationPassword = "TestPassword1",
                Username = "TestUser",
                Password = "TestPassword1",
            });

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.False(string.IsNullOrEmpty(response.Token));
        }

        [Fact]
        public void RegsiterUserFailUsernameTaken()
        {
            var userDataLayer = new MockUserDataLayer();
            userDataLayer.Users = new List<User>()
            {
                new User()
                {
                    CreatedBy = "Users Unit Test",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = "U5Suy6J/faf/RV0K/kaknxGiHt8xVNzD9s7w=",
                    Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                    UserId = 1,
                    Username = "TestUser"
                }
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = userDataLayer;
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.Register(new RegisterUserRequest()
            {
                ConfirmationPassword = "TestPassword1",
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
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.Register(new RegisterUserRequest()
            {
                ConfirmationPassword = "TestPaword1",
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
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.Register(new RegisterUserRequest()
            {
                ConfirmationPassword = "TestPaword1",
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
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.Register(new RegisterUserRequest()
            {
                ConfirmationPassword = "",
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
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer();

            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.Register(new RegisterUserRequest()
            {
                ConfirmationPassword = "TestPaword1",
                Username = "TestUser",
                Password = "TestPassword1",
            });

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrEmpty(response.Token));
            Assert.True(response.Messages[0] == "Password do not match");
        }

        [Fact]
        public void RegsiterUserFailWeakPasswordTooShort()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.Register(new RegisterUserRequest()
            {
                ConfirmationPassword = "Teord1",
                Username = "TestUser",
                Password = "Teord1",
            });

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "Password is not strong enough");
        }

        [Fact]
        public void RegsiterUserFailWeakPasswordNoNumber()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.Register(new RegisterUserRequest()
            {
                ConfirmationPassword = "TestPassword",
                Username = "TestUser",
                Password = "TestPassword",
            });

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "Password is not strong enough");
        }

        [Fact]
        public void UpdateUsernameFailNoNewUsername()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var mockUserDataLayer = new MockUserDataLayer();
            mockUserDataLayer.Users = new List<User>()
            {
                new User()
                {
                    CreatedBy = "UnitTest",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = "TestPassword",
                    Salter = "TestSalt",
                    UserId = 1,
                    Username = "OriginalUser"
                },
                new User()
                {
                    CreatedBy = "UnitTest",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = "TestPass",
                    Salter = "TestPast",
                    UserId = 2,
                    Username = "TestUser",
                }
            };

            mockUserUnitOfWork.MockUserDataLayer = mockUserDataLayer;
            mockUserUnitOfWork.MockRoleDataLayer = new MockRoleDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.UpdateUsername(new UpdateUsernameRequest()
            {
                Password = "TestPass"
            }, 4);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a new username");

            response = service.UpdateUsername(new UpdateUsernameRequest()
            {
                NewUsername = "",
                Password = "TestPass"
            }, 4);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide a new username");
        }

        [Fact]
        public void UpdateUsernameFailNoPassword()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var mockUserDataLayer = new MockUserDataLayer();
            mockUserDataLayer.Users = new List<User>()
            {
                new User()
                {
                    CreatedBy = "UnitTest",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = "TestPassword",
                    Salter = "TestSalt",
                    UserId = 1,
                    Username = "OriginalUser"
                },
                new User()
                {
                    CreatedBy = "UnitTest",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = "TestPass",
                    Salter = "TestPast",
                    UserId = 2,
                    Username = "TestUser",
                }
            };

            mockUserUnitOfWork.MockUserDataLayer = mockUserDataLayer;
            mockUserUnitOfWork.MockRoleDataLayer = new MockRoleDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.UpdateUsername(new UpdateUsernameRequest()
            {
                NewUsername = "NewUser"
            }, 4);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide your password to update your username");

            response = service.UpdateUsername(new UpdateUsernameRequest()
            {
                NewUsername = "NewUser",
            }, 4);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "You must provide your password to update your username");
        }

        [Fact]
        public void UpdateUsernameFailNewUsernameTaken()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var mockUserDataLayer = new MockUserDataLayer();
            mockUserDataLayer.Users = new List<User>()
            {
                new User()
                {
                    CreatedBy = "UnitTest",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = "TestPassword",
                    Salter = "TestSalt",
                    UserId = 1,
                    Username = "OriginalUser"
                },
                new User()
                {
                    CreatedBy = "UnitTest",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = "TestPass",
                    Salter = "TestPast",
                    UserId = 2,
                    Username = "TestUser",
                }
            };

            mockUserUnitOfWork.MockUserDataLayer = mockUserDataLayer;
            mockUserUnitOfWork.MockRoleDataLayer = new MockRoleDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.UpdateUsername(new UpdateUsernameRequest()
            {
                NewUsername = "OriginalUser",
                Password = "TestPass"
            }, 2);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "Username already exists");
        }

        [Fact]
        public void UpdateUsernameFailUserNotFound()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var mockUserDataLayer = new MockUserDataLayer();
            mockUserDataLayer.Users = new List<User>()
            {
                new User()
                {
                    CreatedBy = "UnitTest",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = "TestPassword",
                    Salter = "TestSalt",
                    UserId = 1,
                    Username = "OriginalUser"
                },
                new User()
                {
                    CreatedBy = "UnitTest",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = "TestPass",
                    Salter = "TestPast",
                    UserId = 2,
                    Username = "TestUser",
                }
            };

            mockUserUnitOfWork.MockUserDataLayer = mockUserDataLayer;
            mockUserUnitOfWork.MockRoleDataLayer = new MockRoleDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.UpdateUsername(new UpdateUsernameRequest()
            {
                NewUsername = "NewUsername",
                Password = "TestPass"
            }, 4);

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.True(response.Messages[0] == "Unable to find user with id [4]");
        }

        [Fact]
        public void UpdateUsernameFailWrongPassword()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var mockUserDataLayer = new MockUserDataLayer();
            mockUserDataLayer.Users = new List<User>()
            {
                new User()
                {
                    CreatedBy = "UnitTest",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = "TestPassword",
                    Salter = "TestSalt",
                    UserId = 1,
                    Username = "OriginalUser"
                },
                new User()
                {
                    CreatedBy = "UnitTest",
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Password = "TestPass",
                    Salter = "TestPast",
                    UserId = 2,
                    Username = "TestUser",
                }
            };

            mockUserUnitOfWork.MockUserDataLayer = mockUserDataLayer;
            mockUserUnitOfWork.MockRoleDataLayer = new MockRoleDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.UpdateUsername(new UpdateUsernameRequest()
            {
                NewUsername = "NewUsername",
                Password = "TestPasscdvd"
            }, 2);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "Password is not correct");
        }

        [Fact]
        public void UpdateUsernamePass()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var mockUserDataLayer = new MockUserDataLayer();

            var user1 = new User()
            {
                CreatedBy = "UnitTest",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Password = "TestPassword",
                Salter = "TestSalt",
                UserId = 1,
                Username = "OriginalUser"
            };

            var user2 = new User()
            {
                CreatedBy = "UnitTest",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Password = "U5Suy6JmLuYeztykx//RV0K/kaknxGiHt8xVNzD9s7w=",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 2,
                Username = "TestUser",
            };

            mockUserDataLayer.Users = new List<User>();
            mockUserDataLayer.Users.Add(user1);
            mockUserDataLayer.Users.Add(user2);

            mockUserUnitOfWork.MockUserDataLayer = mockUserDataLayer;
            mockUserUnitOfWork.MockRoleDataLayer = new MockRoleDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);

            var request = new UpdateUsernameRequest()
            {
                NewUsername = "NewUsername",
                Password = "TestPassword1"
            };

            var response = service.UpdateUsername(request, 2);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(user2.Username == request.NewUsername);
            Assert.True(user2.ModifiedBy == request.NewUsername);
        }

        [Fact]
        public void UpdatePasswordFailPasswordMissing()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var mockUserDataLayer = new MockUserDataLayer();

            var user1 = new User()
            {
                CreatedBy = "UnitTest",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Password = "TestPassword",
                Salter = "TestSalt",
                UserId = 1,
                Username = "OriginalUser"
            };

            var user2 = new User()
            {
                CreatedBy = "UnitTest",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Password = "U5Suy6JmLuYeztykx//RV0K/kaknxGiHt8xVNzD9s7w=",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 2,
                Username = "TestUser",
            };

            mockUserDataLayer.Users = new List<User>();
            mockUserDataLayer.Users.Add(user1);
            mockUserDataLayer.Users.Add(user2);

            mockUserUnitOfWork.MockUserDataLayer = mockUserDataLayer;
            mockUserUnitOfWork.MockRoleDataLayer = new MockRoleDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);

            var request = new UpdatePasswordRequest()
            {
                NewPassword = "NewPassword",
                NewPasswordConfirmation = "NewPassword",
            };

            var response = service.UpdatePassword(request, 2);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "Current password required");

            request.Password = "";
            response = service.UpdatePassword(request, 2);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "Current password required");
        }

        [Fact]
        public void UpdatePasswordFailNewPasswordMissing()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var mockUserDataLayer = new MockUserDataLayer();

            var user1 = new User()
            {
                CreatedBy = "UnitTest",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Password = "TestPassword",
                Salter = "TestSalt",
                UserId = 1,
                Username = "OriginalUser"
            };

            var user2 = new User()
            {
                CreatedBy = "UnitTest",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Password = "U5Suy6JmLuYeztykx//RV0K/kaknxGiHt8xVNzD9s7w=",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 2,
                Username = "TestUser",
            };

            mockUserDataLayer.Users = new List<User>();
            mockUserDataLayer.Users.Add(user1);
            mockUserDataLayer.Users.Add(user2);

            mockUserUnitOfWork.MockUserDataLayer = mockUserDataLayer;
            mockUserUnitOfWork.MockRoleDataLayer = new MockRoleDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);

            var request = new UpdatePasswordRequest()
            {
                Password = "OldPassword",
                NewPasswordConfirmation = "NewPassword",
            };

            var response = service.UpdatePassword(request, 2);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "New password required");

            request.NewPassword = "";
            response = service.UpdatePassword(request, 2);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "New password required");
        }

        [Fact]
        public void UpdatePasswordFailNewPasswordConfirmationMissing()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var mockUserDataLayer = new MockUserDataLayer();

            var user1 = new User()
            {
                CreatedBy = "UnitTest",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Password = "TestPassword",
                Salter = "TestSalt",
                UserId = 1,
                Username = "OriginalUser"
            };

            var user2 = new User()
            {
                CreatedBy = "UnitTest",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Password = "U5Suy6JmLuYeztykx//RV0K/kaknxGiHt8xVNzD9s7w=",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 2,
                Username = "TestUser",
            };

            mockUserDataLayer.Users = new List<User>();
            mockUserDataLayer.Users.Add(user1);
            mockUserDataLayer.Users.Add(user2);

            mockUserUnitOfWork.MockUserDataLayer = mockUserDataLayer;
            mockUserUnitOfWork.MockRoleDataLayer = new MockRoleDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);

            var request = new UpdatePasswordRequest()
            {
                Password = "OldPassword",
                NewPassword = "NewPassword",
            };

            var response = service.UpdatePassword(request, 2);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "New password confirmation required");

            request.NewPasswordConfirmation = "";
            response = service.UpdatePassword(request, 2);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "New password confirmation required");
        }

        [Fact]
        public void UpdatePasswordFailWeakPasswordTooShort()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.UpdatePassword(new UpdatePasswordRequest()
            {
                NewPasswordConfirmation = "Pass1",
                NewPassword = "Pass1",
                Password = "OldPassword1",
            }, 1);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "New password is not strong enough");
        }

        [Fact]
        public void UpdatePasswordFailWeakPasswordNoNumber()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.UpdatePassword(new UpdatePasswordRequest()
            {
                NewPassword = "NewPassword",
                NewPasswordConfirmation = "NewPassword",
                Password = "TestPassword",
            }, 1);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "New password is not strong enough");
        }

        [Fact]
        public void UpdatePasswordFailPasswordMismatch()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer();
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.UpdatePassword(new UpdatePasswordRequest()
            {
                NewPassword = "NewPassword1",
                NewPasswordConfirmation = "NewPword1",
                Password = "TestPassword1",
            }, 1);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "New password confirmation does not match");
        }

        [Fact]
        public void UpdatePasswordPass()
        {
            User testUser = new User()
            {
                CreatedBy = "UnitTest",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Password = "U5Suy6JmLuYeztykx//RV0K/kaknxGiHt8xVNzD9s7w=",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 1,
                Username = "TestUser"
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer()
            {
                Users = new List<User>()
                {
                    testUser
                }
            };
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var request = new UpdatePasswordRequest()
            {
                NewPassword = "NewPassword1",
                NewPasswordConfirmation = "NewPassword1",
                Password = "TestPassword1",
            };

            var response = service.UpdatePassword(request, 1);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(testUser.Password == HashPassword(request.Password, testUser.Salter));
        }

        [Fact]
        public void UpdateUserSuccess()
        {
            var testRole1 = new Role()
            {
                Name = "Test Role 1",
                RoleId = 1,
            };

            var testRole2 = new Role()
            {
                RoleId = 2,
                Name = "Test Role 2",
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var testUser = new User
            {
                CreatedBy = "Unit Test",
                CreatedDate = new DateTime(),
                IsActive = true,
                IsDeleted = false,
                Password = "T3stPassword1",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 1,
                Username = "TestUsr",
                UserRole = new List<UserRole>()
                {
                    new UserRole()
                    {
                        UserId = 1,
                        RoleId = 1,
                        UserRoleId = 1,
                        Role = testRole1
                    }
                }
            };

            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer()
            {
                Users = new List<User>()
                {
                    testUser,
                }
            };

            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);

            var request = new UpdateUserRequest()
            {
                UserID = 1,
                Username = "TestUser",
                ConfirmationPassword = "TestPassword1",
                Password = "TestPassword1",
                Roles = new List<Common.DTOs.RoleDTO>()
                {
                    new Common.DTOs.RoleDTO()
                    {
                        Name = testRole2.Name,
                        RoleId = testRole2.RoleId,
                    }
                }
            };

            var response = service.UpdateUser(request);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(testUser.Password == HashPassword(request.Password, testUser.Salter));
            Assert.True(testUser.ModifiedBy == MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value);
            Assert.True(testUser.UserRole.FirstOrDefault().RoleId == testRole2.RoleId);
        }

        [Fact]
        public void UpdateUserSuccessSameUsername()
        {
            var testRole1 = new Role()
            {
                Name = "Test Role 1",
                RoleId = 1,
            };

            var testRole2 = new Role()
            {
                RoleId = 2,
                Name = "Test Role 2",
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var testUser = new User
            {
                CreatedBy = "Unit Test",
                CreatedDate = new DateTime(),
                IsActive = true,
                IsDeleted = false,
                Password = "T3stPassword1",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 1,
                Username = "TestUser",
                UserRole = new List<UserRole>()
                {
                    new UserRole()
                    {
                        UserId = 1,
                        RoleId = 1,
                        UserRoleId = 1,
                        Role = testRole1
                    }
                }
            };

            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer()
            {
                Users = new List<User>()
                {
                    testUser,
                }
            };

            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);

            var request = new UpdateUserRequest()
            {
                UserID = 1,
                Username = "TestUser",
                ConfirmationPassword = "TestPassword1",
                Password = "TestPassword1",
                Roles = new List<Common.DTOs.RoleDTO>()
                {
                    new Common.DTOs.RoleDTO()
                    {
                        Name = testRole2.Name,
                        RoleId = testRole2.RoleId,
                    }
                }
            };

            var response = service.UpdateUser(request);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(testUser.Password == HashPassword(request.Password, testUser.Salter));
            Assert.True(testUser.ModifiedBy == MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value);
            Assert.True(testUser.UserRole.FirstOrDefault().RoleId == testRole2.RoleId);
        }

        [Fact]
        public void UpdateUserSuccessNoPasswordChange()
        {
            var testRole1 = new Role()
            {
                Name = "Test Role 1",
                RoleId = 1,
            };

            var testRole2 = new Role()
            {
                RoleId = 2,
                Name = "Test Role 2",
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var testUser = new User
            {
                CreatedBy = "Unit Test",
                CreatedDate = new DateTime(),
                IsActive = true,
                IsDeleted = false,
                Password = "T3stPassword1",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 1,
                Username = "TestUsr",
                UserRole = new List<UserRole>()
                {
                    new UserRole()
                    {
                        UserId = 1,
                        RoleId = 1,
                        UserRoleId = 1,
                        Role = testRole1
                    }
                }
            };

            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer()
            {
                Users = new List<User>()
                {
                    testUser,
                }
            };

            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);

            var request = new UpdateUserRequest()
            {
                UserID = 1,
                Username = "TestUser",
                Roles = new List<Common.DTOs.RoleDTO>()
                {
                    new Common.DTOs.RoleDTO()
                    {
                        Name = testRole2.Name,
                        RoleId = testRole2.RoleId,
                    }
                }
            };

            var response = service.UpdateUser(request);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(testUser.ModifiedBy == MockPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value);
            Assert.True(testUser.UserRole.FirstOrDefault().RoleId == testRole2.RoleId);
        }

        [Fact]
        public void UpdateUserFailNoUsername()
        {
            var testRole1 = new Role()
            {
                Name = "Test Role 1",
                RoleId = 1,
            };

            var testRole2 = new Role()
            {
                RoleId = 2,
                Name = "Test Role 2",
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var testUser = new User
            {
                CreatedBy = "Unit Test",
                CreatedDate = new DateTime(),
                IsActive = true,
                IsDeleted = false,
                Password = "T3stPassword1",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 1,
                Username = "Testusername",
                UserRole = new List<UserRole>()
                {
                    new UserRole()
                    {
                        UserId = 1,
                        RoleId = 1,
                        UserRoleId = 1,
                        Role = testRole1
                    }
                }
            };

            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer()
            {
                Users = new List<User>()
                {
                    testUser,
                }
            };

            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);

            var request = new UpdateUserRequest()
            {
                UserID = 1,
                Username = "",
                ConfirmationPassword = "TestPassword1",
                Password = "TestPassword1",
                Roles = new List<Common.DTOs.RoleDTO>()
                {
                    new Common.DTOs.RoleDTO()
                    {
                        Name = testRole2.Name,
                        RoleId = testRole2.RoleId,
                    }
                }
            };

            var response = service.UpdateUser(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "User must have a username");
        }

        [Fact]
        public void UpdateUserFailUsernameTaken()
        {
            var testRole1 = new Role()
            {
                Name = "Test Role 1",
                RoleId = 1,
            };

            var testRole2 = new Role()
            {
                RoleId = 2,
                Name = "Test Role 2",
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var testUser = new User
            {
                CreatedBy = "Unit Test",
                CreatedDate = new DateTime(),
                IsActive = true,
                IsDeleted = false,
                Password = "T3stPassword1",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 1,
                Username = "Testusername",
                UserRole = new List<UserRole>()
                {
                    new UserRole()
                    {
                        UserId = 1,
                        RoleId = 1,
                        UserRoleId = 1,
                        Role = testRole1
                    }
                }
            };

            var testUser2 = new User
            {
                CreatedBy = "Unit Test",
                CreatedDate = new DateTime(),
                IsActive = true,
                IsDeleted = false,
                Password = "T3stPassword1",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 2,
                Username = "Testusername2",
                UserRole = new List<UserRole>()
                {
                    new UserRole()
                    {
                        UserId = 1,
                        RoleId = 1,
                        UserRoleId = 1,
                        Role = testRole2
                    }
                }
            };

            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer()
            {
                Users = new List<User>()
                {
                    testUser,
                    testUser2,
                }
            };

            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);

            var request = new UpdateUserRequest()
            {
                UserID = 1,
                Username = "Testusername2",
                ConfirmationPassword = "TestPassword1",
                Password = "TestPassword1",
                Roles = new List<Common.DTOs.RoleDTO>()
                {
                    new Common.DTOs.RoleDTO()
                    {
                        Name = testRole2.Name,
                        RoleId = testRole2.RoleId,
                    }
                }
            };

            var response = service.UpdateUser(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "Username is already taken");
        }

        [Fact]
        public void UpdateUserFailNoRole()
        {
            var testRole1 = new Role()
            {
                Name = "Test Role 1",
                RoleId = 1,
            };

            var testRole2 = new Role()
            {
                RoleId = 2,
                Name = "Test Role 2",
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var testUser = new User
            {
                CreatedBy = "Unit Test",
                CreatedDate = new DateTime(),
                IsActive = true,
                IsDeleted = false,
                Password = "T3stPassword1",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 1,
                Username = "Testusername",
                UserRole = new List<UserRole>()
                {
                    new UserRole()
                    {
                        UserId = 1,
                        RoleId = 1,
                        UserRoleId = 1,
                        Role = testRole1
                    }
                }
            };

            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer()
            {
                Users = new List<User>()
                {
                    testUser,
                }
            };

            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);

            var request = new UpdateUserRequest()
            {
                UserID = 1,
                Username = "TestUsername",
                ConfirmationPassword = "TestPassword1",
                Password = "TestPassword1",
                Roles = new List<Common.DTOs.RoleDTO>()
            };

            var response = service.UpdateUser(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "User must have a role");
        }

        [Fact]
        public void UpdateUserFailWeakPasswordNoNumbers()
        {
            var testRole1 = new Role()
            {
                Name = "Test Role 1",
                RoleId = 1,
            };

            var testRole2 = new Role()
            {
                RoleId = 2,
                Name = "Test Role 2",
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var testUser = new User
            {
                CreatedBy = "Unit Test",
                CreatedDate = new DateTime(),
                IsActive = true,
                IsDeleted = false,
                Password = "TestPassword1",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 1,
                Username = "Testusername",
                UserRole = new List<UserRole>()
                {
                    new UserRole()
                    {
                        UserId = 1,
                        RoleId = 1,
                        UserRoleId = 1,
                        Role = testRole1
                    }
                }
            };

            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer()
            {
                Users = new List<User>()
                {
                    testUser,
                }
            };

            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);

            var request = new UpdateUserRequest()
            {
                UserID = 1,
                Username = "TestUser",
                ConfirmationPassword = "TestPassword",
                Password = "TestPassword",
                Roles = new List<Common.DTOs.RoleDTO>()
                {
                    new Common.DTOs.RoleDTO()
                    {
                        Name = testRole2.Name,
                        RoleId = testRole2.RoleId,
                    }
                }
            };

            var response = service.UpdateUser(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "Password is not strong enough");
        }

        [Fact]
        public void UpdateUserFailWeakPasswordTooShort()
        {
            var testRole1 = new Role()
            {
                Name = "Test Role 1",
                RoleId = 1,
            };

            var testRole2 = new Role()
            {
                RoleId = 2,
                Name = "Test Role 2",
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var testUser = new User
            {
                CreatedBy = "Unit Test",
                CreatedDate = new DateTime(),
                IsActive = true,
                IsDeleted = false,
                Password = "TestPassword1",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 1,
                Username = "Testusername",
                UserRole = new List<UserRole>()
                {
                    new UserRole()
                    {
                        UserId = 1,
                        RoleId = 1,
                        UserRoleId = 1,
                        Role = testRole1
                    }
                }
            };

            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer()
            {
                Users = new List<User>()
                {
                    testUser,
                }
            };

            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);

            var request = new UpdateUserRequest()
            {
                UserID = 1,
                Username = "TestUser",
                ConfirmationPassword = "Pwrd1",
                Password = "Pwrd1",
                Roles = new List<Common.DTOs.RoleDTO>()
                {
                    new Common.DTOs.RoleDTO()
                    {
                        Name = testRole2.Name,
                        RoleId = testRole2.RoleId,
                    }
                }
            };

            var response = service.UpdateUser(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "Password is not strong enough");
        }

        [Fact]
        public void UpdateUserFailPasswordMismatch()
        {
            var testRole1 = new Role()
            {
                Name = "Test Role 1",
                RoleId = 1,
            };

            var testRole2 = new Role()
            {
                RoleId = 2,
                Name = "Test Role 2",
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            var testUser = new User
            {
                CreatedBy = "Unit Test",
                CreatedDate = new DateTime(),
                IsActive = true,
                IsDeleted = false,
                Password = "TestPassword1",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 1,
                Username = "Testusername",
                UserRole = new List<UserRole>()
                {
                    new UserRole()
                    {
                        UserId = 1,
                        RoleId = 1,
                        UserRoleId = 1,
                        Role = testRole1
                    }
                }
            };

            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer()
            {
                Users = new List<User>()
                {
                    testUser,
                }
            };

            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);

            var request = new UpdateUserRequest()
            {
                UserID = 1,
                Username = "TestUser",
                ConfirmationPassword = "TestPassword2",
                Password = "TestPassword1",
                Roles = new List<Common.DTOs.RoleDTO>()
                {
                    new Common.DTOs.RoleDTO()
                    {
                        Name = testRole2.Name,
                        RoleId = testRole2.RoleId,
                    }
                }
            };

            var response = service.UpdateUser(request);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(response.Messages[0] == "Password do not match");
        }

        [Fact]
        public void DeleteUserFailNotFound()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer()
            {
                Users = new List<User>()
                {
                    new User()
                    {
                        CreatedBy = "UnitTest",
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        Password = "U5Suy6JmLuYeztykx//RV0K/kaknxGiHt8xVNzD9s7w=",
                        Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                        UserId = 1,
                        Username = "TestUser"
                    },
                }
            };
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.DeleteUser(2);

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void DeleteUserPass()
        {
            User user = new User()
            {
                CreatedBy = "UnitTest",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Password = "U5Suy6JmLuYeztykx//RV0K/kaknxGiHt8xVNzD9s7w=",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 1,
                Username = "TestUser"
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer()
            {
                Users = new List<User>()
                {
                    user,
                }
            };
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.DeleteUser(1);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(user.IsDeleted);
            Assert.True(user.Username == "Deleted");
        }

        [Fact]
        public void DeactivateUserFailNotFound()
        {
            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer()
            {
                Users = new List<User>()
                {
                    new User()
                    {
                        CreatedBy = "UnitTest",
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        Password = "U5Suy6JmLuYeztykx//RV0K/kaknxGiHt8xVNzD9s7w=",
                        Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                        UserId = 1,
                        Username = "TestUser"
                    },
                }
            };
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.DeactivateUser(2);

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public void DeactivateUserPass()
        {
            User user = new User()
            {
                CreatedBy = "UnitTest",
                CreatedDate = DateTime.Now,
                IsActive = true,
                Password = "U5Suy6JmLuYeztykx//RV0K/kaknxGiHt8xVNzD9s7w=",
                Salter = "lXCaZkEU8/CyYuvmSs2P2g==",
                UserId = 1,
                Username = "TestUser"
            };

            var mockUserUnitOfWork = new MockUserUnitOfWork();
            mockUserUnitOfWork.MockUserDataLayer = new MockUserDataLayer()
            {
                Users = new List<User>()
                {
                    user,
                }
            };
            var service = new UserService(mockUserUnitOfWork, Configuration, MockPrincipal);
            var response = service.DeactivateUser(1);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.False(user.IsActive);
        }

        private string HashPassword(string password, string salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}
