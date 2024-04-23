using Blomsterbinderiet.Migrations;
using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Admin
{
    public class GetAllUsersModel : PageModel
    {
        public UserService UserService { get; set; }
        public IEnumerable<User> Users { get; set; }

        public GetAllUsersModel(UserService userService)
        {
            UserService = userService;
        }

        public IActionResult OnGet()
        {
            Users = UserService.GetAllUsers();

            return Page();
        }

        public IActionResult OnGetSortByName()
        {
            Users = UserService.SortByName().ToList();
            return Page();
        }

        public IActionResult OnGetSortByNameDescending()
        {
            Users = UserService.SortByNameDescending().ToList();
            return Page();
        }
        public IActionResult OnGetSortByRole()
        {
            Users = UserService.SortByRole().ToList();
            return Page();
        }

        public IActionResult OnGetSortByRoleDescending()
        {
            Users = UserService.SortByRoleDescending().ToList();
            return Page();
        }
    }
}
