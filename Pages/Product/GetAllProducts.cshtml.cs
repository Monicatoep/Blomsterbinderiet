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
        public ServiceGeneric<Keyword> KeywordService { get; set; }

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
        [DisplayName("Søg på produkt attribut")]
        public string? KeywordNameSearch { get; set; }

        public GetAllProductsModel(ProductService productService, ServiceGeneric<Keyword> keywordService)
        {
            ProductService = productService;
            KeywordService = keywordService;
        }

        public IEnumerable<Models.Product> Products { get; private set; }

		public async Task<IActionResult> OnGetAsync()
        {
            Products = (await ProductService.GetProductsAsync()).OrderBy(p => p.Name);
            return Page();
        }

        public async Task<IActionResult> OnGetKeywordAsync(string keywordName)
        {
            List<string> includeProperties = new()
            {
                nameof(Models.Product.Keywords)
            };

            List<Func<Models.Product, bool>> conditions = new()
            {
                p => p.Keywords.Any(k=>k.Name.Contains(keywordName))
            };
            KeywordNameSearch = keywordName;
            Products = await ProductService.GetAllDataAsync(includeProperties, conditions);
            Products = Products.OrderBy(p => p.Name);
            return Page();
        }

        public async Task<IActionResult> OnGetResetAsync()
        {
            SortProperty = null;
            SortDirection = false;
            Colour = null;
            Price1 = null;
            Price2 = null;
            Products = (await ProductService.GetProductsAsync()).OrderBy(p => p.Name);
            return Page();
        }

        public async Task OnGetSearchStringAsync(string searchString)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            List<Func<Models.Product, bool>> conditions = new();
            if(Colour != null)
            {
                conditions.Add(p => p.Colour.ToLower().Contains(Colour.ToLower()));
            }
            if(Price1 != null || Price2 != null)
            {
                int min = Math.Min(Convert.ToInt32(Price1), Convert.ToInt32(Price2));
                int maks = Math.Max(Convert.ToInt32(Price1), Convert.ToInt32(Price2));
                conditions.Add(p => p.Price >= min && p.Price <= maks);
            }
            if (KeywordNameSearch != null)
            {
                conditions.Add(p => p.Keywords.Any(k=>k.Name.ToLower().Contains(KeywordNameSearch.ToLower())));
            }

            List<string> includeProperties = new()
            {
                nameof(Models.Product.Keywords)
            };

            //the commented piece of code throws an exception because notracking cycle of object instantiation
            //includeProperties.Add($"{nameof(Models.Product.Keywords)}.{nameof(Models.Keyword.Products)}");

            Products = await ProductService.GetAllDataAsync(includeProperties, conditions);
            Products = await ProductService.OrderBy(Products, SortProperty, SortDirection);

            return Page();
        }

        //public async Task OnGetShowDisabledAsync()
        //{
        //    Products = (await ProductService.GetProductsAsync()).OrderBy(p => p.Name);
        //}
    }
}
