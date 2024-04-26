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

        public ICollection<BasketItem> ReadCookie(IRequestCookieCollection input)
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

        public void SaveCookie(IResponseCookies input, IEnumerable<BasketItem> listOfItems)
        {
            string jsonString = JsonSerializer.Serialize(listOfItems);
            input.Append(_cookieName, jsonString);
        }

        public IEnumerable<OrderLine> LoadOrderLines(IRequestCookieCollection input)
        {
            if (ReadCookie(input) == null )
            {
                return null;
            }
            List<BasketItem> basketItems = ReadCookie(input).ToList();
            List<OrderLine> orderLines = new();

            foreach (BasketItem BItem in basketItems)
            {
                Product line = ProductService.GetProductByIdAsync(BItem.ProductID).Result;
                OrderLine Temporary = new() { Amount = BItem.Amount, Product = line };
                orderLines.Add(Temporary);
            }

            return orderLines;
        }

        public void PlusOne(IRequestCookieCollection input, IResponseCookies output, int id)
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
                        temp.Amount += 1;
                        if (temp.Amount <= 0)
                        {
                            data.Remove(temp);
                        }
                        goto found;
                    }
                }
            }
            return;
        found:
            SaveCookie(output, data);
        }

        public void MinusOne(IRequestCookieCollection input, IResponseCookies output, int id)
        {
            ICollection<BasketItem> data = ReadCookie(input);
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
            return;
            found:
            SaveCookie(output, data);
        }
    }
}
