using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Admin
{
    public class UpdateUserModel : PageModel
    {
        public UserService UserService { get; set; }

        [BindProperty]
        public User User { get; set; }

        public UpdateUserModel(UserService userService)
        {
            UserService = userService;
        }

        public IActionResult OnGet(int id)
        {
            User = UserService.GetUserByIdAsync(id);
            if (User == null) 
            { 
                return RedirectToPage("GetAllUsers"); 
            }
            return Page();
        }

        public IActionResult OnPostAsync()
        {
            Console.WriteLine(User);
            UserService.UpdateUser(User);
            return RedirectToPage("GetAllUsers");
        }
    }
}
