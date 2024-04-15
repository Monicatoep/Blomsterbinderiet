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
    public class CustomerSignUpModel : PageModel
    {
        [BindProperty]
        [Required (ErrorMessage = "Du skal indtaste en e-mailadresse")]
        public string Email { get; set; }

        [BindProperty, DataType(DataType.Password)]
        [Required(ErrorMessage = "Du skal indtaste et password")]
        public string Password { get; set; }

        
        public string Role { get; set; }
       
        [BindProperty]
        [Required(ErrorMessage = "Du skal indtaste et telefonnummer")]
        public string Phone { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Du skal indtaste en adresse")]
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

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Role = "Customer";
            await _userService.AddUserAsync(new User(passwordHasher.HashPassword(null, Password), Role, Email, Phone, Address));
            return RedirectToPage("/Customer/RegisterSuccess");
        }
    }
}
