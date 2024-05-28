using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Customer
{
    [ModelMetadataType(typeof(Models.User))]
    public class CustomerSignUpModel : PageModel
    {
        private UserService UserService { get; set; }
        private PasswordHasher<string> PasswordHasher { get; set; }
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

        public CustomerSignUpModel(UserService userService)
        {
            UserService = userService;
            PasswordHasher = new PasswordHasher<string>();
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
            Email = Email.Trim();
            Password = Password.Trim();
            await UserService.AddUserAsync(new User(Name, PasswordHasher.HashPassword(null, Password), "Customer", Email, Phone, Address));
            return RedirectToPage("/UserPages/RegisterSuccess");
        }
    }
}
