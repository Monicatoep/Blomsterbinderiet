using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Admin
{
    public class DeactivateUserModel : PageModel
    {
		private UserService _userService;

		public DeactivateUserModel(UserService userService)
		{
			_userService = userService;
		}

		[BindProperty]
		public User User { get; set; }

		public async Task<IActionResult> OnGetAsync(int id)
		{
			User = await _userService.GetUserByIdAsync(id);
			if (User == null)
			{
				return RedirectToPage("/NotFound");
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int id)
		{
			User = await _userService.GetUserByIdAsync(id);
            User.State = "Deaktiveret";
			await _userService.UpdateUserAsync(User);

			return RedirectToPage("/Admin/GetAllUsers");
		}
    }
}
