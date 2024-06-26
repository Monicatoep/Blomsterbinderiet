using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Service;
using Blomsterbinderiet.Models;

namespace Blomsterbinderiet.Pages.UserPages
{
    public class UpdateUserModel : PageModel
    {
        private UserService UserService { get; set; }
        public string Message { get; set; }
        [BindProperty]
        public User User { get; set; }

        public UpdateUserModel(UserService userService)
        {
            UserService = userService;
        }

        public async Task OnGetAsync()
        {
            User = await UserService.GetUserByHttpContextAsync(HttpContext);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            User.Email = User.Email.Trim();
            await UserService.UpdateUserAsync(User, new List<string>() { nameof(User.Name), nameof(User.Phone), nameof(User.Address) });
            Message = "Opdaterede din profil";
            return Page();
        }
    }
}
