using Blomsterbinderiet.Models;
using Blomsterbinderiet.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blomsterbinderiet.Pages.Admin
{
    public class UpdateKeywordModel : PageModel
    {
        [BindProperty]
        public Keyword Keyword { get; set; }

        public string Confirmation { get; set; }

        public KeywordService KeywordService { get; set; }

        public UpdateKeywordModel(KeywordService keywordService)
        {
            KeywordService = keywordService;
        }

        public void OnGet(int id)
        {
            Keyword = KeywordService.GetKeywordByIdAsync(id).Result;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Confirmation = "Opdatering fejlede";
                return Page();
            }
            await KeywordService.UpdateKeywordAsync(Keyword);
            Confirmation = "Opdaterede produktet";
            return Page();
        }


    }
}
