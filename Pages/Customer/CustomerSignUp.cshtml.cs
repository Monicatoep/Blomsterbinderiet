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
        [Required(ErrorMessage = "Du skal indtaste et navn")]
        public string Name { get; set; }

        [BindProperty]
        [Required (ErrorMessage = "Du skal indtaste en e-mailadresse")]
        public string Email { get; set; }

        [BindProperty, DataType(DataType.Password)]
        [Required(ErrorMessage = "Du skal indtaste et password")]
        public string Password { get; set; }

        
        public string Role { get; set; }
       
        [BindProperty]
        [MinLength(8, ErrorMessage = "Telefonnummer skal minimum være 8 tegn")]
        [MaxLength(12, ErrorMessage = "Telefonnummer skal maksimalt være 12 tegn")]
        [Required(ErrorMessage = "Du skal indtaste et telefonnummer")]
        public string Phone { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Du skal indtaste en adresse")]
        public string Address { get; set; }

        [BindProperty]
        public string State { get; set; }


        private UserService UserService;

        private PasswordHasher<string> PasswordHasher;

        public CustomerSignUpModel(UserService userService)
        {
            UserService = userService;
            PasswordHasher = new PasswordHasher<string>();
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
            State = "Activ";
            await UserService.AddUserAsync(new User(Name, PasswordHasher.HashPassword(null, Password), Role, Email, Phone, Address, State));
            return RedirectToPage("/UserPages/RegisterSuccess");
        }
    }
}
