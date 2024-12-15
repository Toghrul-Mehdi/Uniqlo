using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Net.Mail;
using System.Net;
using System;
using Uniqlo.Models;
using Uniqlo.Helpers;
using Uniqlo.ViewModel.Auths;

namespace Uniqlo.Controllers
{
    public class AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IOptions<SmtpOptions> options) : Controller
    {
        readonly SmtpOptions _smtp = options.Value;
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Register(UserCreateVM vm)
        {
            if (!ModelState.IsValid)    
                return View();
            User user = new User
            {
                Email = vm.Email,
                Fullname = vm.Fullname,
                UserName = vm.Username,
                ProfileImageUrl = "photo.jpg"
            };
            var result = await userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]


        public async Task<IActionResult> Login(LoginVM vm, string? returnUrl)
        {
            if (!ModelState.IsValid) return View();
            User? user = null;
            if (vm.UsernameOrEmail.Contains("@"))
            {
                user = await userManager.FindByEmailAsync(vm.UsernameOrEmail);
            }
            else
            {
                user = await userManager.FindByNameAsync(vm.UsernameOrEmail);
            }
            if (user is null)
            {
                ModelState.AddModelError("", "Username or password is wrong!");
                return View();
            }
            var result = await signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);
            if (!result.Succeeded)
            {
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Username or password is wrong");
                }
                if (!result.IsLockedOut)
                {
                    ModelState.AddModelError("", "wait until" + user.LockoutEnd!.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                return View();
            }
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return LocalRedirect(returnUrl);
            }
        }


        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }


        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("E-posta tapilmadi.");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action(
                action: "ResetPassword",
                controller: "Account",
                values: new { token, email = user.Email },
                protocol: "https");

            SmtpClient smtp = new SmtpClient
            {
                Host = _smtp.Host,
                Port = _smtp.Port,
                EnableSsl = true,
                Credentials = new NetworkCredential(_smtp.Username, _smtp.Password)
            };

            MailMessage msg = new MailMessage
            {
                From = new MailAddress(_smtp.Username, "Togrul Mehdiyev CodeAcademy"),
                Subject = "Reset Password",
                Body = $"<p>Parolu deyismek ucun <a href='{resetLink}'>bu linke</a> klikleyin.</p>",
                IsBodyHtml = true
            };
            msg.To.Add(email);

            smtp.Send(msg);
            return Ok("Emaile gonderildi");
        }

        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid Connection");
            }

            return View(new ResetPasswordVM { Token = token, Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await userManager.FindByEmailAsync(vm.Email);
            if (user == null)
            {
                return BadRequest("Istifadeci tapilmadi");
            }

            var resetPassResult = await userManager.ResetPasswordAsync(user, vm.Token, vm.NewPassword);
            if (resetPassResult.Succeeded)
            {
                return Ok("Hershey ugurla tamamlandi");
            }

            foreach (var error in resetPassResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(vm);
        }





    }
}
