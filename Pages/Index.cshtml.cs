using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public ProductService ProductService;

        [BindProperty]
        public string SearchString { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ProductService productService)
        {
            ProductService = productService;
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/Product/GetAllProducts", "SearchString", new { searchString = SearchString });
        }
    }
}
