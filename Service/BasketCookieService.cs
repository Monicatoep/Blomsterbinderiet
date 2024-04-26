using Azure;
using Azure.Core;
using Blomsterbinderiet.Migrations;
using Blomsterbinderiet.Models;
using System.Text.Json;

namespace Blomsterbinderiet.Service
{
    public class BasketCookieService
    {
        //https://www.learnrazorpages.com/razor-pages/cookies

        private string _cookieName = "BlomsterBinderietBasket";

        public ProductService ProductService { get; set; }

        public BasketCookieService(ProductService productService)
        {
            ProductService = productService;
        }

        public async Task<ICollection<BasketItem>> ReadCookie(IRequestCookieCollection input)
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

        public async Task SaveCookie(IResponseCookies input, IEnumerable<BasketItem> listOfItems)
        {
            string jsonString = JsonSerializer.Serialize(listOfItems);
            input.Append(_cookieName, jsonString);
        }

        public async Task<IEnumerable<OrderLine>> LoadOrderLines(IEnumerable<BasketItem> basket)
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

        public async Task<IEnumerable<BasketItem>?> PlusOne(IRequestCookieCollection input, IResponseCookies output, int id)
        {
            ICollection<BasketItem> data = await ReadCookie(input);
            BasketItem temp;
            if (data != null)
            {
                int length = data.Count;
                for (int i = 0; i < length; i++)
                {
                    temp = data.ElementAt(i);

                    if (temp.ProductID == id)
                    {
                        temp.Amount += 1;
                        if (temp.Amount <= 0)
                        {
                            data.Remove(temp);
                        }
                        goto found;
                    }
                }
            }
            return data;
            found:
            await SaveCookie(output, data);
            return data;
        }

        public async Task<IEnumerable<BasketItem>?> MinusOne(IRequestCookieCollection input, IResponseCookies output, int id)
        {
            ICollection<BasketItem> data = await ReadCookie(input);
            BasketItem temp;
            if (data != null)
            {
                int length = data.Count;
                for(int i=0; i< length; i++)
                {
                    temp = data.ElementAt(i);

                    if (temp.ProductID == id)
                    {
                        temp.Amount -= 1;
                        if (temp.Amount <=0)
                        {
                            data.Remove(temp);
                        }
                        goto found;
                    }
                }
            }
            return data;
            found:
            await SaveCookie(output, data);
            return data;
        }
    }
}
