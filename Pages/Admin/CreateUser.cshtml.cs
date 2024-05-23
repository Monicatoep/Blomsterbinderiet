using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Blomsterbinderiet.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class CreateUserModel : PageModel
    {
        private UserService UserService { get; set; }
        
        private PasswordHasher<string> PasswordHasher { get; set; }
        
        [BindProperty]
        [Required(ErrorMessage = "Du skal indtaste et navn")]
        public string Name { get; set; }
        
        [BindProperty]
        [Required(ErrorMessage = "Du skal indtaste en e-mailadresse")]
        public string Email { get; set; }
        
        [BindProperty, DataType(DataType.Password)]
        [DisplayName("Adgangskode")]
        [Required(ErrorMessage = "Du skal indtaste et password")]
        public string Password { get; set; }
        
        [BindProperty]
        [DisplayName("Rolle")]
        [Required(ErrorMessage ="Du skal vælge rolle")]
        public string Role { get; set; }
        
        [BindProperty]
        [DisplayName("Telefonnummer")]
        [MinLength(8, ErrorMessage = "Telefonnummer skal minimum være 8 tegn")]
        [MaxLength(12, ErrorMessage = "Telefonnummer skal maksimalt være 12 tegn")]
        [Required(ErrorMessage = "Du skal indtaste et telefonnummer")]
        public string Phone { get; set; }
        
        [BindProperty]
        [DisplayName("Adresse")]
        [Required(ErrorMessage = "Du skal indtaste en adresse")]
        public string Address { get; set; }
        
        public string State { get; set; }

        public CreateUserModel(UserService userService)
        {
            UserService = userService;
            this.PasswordHasher = new PasswordHasher<string>();
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
            Password = Password.Trim();
            Email = Email.Trim();
            await UserService.AddUserAsync(new User(Name,PasswordHasher.HashPassword(null, Password), Role, Email, Phone, Address));
            return RedirectToPage("/Admin/CreateUserSuccess");
        }
    }
}
