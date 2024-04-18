using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Blomsterbinderiet.Pages.Customer
{
    public class ProfilePageModel : PageModel
    {
        public DbGenericService<Models.User> DBService { get; set; }
        public Models.User User { get; set; }

        public ProfilePageModel(DbGenericService<Models.User> service)
        {
            DBService = service;
        }

        public async Task OnGet()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                if (userId != null)
                {
                    User = await DBService.GetObjectByIdAsync(Convert.ToInt32(userId));
                }
            }
        }
    }
}
