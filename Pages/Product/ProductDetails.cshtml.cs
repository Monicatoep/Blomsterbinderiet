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
        public Models.Product Product { get; set; }

        public ProductDetailsModel(DbGenericService<Models.Product> service)
        {
            this.service = service;
        }

        public void OnGet(int id)
        {
            Product = service.GetObjectByIdAsync(id).Result;
        }

        //cookies can only store string values
        public IActionResult OnPost()
        {
            var cookieValue = Request.Cookies["BlomsterBinderietBasket"];
            //Console.WriteLine(cookieValue);
            List<BasketItem> temp;
            if (String.IsNullOrWhiteSpace(cookieValue))
            {
                //Console.WriteLine("Cookie var tom");
                temp = new();
                temp.Add(new() { ProductID = _ProductID, Amount = _Amount });
            } else
            {
                //Console.WriteLine("Cookie var ikke tom");
                temp = JsonSerializer.Deserialize<List<BasketItem>>(cookieValue);
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
            string jsonString = JsonSerializer.Serialize(temp);
            Response.Cookies.Append("BlomsterBinderietBasket", jsonString);

            

            //var test = HttpContext.Request.Cookies;

            //var authenticateResult = HttpContext.AuthenticateAsync();

            //if (authenticateResult.Result.Succeeded)
            //{
               
            //}
            //claimsIdentity = (ClaimsIdentity)authenticateResult.Result.Principal.Identity;
            //Console.WriteLine(claimsIdentity);
            //if (!claimsIdentity.HasClaim(c => c.Type == "your-claim"))
            //{
            //    claimsIdentity.AddClaim(new Claim("your-claim", "your-value"));

            //    HttpContext.SignInAsync(authenticateResult.Result.Principal, authenticateResult.Result.Properties);
            //}

            //claims.Add(new Claim(ClaimTypes.Role, temp.AccessLevel));

            return RedirectToPage("/Basket/Basket");
        }
    }
}
