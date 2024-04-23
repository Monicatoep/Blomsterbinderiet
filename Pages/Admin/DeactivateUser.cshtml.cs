using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Admin
{
    public class DeactivateUserModel : PageModel
    {
		private UserService UserService;

		public DeactivateUserModel(UserService userService)
		{
			UserService = userService;
		}

		[BindProperty]
		public User User { get; set; }


		public IActionResult OnGet(int id)
		{
			User = UserService.GetUserByIdAsync(id);
			if (User == null)
				return RedirectToPage("/NotFound");

			return Page();
		}

		public IActionResult OnPost()
		{
			User.State = "Deaktiveret";
			UserService.

			return RedirectToPage("/Admin/GetAllUsers");
		}
    }
}
