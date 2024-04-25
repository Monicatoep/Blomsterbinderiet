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

		public IActionResult OnGet(int id)
		{
			User = _userService.GetUserByIdAsync(id);
			if (User == null)
				return RedirectToPage("/NotFound");

			return Page();
		}

		public IActionResult OnPost(int id)
		{
			User = _userService.GetUserByIdAsync(id);
            User.State = "Deaktiveret";
			_userService.UpdateUser(User);

			return RedirectToPage("/Admin/GetAllUsers");
		}
    }
}
