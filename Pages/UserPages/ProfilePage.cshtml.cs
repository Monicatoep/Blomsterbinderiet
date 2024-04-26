using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Blomsterbinderiet.Pages.Customer
{
    public class ProfilePageModel : PageModel
    {
        public UserService UserService { get; set; }

        public Models.User User { get; set; }

        public ProfilePageModel(UserService userService)
        {
            UserService = userService;
        }

        public async Task OnGetAsync()
        {
            User = await UserService.GetUserByHttpContextAsync(HttpContext);
        }
    }
}
