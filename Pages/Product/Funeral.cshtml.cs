using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

namespace Blomsterbinderiet.Pages.Product
{
    public class FuneralModel : PageModel
    {

        private ProductService ProductService { get; set; }
        private CookieService CookieService { get; set; }
        [BindProperty]
        [DisplayName("Sorter efter")]
        public string? SortProperty { get; set; }
        [BindProperty]
        [DisplayName("St�rst til mindst?")]
        public bool SortDirection { get; set; }
        [BindProperty]
        [DisplayName("Navn skal indeholde")]
        public string? SearchString { get; set; }
        [BindProperty]
        [DisplayName("Farve")]
        public string? Colour { get; set; }
        [BindProperty]
        [DisplayName("Minimum")]
        public double? MinimumPrice { get; set; }
        [BindProperty]
        [DisplayName("Maksimum")]
        public double? MaksimumPrice { get; set; }
        [BindProperty]
        [DisplayName("S�g p� produkt attribut")]
        public string? KeywordNameSearch { get; set; }
        [BindProperty]
        [DisplayName("Vis deaktiverede produkter")]
        public bool ShowDisabled { get; set; }
        public IEnumerable<Models.Product> Products { get; private set; }
        public string Message { get; set; }
        public int ID { get; set; }

        public FuneralModel(ProductService productService, CookieService cookieService)
        {
            ProductService = productService;
            CookieService = cookieService;
        }

        public async Task OnGetAsync()
        {
            Products = await ProductService.GetAllProductsIncludeKeywordsAsync();
            Products = Products.Where(p => p.Keywords.Any(k => k.Name.Contains("Begravelse")));
            Products = Products.Where(p => p.Disabled == false);
            Products = Products.OrderBy(p => p.Name);
        }

        public async Task<IActionResult> OnGetResetAsync()
        {
            SortProperty = null;
            SortDirection = false;
            Colour = null;
            MinimumPrice = null;
            MaksimumPrice = null;
            ShowDisabled = false;
            Products = await ProductService.GetAllProductsIncludeKeywordsAsync();
            Products = Products.Where(p => p.Keywords.Any(k => k.Name.Contains("Begravelse")));
            Products = Products.Where(p => p.Disabled == false);
            Products = Products.OrderBy(p => p.Name);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Products = await ProductService.GetAllProductsFilteredAsync(SearchString, Colour, MinimumPrice, MaksimumPrice, KeywordNameSearch, ShowDisabled);
            Products = ProductService.Sort(Products, SortProperty, SortDirection);
            Products = Products.Where(p => p.Keywords.Any(k => k.Name.Contains("Begravelse")));
            //Products.OrderBy(p => p.Disabled);

            return Page();
        }

        public async Task<IActionResult> OnPostAddToBasketAsync(int id)
        {
            CookieService.PlusOne(Request.Cookies, Response.Cookies, id);

            Products = await ProductService.GetAllProductsIncludeKeywordsAsync();
            Products = Products.Where(p => p.Keywords.Any(k => k.Name.Contains("Begravelse")));
            Products = Products.Where(p => p.Disabled == false);
            Products = Products.OrderBy(p => p.Name);
            Message = $"Tilf�jede produkt til kurven";
            ID = id;
            return Page();
        }
    }
}


