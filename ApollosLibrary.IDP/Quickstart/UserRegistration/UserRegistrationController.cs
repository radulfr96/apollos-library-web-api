using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ApollosLibrary.IDP.Model;
using ApollosLibrary.IDP.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ApollosLibrary.IDP.UserRegistration
{
    public class UserRegistrationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IIdentityServerInteractionService _interactionService;

        public UserRegistrationController(IUserService userService, IIdentityServerInteractionService interactionService)
        {
            _userService = userService;
            _interactionService = interactionService;
        }

        [HttpGet]
        public IActionResult RegisterUser(string returnUrl)
        {
            var vm = new RegisterUserViewModel()
            {
                ReturnUrl = returnUrl
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = Guid.NewGuid();

            var user = new User()
            {
                CreatedBy = userId,
                CreatedDate = DateTime.Now,
                IsActive = false,
                Subject = Guid.NewGuid().ToString(),
                UserId = userId,
                Username = model.Email,
                UserClaims = new List<UserClaim>()
                {
                    new UserClaim()
                    {
                        Id = Guid.NewGuid(),
                        Type = "sub",
                        Value = "UnpaidAccount",
                    },
                },
            };

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                var codeData = new byte[128];
                randomNumberGenerator.GetBytes(codeData);
                user.SecurityCode = Convert.ToBase64String(codeData);
            }

            user.SecurityCodeExpirationDate = DateTime.Now.AddHours(1);

            await _userService.AddUser(user, model.Password);

            var link = Url.ActionLink("ActivateUser", "UserRegistration", new { securityCode = user.SecurityCode });

            Debug.WriteLine(link);

            return View("ActivationCodeSent");
        }

        [HttpGet]
        public async Task<IActionResult> ActivateUser(string securityCode)
        {
            if (await _userService.ActivateUser(securityCode))
            {
                ViewData["Message"] = "Your account was successfully activated.  " +
                    "Navigate to your client application to log in.";
            }
            else
            {
                ViewData["Message"] = "Your account couldn't be activated, " +
                    "please contact your administrator.";
            }

            return View();
        }
    }
}
