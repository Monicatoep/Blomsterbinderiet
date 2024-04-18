using Azure;
using Azure.Core;
using Blomsterbinderiet.Models;
using System.Text.Json;

namespace Blomsterbinderiet.Service
{
    public class BasketCookieService
    {
        //https://www.learnrazorpages.com/razor-pages/cookies

        private string _cookieName = "BlomsterBinderietBasket";

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

        public void SaveCookie(IResponseCookies input, ICollection<BasketItem> listOfItems)
        {
            string jsonString = JsonSerializer.Serialize(listOfItems);
            input.Append(_cookieName, jsonString);
        }
    }
}
