using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages
{
    public class ProductDetailsModel : PageModel
    {
        [BindProperty]
        public int Amount { get; set; }
        public void OnGet()
        {
        }
    }
}
