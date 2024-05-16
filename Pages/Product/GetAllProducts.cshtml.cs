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
        private CookieService CookieService { get; set; }
        [BindProperty]
        [DisplayName("Sorter efter")]
        public string? SortProperty { get; set; }
        [BindProperty]
        [DisplayName("Navn skal indeholde")]
        public string? SearchString { get; set; }
        [BindProperty]
        [DisplayName("Størst til mindst?")]
        public bool SortDirection { get; set; }
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
            MinimumPrice = null;
            MaksimumPrice = null;
            ShowDisabled = false;
            Products = await ProductService.GetAllProductsStandardFilterAndSort();
            return Page();
        }

        public async Task<IActionResult> OnGetSearchStringAsync(string searchString)
        {
            Products = await ProductService.GetAllProductsStandardFilterAndSort();
            SearchString = searchString;
            searchString = searchString.ToLower();
            //set Products to only contain where either product name or searchString is part of one another
            Products = Products.Where(p => p.Name.ToLower().Contains(searchString) || searchString.Contains(p.Name.ToLower()));

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Products = await ProductService.GetAllProductsFiltered(SearchString, Colour,MinimumPrice,MaksimumPrice,KeywordNameSearch,ShowDisabled);
            Products = ProductService.Sort(Products, SortProperty, SortDirection);
            //Products.OrderBy(p => p.Disabled);

            return Page();
        }

        public async Task<IActionResult> OnPostAddToBasket(int id)
        {
            CookieService.PlusOne(Request.Cookies, Response.Cookies, id);

            Products = await ProductService.GetAllProductsStandardFilterAndSort();
            return Page();
        }
    }
}
