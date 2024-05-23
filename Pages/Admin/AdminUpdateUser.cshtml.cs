using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminUpdateUserModel : PageModel
    {
        private UserService UserService { get; set; }
        
        [BindProperty]
        public User User { get; set; }

        public AdminUpdateUserModel(UserService userService)
        {
            UserService = userService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            User = await UserService.GetUserByIdAsync(id);
            if (User == null)
            {
                return RedirectToPage("GetAllUsers");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            User.Password = User.Password.Trim();
            User.Email = User.Email.Trim();
            await UserService.UpdateUserAsync(User, new List<string>() { nameof(User.Name), nameof(User.Email), nameof(User.Phone), nameof(User.Address), nameof(User.Role) });
            return RedirectToPage("GetAllUsers");
        }
    }
}
