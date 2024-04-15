using Blomsterbinderiet.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Blomsterbinderiet.Pages.Customer
{
    public class LoginModel : PageModel
    {       
        private UserService _userService;

        [BindProperty]
        public string Email { get; set; }

        [BindProperty, DataType(DataType.Password)]
        public string Password { get; set; }
        public string Message { get; set; }

        public LoginPageModel(UserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {

            List<User> users = _userService.Users;
            foreach (User user in users)
            {

                if (Email == user.Email)
                {
                    var passwordHasher = new PasswordHasher<string>();
                    if (passwordHasher.VerifyHashedPassword(null, user.Password, Password) == PasswordVerificationResult.Success)
                    {

                        //LoggedInUser = user;

                        var claims = new List<Claim> { new Claim(ClaimTypes.Name, Email) };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity);
                            
                        return RedirectToPage("/");
                    }
                }

            }

            Message = "Invalid attempt";
            return Page();
        }
     
    }
}
