using Blomsterbinderiet.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text.Json;

namespace Blomsterbinderiet.Service
{
    public class CookieService
    {
        private ProductService ProductService { get; set; }
        //https://www.learnrazorpages.com/razor-pages/cookies
        private readonly string _cookieName = "BlomsterBinderietBasket";
        public CookieService(ProductService productService)
        {
            ProductService = productService;
        }

        public ICollection<BasketItem>? ReadCookie(IRequestCookieCollection input)
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

        public void SaveCookie(IResponseCookies output, IEnumerable<BasketItem> listOfItems)
        {
            string jsonString = JsonSerializer.Serialize(listOfItems);
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(60)
            };
            output.Append(_cookieName, jsonString, cookieOptions);
        }

        public async Task<List<OrderLine>>? LoadOrderLinesAsync(IEnumerable<BasketItem> basket)
        {
            if (basket == null)
            {
                return null;
            }
            List<OrderLine> orderLines = new();

            foreach (BasketItem BItem in basket)
            {
                Product line = await ProductService.GetProductByIdAsync(BItem.ProductID);
                OrderLine Temporary = new() { Amount = BItem.Amount, Product = line };
                orderLines.Add(Temporary);
            }

            return orderLines;
        }

        public IEnumerable<BasketItem>? ChangeAmount(IRequestCookieCollection input, IResponseCookies output, int id, int amount)
        {
            ICollection<BasketItem> data = ReadCookie(input);
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
            data = AddItem(data, id, amount);
        Found:
            SaveCookie(output, data);
            return data;
        }

        public ICollection<BasketItem> AddItem(ICollection<BasketItem> basketItems, int id, int amount)
        {
            if (basketItems == null)
            {
                basketItems = new List<BasketItem>();
            }
            basketItems.Add(new() { ProductID = id, Amount = amount });
            return basketItems;
        }

        public IEnumerable<BasketItem>? MinusOne(IRequestCookieCollection input, IResponseCookies output, int id)
        {
            return ChangeAmount(input, output, id, -1);
        }

        public IEnumerable<BasketItem>? PlusMany(IRequestCookieCollection input, IResponseCookies output, int id, int amount)
        {
            return ChangeAmount(input, output, id, amount);
        }

        public IEnumerable<BasketItem>? PlusOne(IRequestCookieCollection input, IResponseCookies output, int id)
        {
            return ChangeAmount(input, output, id, 1);
        }

        public ClaimsIdentity? Login(IEnumerable<User> listOfUsers, string email, string password)
        {
            foreach (User user in listOfUsers)
            {
                if (email == user.Email && user.State == "Aktiv")
                {
                    var passwordHasher = new PasswordHasher<string>();
                    if (passwordHasher.VerifyHashedPassword(null, user.Password, password) == PasswordVerificationResult.Success)
                    {
                        var claims = new List<Claim> 
                        { 
                            new(ClaimTypes.Name, user.ID.ToString()),
                            new(ClaimTypes.Role, user.Role),
                            new(ClaimTypes.Email, email)
                        };
                        return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    }
                }
            }
            return null;
        }

        public IEnumerable<BasketItem> RemoveBasketItem(IRequestCookieCollection input, IResponseCookies output, int id)
        {
            ICollection<BasketItem> basketItems = ReadCookie(input);
            BasketItem toBeRemoved = null;
            foreach(BasketItem item in basketItems)
            {
                if(item.ProductID == id)
                {
                    toBeRemoved = item;
                    break;
                }
            }
            basketItems.Remove(toBeRemoved);
            SaveCookie(output, basketItems);
            return basketItems;
        }
    }
}