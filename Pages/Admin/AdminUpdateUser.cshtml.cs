using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Admin
{
    public class AdminUpdateUserModel : PageModel
    {
        public UserService UserService { get; set; }

        [BindProperty]
        public User User { get; set; }

        public AdminUpdateUserModel(UserService userService)
        {
            UserService = userService;
        }

        public IActionResult OnGet(int id)
        {
            User = UserService.GetUserByIdAsync(id);
            if (User == null)
            {
                return RedirectToPage("GetAllUsers");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            UserService.UpdateUser(User, new List<string>() { nameof(User.Name), nameof(User.Email), nameof(User.Phone), nameof(User.Address) });
            return RedirectToPage("GetAllUsers");
        }
    }
}
