using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Models;
using Blomsterbinderiet.MockData;
using Blomsterbinderiet.Service;
using System.ComponentModel;
using System.Linq.Expressions;
using Blomsterbinderiet.Migrations;
using System;

namespace Blomsterbinderiet.Pages.Product
{
    public class GetAllProductsModel : PageModel
    {
        private ProductService ProductService { get; set; }
        public CookieService CookieService { get; set; }
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
        public double? Price1 { get; set; }
        [BindProperty]
        public double? Price2 { get; set; }
        [BindProperty]
        [DisplayName("Søg på produkt attribut")]
        public string? KeywordNameSearch { get; set; }
        [BindProperty]
        [DisplayName("Vis deaktiverede produkter")]
        public bool ShowDisabled { get; set; }
        public IEnumerable<Models.Product> Products { get; private set; }

        public GetAllProductsModel(ProductService productService, CookieService cookieService)
        {
            ProductService = productService;
            CookieService = cookieService;
        }

		public async Task OnGetAsync()
        {
            Products = await ProductService.GetAllProductsStandardFilterAndSort();
        }

        public async Task<IActionResult> OnGetKeywordAsync(string keywordName)
        {
            Products = await ProductService.GetAllProductsIncludeKeywordsAsync();

            Products = Products.Where(p => p.Keywords.Any(k => k.Name.Contains(keywordName)));
            Products = Products.Where(p => p.Disabled == false);
            Products = Products.OrderBy(p => p.Name);

            KeywordNameSearch = keywordName;
            
            return Page();
        }

        public async Task<IActionResult> OnGetResetAsync()
        {
            SortProperty = null;
            SortDirection = false;
            Colour = null;
            Price1 = null;
            Price2 = null;
            ShowDisabled = false;
            Products = await ProductService.GetAllProductsStandardFilterAndSort();
            return Page();
        }

        public async Task OnGetSearchStringAsync(string searchString)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Products = await ProductService.GetAllProductsFilteredAndSorted(Colour,Price1,Price2,KeywordNameSearch,ShowDisabled,SortProperty,SortDirection);

            return Page();
        }

        public async Task<IActionResult> OnPostAddToBasket(int id)
        {
            await CookieService.PlusOneAsync(Request.Cookies, Response.Cookies, id);

            Products = await ProductService.GetAllProductsStandardFilterAndSort();
            return Page();
        }
    }
}
