using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Blomsterbinderiet.Pages.Product
{
    public class ProductDetailsModel : PageModel
    {
        [BindProperty]
        public int Amount { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var test = HttpContext.Request.Cookies;
            foreach (var idk in HttpContext.Request.Cookies)
            {
                Console.WriteLine(idk);
            }

            var claims = new List<Claim> { 
                new Claim("basketItem", "1") 
            };
            //claims.Add(new Claim(ClaimTypes.Role, temp.AccessLevel));
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return RedirectToPage("/Basket/Basket");
        }
    }
}
