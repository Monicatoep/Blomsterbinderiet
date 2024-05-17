using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DeactivateUserModel : PageModel
    {
		private UserService UserService { get; set; }
        
		[BindProperty]
        public User User { get; set; }

        public DeactivateUserModel(UserService userService)
		{
			UserService = userService;
		}

		public async Task<IActionResult> OnGetAsync(int id)
		{
			User = await UserService.GetUserByIdAsync(id);
			if (User == null)
			{
				return RedirectToPage("/NotFound");
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			await UserService.DeactivateUserAsync(id);
            return RedirectToPage("/Admin/GetAllUsers");
		}
    }
}
