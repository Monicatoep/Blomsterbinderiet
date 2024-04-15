using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public void OnGet(int id)
        {
            User = DBService.GetObjectByIdAsync(id).Result;
        }
    }
}
