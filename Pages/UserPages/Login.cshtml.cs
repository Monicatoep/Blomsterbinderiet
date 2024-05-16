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
        private UserService UserService { get; set; }
        private CookieService CookieService { get; set; }
        [BindProperty]
        [Required]
        public string Email { get; set; }
        [BindProperty, DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        public string Message { get; set; }
        public string ID { get; set; }        

        public LoginModel(UserService userService, CookieService cookieService)
        {
            UserService = userService;
            this.CookieService = cookieService;
        }

        public void OnGet()
        {
        }  

        public async Task<IActionResult> OnPostAsync()
        {
            if (Email == null || Password == null)
            {
                Message = "Login fejlede";
                return Page(); 
            }
            
            ClaimsIdentity identity = await CookieService.Login(await UserService.GetAllUsersAsync(), Email, Password);
            if(identity != null)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                return RedirectToPage("/index");
            }
            Message = "Login fejlede";
            return Page();
        }
    }
}
