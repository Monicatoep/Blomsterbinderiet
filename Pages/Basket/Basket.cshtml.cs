using Blomsterbinderiet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Text.Json;

namespace Blomsterbinderiet.Pages.Basket
{
    public class BasketModel : PageModel
    {
        public void OnGet()
        {


            IEnumerable<BasketItem> temp = null;
            var cookieValue = Request.Cookies["BlomsterBinderietBasket"];
            if(!String.IsNullOrWhiteSpace(cookieValue))
            {
                temp = JsonSerializer.Deserialize<IEnumerable<BasketItem>>(cookieValue);
            }
            


            //Console.WriteLine(cookieValue);
            if(temp != null)
            {
                foreach (BasketItem helloworld in temp)
                {
                    Console.WriteLine(helloworld);
                }
            }
        }
    }
}
