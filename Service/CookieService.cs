using Azure;
using Azure.Core;
using Blomsterbinderiet.Migrations;
using Blomsterbinderiet.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text.Json;

namespace Blomsterbinderiet.Service
{
    public class CookieService
    {
        //https://www.learnrazorpages.com/razor-pages/cookies

        private string _cookieName = "BlomsterBinderietBasket";

        public ProductService ProductService { get; set; }

        public CookieService(ProductService productService)
        {
            ProductService = productService;
        }

        public async Task<ICollection<BasketItem>> ReadCookieAsync(IRequestCookieCollection input)
        {
            var cookieValue = input[_cookieName];
            if (String.IsNullOrWhiteSpace(cookieValue))
            {
                return null;
            }
            else
            {
                return JsonSerializer.Deserialize<ICollection<BasketItem>>(cookieValue);
            }
        }

        public async Task SaveCookieAsync(IResponseCookies input, IEnumerable<BasketItem> listOfItems)
        {
            string jsonString = JsonSerializer.Serialize(listOfItems);
            input.Append(_cookieName, jsonString);
        }

        public async Task<IEnumerable<OrderLine>> LoadOrderLinesAsync(IEnumerable<BasketItem> basket)
        {
            if (basket == null )
            {
                return null;
            }
            List<OrderLine> orderLines = new();

            foreach (BasketItem BItem in basket)
            {
                Product line = ProductService.GetProductByIdAsync(BItem.ProductID).Result;
                OrderLine Temporary = new() { Amount = BItem.Amount, Product = line };
                orderLines.Add(Temporary);
            }

            return orderLines;
        }

        public async Task<IEnumerable<BasketItem>?> ChangeAmountAsync(IRequestCookieCollection input, IResponseCookies output, int id, int amount)
        {
            ICollection<BasketItem> data = await ReadCookieAsync(input);
            BasketItem temp;
            if (data != null)
            {
                int length = data.Count;
                for (int i = 0; i < length; i++)
                {
                    temp = data.ElementAt(i);

                    if (temp.ProductID == id)
                    {
                        temp.Amount += amount;
                        if (temp.Amount <= 0)
                        {
                            data.Remove(temp);
                        }
                        goto Found;
                    }
                }
            }
            data = await AddItem(data, id, amount);
            Found:
            await SaveCookie(output, data);
            return data;
        }

        public async Task<ICollection<BasketItem>> AddItem(ICollection<BasketItem> basketItems, int id, int amount)
        {
            Console.WriteLine("Tilføjer" + id + " " + amount);
            if(basketItems == null)
            {
                basketItems = new List<BasketItem>();
            }
            basketItems.Add(new() { ProductID = id, Amount = amount });
            return basketItems;
        }

        public async Task<IEnumerable<BasketItem>?> MinusOne(IRequestCookieCollection input, IResponseCookies output, int id)
        {
            return await ChangeAmountAsync(input, output, id, -1);
        }

        public async Task<IEnumerable<BasketItem>?> PlusManyAsync(IRequestCookieCollection input, IResponseCookies output, int id, int amount)
        {
            return await ChangeAmountAsync(input, output, id, amount);
        }

        public async Task<IEnumerable<BasketItem>?> PlusOneAsync(IRequestCookieCollection input, IResponseCookies output, int id)
        {
            return await ChangeAmountAsync(input, output, id, 1);
        }

        public async Task<ClaimsIdentity> LoginAsync(HttpContext context,IEnumerable<User> listOfUsers, string email, string password)
        {
            foreach (User user in listOfUsers)
            {
                if (email == user.Email && user.State == "Aktiv")
                {
                    var passwordHasher = new PasswordHasher<string>();
                    if (passwordHasher.VerifyHashedPassword(null, user.Password, password) == PasswordVerificationResult.Success)
                    {
                        var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.ID.ToString()) };
                        claims.Add(new Claim(ClaimTypes.Role, user.Role));
                        claims.Add(new Claim(ClaimTypes.Email, email));

                        return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    }
                }
            }
            return null;
        }
    }
}
