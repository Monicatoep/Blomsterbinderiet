using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class GetAllKeywordsModel : PageModel
    { 
        public KeywordService KeywordService { get; set; }
        public IEnumerable<Keyword> Keywords { get; set; }

        [BindProperty]
        public string Name { get; set; }

        public GetAllKeywordsModel(KeywordService keywordService)
        {
            KeywordService = keywordService;
        }

        public void OnGet()
        {
            Keywords = KeywordService.GetAllKeywordsAsync().Result.ToList();
        }

        //public async Task<IActionResult> OnPostAddKeywordAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }
        //    await KeywordService.AddKeywordAsync(new Keyword(Name));
        //}

    }
}
