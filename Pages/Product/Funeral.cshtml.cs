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
        [DisplayName("Størst til mindst?")]
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
        [DisplayName("Søg på produkt attribut")]
        public string? KeywordNameSearch { get; set; }
        [BindProperty]
        [DisplayName("Vis deaktiverede produkter")]
        public bool ShowDisabled { get; set; }
        [BindProperty]
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
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
            CurrentPage = 1;
            SortProperty = nameof(Models.Product.Name);
            await FilterSort();
        }

        public async Task<IActionResult> OnGetResetAsync()
        {
            SortProperty = nameof(Models.Product.Name);
            SortDirection = false;
            Colour = null;
            MinimumPrice = null;
            MaksimumPrice = null;
            ShowDisabled = false;
            CurrentPage = 1;
            await FilterSort();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await FilterSort();

            return Page();
        }

        public async Task<IActionResult> OnPostNewPageAsync(int pageNumber)
        {
            CurrentPage = pageNumber;
            await FilterSort();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToBasketAsync(int id)
        {
            await FilterSort();
            CookieService.PlusOne(Request.Cookies, Response.Cookies, id);
            Message = $"Tilføjede produkt til kurven";
            ID = id;
            return Page();
        }

        private async Task FilterSort()
        {
            Products = await ProductService.GetAllProductsFiltered(SearchString, Colour, MinimumPrice, MaksimumPrice, KeywordNameSearch, ShowDisabled);
            Products = Products.Where(p => p.Keywords.Any(k => k.Name.Contains("Begravelse")));
            Products = ProductService.Sort(Products, SortProperty, SortDirection);
            Products = Products.OrderBy(p => p.Disabled);
            PageCount = (int)Math.Ceiling(Products.Count() / 6d);
            Products = Products.Skip((CurrentPage - 1) * 6).Take(6);
        }
    }
}


