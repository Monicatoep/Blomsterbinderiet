using Blomsterbinderiet.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Blomsterbinderiet.Service;

namespace Blomsterbinderiet.Pages.Customer
{
    public class LoginModel : PageModel
    {       
        private UserService _userService;

        [BindProperty]
        [Required]
        public string Email { get; set; }

        [BindProperty, DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        public string Message { get; set; }
        public string ID { get; set; }

        public LoginModel(UserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
        }  

        public async Task<IActionResult> OnPost()
        {
            if (Email == null || Password == null)
            {
                Message = "Invalid attempt";
                return Page(); 
            }
            
            List<User> users = _userService.Users;
            foreach (User user in users)
            {
                if (Email == user.Email)
                {
                    if(user.State == "Aktiv")
                    {
                        var passwordHasher = new PasswordHasher<string>();
                        if (passwordHasher.VerifyHashedPassword(null, user.Password, Password) == PasswordVerificationResult.Success)
                        {
                            ID = user.ID.ToString();
                            var claims = new List<Claim> { new Claim(ClaimTypes.Name, ID) };
                            claims.Add(new Claim(ClaimTypes.Role, user.Role));
                            claims.Add(new Claim(ClaimTypes.Email, Email));

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                            return RedirectToPage("/index");
                        }
                    }
                }
            }
            Message = "Invalid attempt";
            return Page();
        }
    }
}
