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

        public CookieService CookieService { get; set; }

        public LoginModel(UserService userService, CookieService cookieService)
        {
            _userService = userService;
            this.CookieService = cookieService;
        }

        public void OnGet()
        {
        }  

        public async Task<IActionResult> OnPostAsync()
        {
            if (Email == null || Password == null)
            {
                Message = "Invalid attempt";
                return Page(); 
            }
            
            ClaimsIdentity identity = await CookieService.LoginAsync(await _userService.GetAllUsersAsync(), Email, Password);
            if(identity != null)
            {
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                return RedirectToPage("/index");
            }
            Message = "Invalid attempt";
            return Page();
        }
    }
}
