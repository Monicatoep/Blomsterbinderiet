using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Service;
using Blomsterbinderiet.Models;
using System.Security.Claims;

namespace Blomsterbinderiet.Pages.UserPages
{
    public class UpdateUserModel : PageModel
    {
        public DbGenericService<User> UserService { get; set; }
        public string Message { get; set; }

        [BindProperty]
        public User User { get; set; }

        public UpdateUserModel(DbGenericService<User> userService)
        {
            UserService = userService;
        }

        public void OnGet()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                if (userId != null)
                {
                    User = UserService.GetObjectByIdAsync(Convert.ToInt32(userId)).Result;
                }
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            await UserService.UpdateObjectAsync(User, new List<string>() { nameof(User.Name), nameof(User.Phone), nameof(User.Address) });
            Message = "Opdaterede din profil";
            return Page();
        }
    }
}
