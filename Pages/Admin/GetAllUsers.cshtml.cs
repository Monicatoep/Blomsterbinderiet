using Blomsterbinderiet.Migrations;
using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class GetAllUsersModel : PageModel
    {
        public UserService UserService { get; set; }
        public IEnumerable<User> Users { get; set; }

        public GetAllUsersModel(UserService userService)
        {
            UserService = userService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Users = await UserService.GetAllUsersAsync();

            return Page();
        }

        #region Name sorting
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
        #endregion

        #region Role sorting
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
        #endregion
    }
}
