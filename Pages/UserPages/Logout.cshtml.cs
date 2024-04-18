using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Service;

namespace Blomsterbinderiet.Pages.Customer
{
    public class LogoutModel : PageModel
    {
        public BasketCookieService cookieService { get; set; }

        public LogoutModel(BasketCookieService cookieService)
        {
            this.cookieService = cookieService;
        }

        public async Task<IActionResult> OnGet()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            cookieService.SaveCookie(Response.Cookies, null);
            return RedirectToPage("/index");
        }
    }
}
