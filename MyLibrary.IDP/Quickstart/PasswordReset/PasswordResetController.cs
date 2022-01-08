using Microsoft.AspNetCore.Mvc;
using MyLibrary.IDP.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.IDP.PasswordReset
{
    public class PasswordResetController : Controller
    {
        private readonly IUserService _userService;

        public PasswordResetController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult RequestPasswordReset()
        {
            var vm = new PasswordResetRequestViewModel();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RequestPasswordReset(PasswordResetRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var securityCode = await _userService.InitiatePasswordResetRequest(model.Email);


            var link = $"{HttpContext.Request.Host.Host}{Url.Action("ResetPassword", "PasswordReset", new { securityCode })}";

            MailMessage message = new MailMessage("noreply@mylibrary.com", model.Email);

            string mailbody = $"Please click the following link to reset your password: <a href='/{link}'>{link}</a";
            message.Subject = "My Library Password Reset";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("127.0.0.1", 25); //smtp    
            //client.EnableSsl = true;
            client.UseDefaultCredentials = true;
            
            client.Send(message);

            return View("PasswordResetRequestSent");
        }

        [HttpGet]
        public IActionResult ResetPassword(string securityCode)
        {
            var vm = new ResetPasswordViewModel()
            {
                SecurityCode = securityCode
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _userService.SetPassword(model.SecurityCode, model.Password))
            {
                ViewData["Message"] = "Your password was successfully changed.  " +
                    "Navigate to your client application to log in.";
            }
            else
            {
                ViewData["Message"] = "Your password couldn't be changed, please" +
                    " contact your administrator.";
            }

            return View("ResetPasswordResult");
        }
    }
}
