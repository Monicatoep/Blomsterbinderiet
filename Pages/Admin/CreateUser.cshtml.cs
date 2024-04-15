using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Blomsterbinderiet.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class CreateUserModel : PageModel
    {
        private UserService _userService;

        private PasswordHasher<string> _passwordHasher;

        [BindProperty]
        [Required(ErrorMessage = "Du skal indtaste en e-mailadresse")]
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

        public CreateUserModel(UserService userService, PasswordHasher<string> passwordHasher)
        {
            _userService = userService;
            this._passwordHasher = passwordHasher;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _userService.AddUserAsync(new User(_passwordHasher.HashPassword(null, Password), Role, Email, Phone, Address));
            return RedirectToPage("/Index");
        }

        public void OnGet()
        {
        }
    }
}
