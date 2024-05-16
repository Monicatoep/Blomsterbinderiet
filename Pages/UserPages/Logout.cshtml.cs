using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blomsterbinderiet.Service;

namespace Blomsterbinderiet.Pages.Customer
{
    public class LogoutModel : PageModel
    {
        private CookieService CookieService { get; set; }

        public LogoutModel(CookieService cookieService)
        {
            this.CookieService = cookieService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            CookieService.SaveCookie(Response.Cookies, null);
            return RedirectToPage("/index");
        }
    }
}
