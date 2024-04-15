using Blomsterbinderiet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Blomsterbinderiet.Pages.Customer
{
    public class CustomerSignUpModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty, DataType(DataType.Password)]
        public string Password { get; set; }

        [BindProperty]
        public string Role { get; set; }

        [BindProperty]
        public string Phone { get; set; }

        [BindProperty]
        public string Address { get; set; }


        private UserService _userService;

        private PasswordHasher<string> passwordHasher;

        public CustomerSignUpModel(UserService userService)
        {
            _userService = userService;
            passwordHasher = new PasswordHasher<string>();
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Role = "Customer";
            _userService.AddUser(new User(UserName, passwordHasher.HashPassword(null, Password)));
            return RedirectToPage("/Pages/Customer/RegisterSuccess");
        }
    }
}
