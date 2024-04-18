using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Blomsterbinderiet.Service;
using Blomsterbinderiet.Models;
using System.Text.Json;

namespace Blomsterbinderiet.Pages.Product
{
    public class ProductDetailsModel : PageModel
    {
        [BindProperty]
        public int _Amount { get; set; }
        [BindProperty]
        public int _ProductID { get; set; }

        public DbGenericService<Models.Product> service { get; set; }
        public BasketCookieService cookieService { get; set; }
        public Models.Product Product { get; set; }

        public ProductDetailsModel(DbGenericService<Models.Product> service, BasketCookieService cookieService)
        {
            this.service = service;
            this.cookieService = cookieService;
        }

        public void OnGet(int id)
        {
            Product = service.GetObjectByIdAsync(id).Result;
        }

        //cookies can only store string values
        public IActionResult OnPost()
        {
            ICollection<BasketItem> temp = cookieService.ReadCookie(Request.Cookies);
            
            if (temp == null)
            {
                //Console.WriteLine("Cookie var tom");
                temp = new List<BasketItem>();
                temp.Add(new() { ProductID = _ProductID, Amount = _Amount });
            } else
            {
                //Console.WriteLine("Cookie var ikke tom");
                foreach(var i in temp)
                {
                    if(i.ProductID == _ProductID)
                    {
                        i.Amount += _Amount;
                        goto Found;
                    }
                }
                temp.Add(new() { ProductID = _ProductID, Amount = _Amount });
            }
            Found:
            cookieService.SaveCookie(Response.Cookies, temp);
            return RedirectToPage("/Basket/Basket");
        }
    }
}
