using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Service;
using Blomsterbinderiet.Models;
using System.Security.Claims;

namespace Blomsterbinderiet.Pages.UserPages
{
    public class UpdateUserModel : PageModel
    {
        public UserService UserService { get; set; }
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
            await UserService.UpdateUserAsync(User, new List<string>() { nameof(User.Name), nameof(User.Phone), nameof(User.Address) });
            Message = "Opdaterede din profil";
            return Page();
        }
    }
}
