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
        public CookieService CookieService { get; set; }
        public IEnumerable<Models.Product> Products { get; private set; }

        public GetAllProductsModel(ProductService productService, CookieService cookieService)
        {
            ProductService = productService;
            CookieService = cookieService;
        }

		public async Task OnGetAsync()
        {
            Products = await ProductService.GetAllProductsAsync();

            Products = Products.Where(p => p.Disabled == false);
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
            Products = (await ProductService.GetAllProductsAsync()).Where(p => p.Disabled == false).OrderBy(p => p.Name);
            return Page();
        }

        public async Task OnGetSearchStringAsync(string searchString)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Products = await ProductService.GetAllProductsIncludeKeywordsAsync();

            if(!ShowDisabled)
            {  
                Products = from product in Products 
                           where product.Disabled == false 
                           select product;
            }

            if (Colour != null)
            {
                Products = Products.Where(p => p.Colour.ToLower().Contains(Colour.ToLower()));
            }
            if(Price1 != null || Price2 != null)
            {
                int min = Math.Min(Convert.ToInt32(Price1), Convert.ToInt32(Price2));
                int maks = Math.Max(Convert.ToInt32(Price1), Convert.ToInt32(Price2));
                Products = Products.Where(p => p.Price >= min && p.Price <= maks);
            }
            if (KeywordNameSearch != null)
            {
                Products = Products.Where(p => p.Keywords.Any(k=>k.Name.ToLower().Contains(KeywordNameSearch.ToLower())));
            }
            
            Products = ProductService.Sort(Products, SortProperty, SortDirection);
            Products = Products.OrderByDescending(p => p.Disabled);

            return Page();
        }

        public async Task<IActionResult> OnPostAddToBasket(int id)
        {
            await CookieService.PlusOneAsync(Request.Cookies, Response.Cookies, id);

            Products = (await ProductService.GetAllProductsAsync()).Where(p => p.Disabled == false).OrderBy(p => p.Name);
            return Page();
        }
    }
}
