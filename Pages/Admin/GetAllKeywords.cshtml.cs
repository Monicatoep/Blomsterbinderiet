using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class GetAllKeywordsModel : PageModel
    { 
        public ProductService ProductService { get; set; }
        public IEnumerable<Keyword> Keywords { get; set; }

        public GetAllKeywordsModel(ProductService productService)
        {
            ProductService = productService;
        }

        public void OnGet()
        {
        }
    }
}
