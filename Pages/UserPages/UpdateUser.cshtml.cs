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

        public async Task OnGet()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                if (userId != null)
                {
                    User = await UserService.GetUserByIdAsync(userId);
                }
            }
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
