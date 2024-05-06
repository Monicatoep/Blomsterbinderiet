using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;

namespace Blomsterbinderiet.Pages.Customer
{
    [ModelMetadataType(typeof(Models.User))]
    public class CustomerSignUpModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }
       
        [BindProperty]
        public string Phone { get; set; }

        [BindProperty]
        public string Address { get; set; }


        private UserService _userService;

        private PasswordHasher<string> _passwordHasher;

        public CustomerSignUpModel(UserService userService)
        {
            _userService = userService;
            _passwordHasher = new PasswordHasher<string>();
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _userService.AddUserAsync(new User(Name, _passwordHasher.HashPassword(null, Password), "Customer", Email, Phone, Address));
            return RedirectToPage("/UserPages/RegisterSuccess");
        }
    }
}
