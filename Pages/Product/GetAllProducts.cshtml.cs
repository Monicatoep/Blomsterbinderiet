using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Models;
using Blomsterbinderiet.MockData;
using Blomsterbinderiet.Service;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Blomsterbinderiet.Pages.Product
{
    public class GetAllProductsModel : PageModel
    {
        private ProductService ProductService { get; set; }

        [BindProperty]
        [DisplayName("Sorter efter")]
        public string? SortProperty { get; set; }
        [BindProperty]
        [DisplayName("Størst til mindst?")]
        public bool SortDirection { get; set; }
        [BindProperty]
        [DisplayName("Farve")]
        public string? Colour { get; set; }
        [BindProperty]
        public int? Price1 { get; set; }
        [BindProperty]
        public int? Price2 { get; set; }
        [BindProperty]
        public int MyProperty { get; set; }

        public GetAllProductsModel(ProductService Service)
        {
            ProductService = Service;
        }

        public IEnumerable<Models.Product> Products { get; private set; }

		public async Task OnGetAsync()
        {
            Products = (await ProductService.GetProductsAsync()).OrderBy(p => p.Name);
        }

        public async Task<IActionResult> OnPost()
        {
            List< Expression<Func<Models.Product, bool>>> conditions = new();
            if(Colour != null)
            {
                conditions.Add(p => p.Colour.Contains(Colour));
            }
            if(Price1 != null || Price2 != null)
            {
                int min = Math.Min(Convert.ToInt32(Price1), Convert.ToInt32(Price2));
                int maks = Math.Max(Convert.ToInt32(Price1), Convert.ToInt32(Price2));
                conditions.Add(p => p.Price >= min && p.Price <= maks);
            }

            Products = await ProductService.GetAllDataAsync(conditions);
            Products = await ProductService.OrderBy(Products, SortProperty, SortDirection);

            return Page();
        }
    }
}
