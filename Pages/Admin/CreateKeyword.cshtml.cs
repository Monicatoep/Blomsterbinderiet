using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Blomsterbinderiet.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class CreateKeywordModel : PageModel
    {
        public KeywordService KeywordService { get; set; }
        
        public IEnumerable<Keyword> Keywords { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Du skal indtaste et navn")]
        public string Name { get; set; }

        public CreateKeywordModel(KeywordService keywordService)
        {
            KeywordService = keywordService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAddKeywordAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await KeywordService.AddKeywordAsync(new Keyword(Name));
            return RedirectToPage("GetAllKeywords");
        }
    }
}
